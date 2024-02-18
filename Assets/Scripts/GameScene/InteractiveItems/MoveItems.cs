using UnityEngine;

public class MoveItems : MonoBehaviour
{
    [SerializeField]
    private float _itemSpeed = 3.0f;

    public float ItemSpeed
    {
        get => _itemSpeed;
        set
        {
            _itemSpeed = value;
        }
    }

    [SerializeField]
    private bool _isMove = true;

    public bool IsMove
    {
        get => _isMove;
        set 
        { 
            _isMove = value; 
        }
    }

    [SerializeField]
    private bool _upMove = false;

    public bool UpMove
    {
        get => _upMove;
        set
        {
            _upMove = value;
        }
    }

    [SerializeField]
    private float destroyDistance = 100.0f;

    [SerializeField]
    private float emergeDistance = 3.0f;

    private Water water;

    private Vector3 targetPosition;

    void Start()
    {
        water = FindAnyObjectByType<Water>();

        CalculateTargetPosition();
    }

    void FixedUpdate()
    {
        var deltaTime = Time.fixedDeltaTime;

        MoveItem(deltaTime);
    }

    public void CalculateTargetPosition()
    {
        targetPosition = UpMove ? 
            new(transform.position.x, water.transform.position.y + 0.2f, transform.position.z + emergeDistance) : 
            new(transform.position.x, water.transform.position.y + 0.2f, transform.position.z + destroyDistance);
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }

    private void MoveItem(float deltaTime)
    {
        if(IsMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, ItemSpeed * deltaTime);
        }

        if(transform.position == targetPosition)
        {
            if (UpMove)
            {
                UpMove = false;
                CalculateTargetPosition();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
