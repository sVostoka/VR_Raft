using UnityEngine;

public class FoodController : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Food food = other.gameObject.transform.parent.GetComponent<Food>();

        if (other.gameObject.tag == RaftCollision.INTERACTABLEITEMTAGCOLLIDER && food != null)
        {
            player.Eating(food.FoodValue);
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}
