using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemController : MonoBehaviour
{
    [SerializeField]
    private float immersionDistance = 0.1f;

    public XRGrabInteractable interactable;
    private MoveItems moveItems;
    private Rigidbody rb;

    private bool _inNat = false;

    public bool InNat
    {
        get => _inNat;
        set => _inNat = value;
    }

    private Water water;

    void Start()
    {
        moveItems = GetComponent<MoveItems>();
        interactable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        water = FindFirstObjectByType<Water>(); 
    }

    void FixedUpdate()
    {
        var deltaTime = Time.fixedDeltaTime;

        CheckSelectedItem();
        CheckWaterTouch();
        CheckHitNet();
        CheckInNet();
    }

    private void CheckHitNet()
    {
        if (InNat)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    private void CheckSelectedItem()
    {
        if (interactable.isSelected)
        {
            InNat = false;
            moveItems.IsMove = !interactable.isSelected;

            rb.isKinematic = false; 
            rb.useGravity = true;
        }

        if (!interactable.isSelected && !moveItems.IsMove && !InNat)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            //gameObject.transform.SetParent(null);
        }
    }

    private void CheckWaterTouch()
    {
        if (!moveItems.IsMove)
        {
            var yPossitionWater = water.transform.position.y;
            var yPossitionItem = transform.position.y;

            if((yPossitionWater - yPossitionItem) > immersionDistance)
            {
                moveItems.UpMove = true;
                moveItems.IsMove = true;
                moveItems.ResetRotation();                
                moveItems.CalculateTargetPosition();

                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }

    private void CheckInNet()
    {
        if (InNat)
        {
            moveItems.UpMove = false;
            moveItems.IsMove = false;
        }
    }
}
