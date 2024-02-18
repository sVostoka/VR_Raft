using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthController : MonoBehaviour
{
    [SerializeField]
    public GameObject mainCamera;

    void Update()
    {
        gameObject.transform.position = mainCamera.transform.position;
        gameObject.transform.rotation = mainCamera.transform.rotation;
    }
}
