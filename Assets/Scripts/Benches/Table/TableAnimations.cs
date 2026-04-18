using UnityEngine;

public class TableAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Bench table;
    private IInteractable interactableTable;

    private void OnEnable()
    {
        PlayerInteraction.OnInteractableApproached += OpenTable;
        PlayerInteraction.OnInteractableLeft += CloseTable;
    }

    private void OnDisable()
    {
        PlayerInteraction.OnInteractableApproached -= OpenTable;
        PlayerInteraction.OnInteractableLeft -= CloseTable;
    }

    private void Awake()
    {
        interactableTable = table as IInteractable;
    }

    private void OpenTable(IInteractable approachedTable)
    {
        if (approachedTable == interactableTable)
        {
            animator.SetTrigger("Open");
        }
    }

    private void CloseTable(IInteractable leftTable)
    {
        if (leftTable == interactableTable)
        {
            animator.SetTrigger("Close");
        }
    }
}
