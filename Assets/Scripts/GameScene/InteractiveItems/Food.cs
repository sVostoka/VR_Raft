using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField]
    private float _foodValue = 20.0f;

    public float FoodValue
    {
        get => _foodValue;
        set => _foodValue = value;
    }
}
