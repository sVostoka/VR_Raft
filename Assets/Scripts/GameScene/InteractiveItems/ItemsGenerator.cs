using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemsGenerator : MonoBehaviour
{
    private System.Random rnd = new();

    [SerializeField]
    private GameObject raft;
    private Water water;

    private struct ItemPrefab
    {
        public GameObject gameObject { get; set; }
        public float probability { get; set; }

        public ItemPrefab(GameObject gameObject, float probability)
        {
            this.gameObject = gameObject;
            this.probability = probability;
        }
    }

    private List<ItemPrefab> itemsPrefab;

    //Генерит ошибки
    //[SerializeField]
    public float startTime = 0.0f;

    //Генерит ошибки
    //[SerializeField]
    public float repeatRate = 2.0f;

    private const string NAMEREPEATFUNCTION = "SpawnItems";

    //Генерит ошибки
    //[SerializeField]
    public float minXSpawnPosition = -20.0f;

    //Генерит ошибки
    //[SerializeField]
    public float maxXSpawnPosition = 20.0f;

    //Генерит ошибки
    //[SerializeField]
    public float distanceSpawn = 40.0f;

    void Start()
    {
        water = FindFirstObjectByType<Water>();
        
        GetPrefabs();

        InvokeRepeating(NAMEREPEATFUNCTION, startTime, repeatRate);
    }

    private void GetPrefabs()
    {
        itemsPrefab = new()
        {
            new ItemPrefab((GameObject)Resources.Load("Prefabs/Level/InteractiveItems/Paddle"), 0.1f),
            new ItemPrefab((GameObject)Resources.Load("Prefabs/Level/InteractiveItems/Net"), 0.2f),
            new ItemPrefab((GameObject)Resources.Load("Prefabs/Level/InteractiveItems/Stick"), 0.8f),
            new ItemPrefab((GameObject)Resources.Load("Prefabs/Level/InteractiveItems/Fishes/Fish_1"), 0.7f),
            new ItemPrefab((GameObject)Resources.Load("Prefabs/Level/InteractiveItems/Fishes/Fish_2"), 0.7f),
        };
    }

    private void SpawnItems()
    {
        var zPosition = raft.transform.position.z - distanceSpawn;
        var yPosition = water.transform.position.y + 0.2f;

        var maxX = raft.transform.position.x + maxXSpawnPosition;
        var minX = raft.transform.position.x - Math.Abs(minXSpawnPosition);
        var xPosition = (float)(rnd.NextDouble() * (maxX - minX) + minX);

        var probability = (float)rnd.NextDouble();

        try
        {
            GameObject spawnItem = Instantiate(
                GetProbPrefab(probability),
                new(xPosition, yPosition, zPosition),
                Quaternion.identity,
                gameObject.transform
            );
        }
        catch { }
    }

    private GameObject GetProbPrefab(float probability)
    {
        List<GameObject> result = new();

        foreach (var item in itemsPrefab)
        {
            if (item.probability > probability)
            {
                result.Add(item.gameObject);
            }
        }

        if (result.Count != 0)
        {
            var element = rnd.Next(0, result.Count);

            return result[element];
        }
        else
        {
            throw new Exception("List is empty");
        }
    }
}
