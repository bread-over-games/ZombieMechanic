using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetMoving(bool moving)
    {
        animator.SetBool("isMoving", moving);
    }

    public void SetCarrying(bool carrying)
    {
        animator.SetBool("isCarrying", carrying);
    }
}
