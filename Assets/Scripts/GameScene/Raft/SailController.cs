using UnityEngine;

public class SailController : MonoBehaviour
{
    public Animator animator;

    public bool isFurled = false;
    public bool isInflated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand")
        {
            if (animator.GetBool("IsFurled") == false)
            {
                animator.SetBool("IsWindy", false);
                animator.SetBool("IsFurled", true);
                isFurled = true;
            }
            else
            {
                animator.SetBool("IsFurled", false);
                isFurled = false;
            }
        }
    }

    public void CheckWeather(bool isWindy)
    {
        if (isWindy == true)
        {
            animator.SetBool("IsWindy", true);
            isInflated = true;
        }
        else
        {
            animator.SetBool("IsWindy", false);
            isInflated = false;
        }
    }
}
