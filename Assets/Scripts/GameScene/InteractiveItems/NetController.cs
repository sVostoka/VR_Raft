using UnityEngine;

public class NetController : MonoBehaviour
{
    public GameObject centerNet;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == RaftCollision.INTERACTABLEITEMTAGCOLLIDER)
        {
            ItemController itemController = other.gameObject.transform.parent.GetComponent<ItemController>();

            if (!itemController.interactable.isSelected)
            {
                other.gameObject.transform.parent.SetParent(gameObject.transform);

                itemController.InNat = true;

                other.gameObject.transform.position = centerNet.transform.position;
            }
        }
    }
}
