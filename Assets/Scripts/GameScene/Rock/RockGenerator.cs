using System;
using System.Collections.Generic;
using UnityEngine;

public class RockGenerator : MonoBehaviour
{
    private System.Random rnd = new();
    
    [SerializeField]
    public float minZDistance = -400.0f;

    [SerializeField]
    public float maxZDistance = -1900.0f;

    [SerializeField]
    public float minXDistance = -1000.0f;

    [SerializeField]
    public float maxXDistance = 1000.0f;

    [SerializeField]
    public float minYDeviation = 0;

    [SerializeField]
    public float maxYDeviation = -5.0f;

    [SerializeField]
    public float minRotation = 0;

    [SerializeField]
    public float maxRotation = 360;

    [SerializeField]
    public float maxScale = 5.0f;

    [SerializeField]
    public float minScale = 1.5f;

    private List<GameObject> rocksPrefab;
    private List<GameObject> rocksObject = new();

    void Start()
    {
        GetPrefabs();
        SpawnRock();
    }

    private void GetPrefabs()
    {
        rocksPrefab = new List<GameObject> 
        {
            (GameObject)Resources.Load("Prefabs/Level/Rocks/Rock_1"),
            (GameObject)Resources.Load("Prefabs/Level/Rocks/Rock_2"),
            (GameObject)Resources.Load("Prefabs/Level/Rocks/Rock_3")
        };
    }

    private void SpawnRock()
    {
        var countParallels = rnd.Next(5, 15);
        var zStep = CalculateStep(minZDistance, maxZDistance, countParallels);

        var countMeridians = rnd.Next(50, 100);
        var xStep = CalculateStep(minXDistance, maxXDistance, countMeridians);

        for(var i = 0; i < countParallels; i++)
        {
            for(var j = 0; j < countMeridians; j++)
            {
                var probability = rnd.NextDouble();

                if(probability > 0.6)
                {
                    var zPosition = CalculatePosition(i, zStep, minZDistance);
                    var xPosition = CalculatePosition(j, xStep, minXDistance);

                    var yDeviation = CalculateFloatValue(maxYDeviation, minYDeviation);

                    var rotation = CalculateFloatValue(minRotation, maxRotation);

                    var scale = CalculateFloatValue(minScale, maxScale);

                    var element = rnd.Next(0, rocksPrefab.Count);

                    GameObject rockGameObject = Instantiate(
                        rocksPrefab[element],
                        new(xPosition, yDeviation, zPosition),
                        Quaternion.Euler(0, rotation, 0),
                        gameObject.transform);

                    rockGameObject.transform.localScale = new(scale, scale, scale);
                    rocksObject.Add(rockGameObject);
                }
            }
        }
    }

    private float CalculateStep(float min, float max, int count)
    {
        return (max - min) / count;
    }

    private float CalculatePosition(int iter, float step, float distance)
    {
        var deviation = rnd.NextDouble() * step * 2 - step;
        var position = iter * step + deviation + distance;

        return (float)position;
    }

    private float CalculateFloatValue(float left, float right)
    {
        return (float)(rnd.NextDouble() * (right - left) + left);
    }
}
