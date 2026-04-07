using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        Bench.OnObjectPicked += ObjectPicked;
        Bench.OnObjectDeposited += ObjectPlaced;
    }

    private void OnDisable()
    {
        Bench.OnObjectPicked -= ObjectPicked;
        Bench.OnObjectDeposited -= ObjectPlaced;
    }

    private void ObjectPicked()
    {
        SetCarrying(true);
    }

    private void ObjectPlaced()
    {
        SetCarrying(false);
    }

    public void SetMoving(bool moving)
    {
        animator.SetBool("isMoving", moving);
    }

    public void SetCarrying(bool carrying)
    {
        animator.SetBool("isCarrying", carrying);
    }
}
