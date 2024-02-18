using UnityEngine;

public class TurnController : MonoBehaviour
{
    [SerializeField]
    public RaftController.TurnSide side;

    private RaftController raftController;

    void Start()
    {
        raftController = FindAnyObjectByType<RaftController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PaddleController paddle = other.transform.GetComponent<PaddleController>();
        ItemController itemController = other.transform.GetComponent<ItemController>();

        if (paddle != null && itemController.interactable.isSelected)
        {
            raftController.Turn(side, 1 * paddle.coeffPaddle);
        }
    }
}
