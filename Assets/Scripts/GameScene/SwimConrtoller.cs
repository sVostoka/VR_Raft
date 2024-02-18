using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SwimConrtoller : MonoBehaviour
{
    [SerializeField]
    private float immersionDistance = 0.2f;

    [SerializeField]
    private float constExpulsion = 2.0f;

    private Player player;
    private Water water;
    private DamageZone damageZone;

    private bool isSwimming = false;
    private Vector3 swimPosition;

    void Start()
    {
        player = FindAnyObjectByType<Player>();
        water = FindAnyObjectByType<Water>();
        damageZone = FindAnyObjectByType<DamageZone>();
    }

    void Update()
    {
        var yPossitionWater = water.transform.position.y;
        var yPossitionPlayer = transform.position.y;

        if ((yPossitionWater - yPossitionPlayer) > immersionDistance && !isSwimming)
        {
            isSwimming = true;
            swimPosition = player.transform.position;
        }

        if (isSwimming)
        {
            player.transform.position = swimPosition;
            damageZone.transform.position = swimPosition;
        }
    }
}
