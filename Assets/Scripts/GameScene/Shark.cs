using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shark : MonoBehaviour
{
    private System.Random random = new();

    private enum SharkState
    {
        Hunting,
        Attack,
        RunAway,
        Starting
    }

    private SharkState sharkState = SharkState.Starting;

    [SerializeField]
    private float angularSpeed;
    
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float immersionSpeed;

    [SerializeField]
    private float emersionSpeed;

    [SerializeField]
    private Vector3 startPosition;

    [SerializeField]
    private Quaternion startRotation;

    [SerializeField]
    private Vector3 runAwayPoint;

    [SerializeField]
    private float attackChance = 0.3f;

    [SerializeField]
    private float huntingChance = 0.3f;

    [SerializeField]
    private float updateTime = 3.0f;

    [SerializeField]
    private float sharkDamage = 0.01f;

    private List<GameObject> attackPoints;

    private RaftController raft;

    private GameObject jaws;

    private GameObject nearestPoint;

    void Start()
    {
        startPosition = transform.localPosition;

        startRotation = transform.localRotation;
        
        runAwayPoint = new(0, -5.25f, 0);

        raft = FindObjectOfType<RaftController>();

        jaws = GameObject.Find("Jaws");

        attackPoints = new List<GameObject>();

        StartCoroutine(GenerateChance());
    }

   
    void FixedUpdate()
    {
        var deltaTime = Time.fixedDeltaTime;

        switch (sharkState)
        {
            case SharkState.Starting:
                StartingPosition();
                break;
            case SharkState.Hunting:
                Hunting(deltaTime);
                break;
            case SharkState.Attack:
                Attack(deltaTime);
                break;
            case SharkState.RunAway: 
                RunAway(deltaTime);
                break;
        }
    }

    //Сброс в начальную позицию для охоты
    public void StartingPosition()
    {
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
    }

    #region SharkStates

    //Если акула находится в "StartingPosition", то с некоторой вероятностью начать охоту
    public void Hunting(float deltaTime)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x, -0.85f, transform.localPosition.z), emersionSpeed * deltaTime);
        transform.RotateAround(raft.transform.localPosition, Vector3.up, angularSpeed * deltaTime);

        CheckAttackPoints();
    }


    //Если акула находится в состоянии "Hunting", то с некоторой вероятностью начать атаку
    public void Attack(float deltaTime)
    {
        if (nearestPoint == null)
        {
            CalculateNearestPoint();
        }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, nearestPoint.transform.localPosition, speed * deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(raft.transform.position - transform.position), rotationSpeed * deltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(raft.transform.position.x, raft.transform.position.y + 1f, raft.transform.position.z) - transform.position), rotationSpeed * deltaTime);


        if (transform.localPosition == nearestPoint.transform.localPosition)
        {
            raft.DamageRaft(sharkDamage);
        }


        //refactoring sdelat
        if (raft.RaftHealth < 60 && raft.stage0.activeSelf == true)
        {
            RefreshAttackPoints();
        }
        if (raft.RaftHealth < 30 && raft.stage1.activeSelf == true)
        {
            RefreshAttackPoints();
        }

    }

    //Если игрок нанес акуле урон во время состояния "Attack", то она переходит в состояние "RunAway"
    public void RunAway(float deltaTime)
    {
        if (nearestPoint != null)
        {
            nearestPoint = null;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(runAwayPoint - transform.localPosition), rotationSpeed * deltaTime);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, runAwayPoint, immersionSpeed * deltaTime);

        if (transform.localPosition == runAwayPoint)
        {
            sharkState = SharkState.Starting;
        }
    }
    #endregion


    #region AttackPoints
    public void RefreshAttackPoints()
    {
        sharkState = SharkState.RunAway;
        attackPoints.Clear();
        nearestPoint = null;
    }

    public void CheckAttackPoints()
    {
        if (attackPoints.Count == 0)
        {
            attackPoints = GameObject.FindGameObjectsWithTag("AttackPoint").ToList();
        }
    }

    public void CalculateNearestPoint()
    {
        float minDistance = 100.0f;

        foreach (var attackPoint in attackPoints)
        {
            float distance = Vector3.Distance(jaws.transform.position, attackPoint.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPoint = attackPoint;
            }
        }
    }

    #endregion

    IEnumerator GenerateChance()
    {
        while (true)
        {
            var chance = random.NextDouble();
            //Debug.Log("Chance: " + chance);
            
            if (attackChance > chance && sharkState == SharkState.Hunting)
            {
                //Debug.Log("Shark state Attack by chance " + chance);
                sharkState = SharkState.Attack;
            }

            if (huntingChance > chance && sharkState == SharkState.Starting)
            {
                //Debug.Log("Shark state Hunting by chance " + chance);
                sharkState = SharkState.Hunting;
            }

            yield return new WaitForSeconds(updateTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == RaftCollision.ROCKTAG)
        {
            sharkState = SharkState.RunAway;
        }

        if(other.tag == RaftCollision.INTERACTABLEITEMTAGCOLLIDER)
        {
            MoveItems moveItem = other.gameObject.transform.parent.GetComponent<MoveItems>();
            if (!moveItem.IsMove) 
            {
                sharkState = SharkState.RunAway;
            }
        }
    }
}
