using UnityEngine;

public class WindController : MonoBehaviour
{
    private System.Random random = new();

    [SerializeField]
    private float timer = 0;

    [SerializeField]
    private float coolDown;

    [SerializeField]
    private float windChance;
    
    public bool isWindy;

    public SailController sail;

    void Start()
    {
        sail = FindObjectOfType<SailController>();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            timer = coolDown;
            var chance = random.NextDouble();

            if (windChance > chance)
            {
                isWindy = !isWindy;
            }
                
        }

        sail.CheckWeather(isWindy);
    }

    public void ChangeSail()
    {
        sail = FindObjectOfType<SailController>();
    }
}
