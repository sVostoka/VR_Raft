using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField]
    private float damage = 0.1f;

    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == player.name)
        {
            player.Damage = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.name == player.name)
        {
            player.DamagePlayer(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == player.name)
        {
            player.Damage = false;
        }
    }
}
