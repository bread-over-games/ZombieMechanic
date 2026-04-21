using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WorkbenchVisualEffects : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Bench workbench;
    private IInteractable interactableTable;

    private void OnEnable()
    {
        PlayerInteraction.OnInteractableApproached += OpenWorkbench;
        PlayerInteraction.OnInteractableLeft += CloseWorkbench;
        Workbench.OnRepairStart += StartWorking;
        Workbench.OnRepairStop += StopWorking;
    }

    private void OnDisable()
    {
        PlayerInteraction.OnInteractableApproached -= OpenWorkbench;
        PlayerInteraction.OnInteractableLeft -= CloseWorkbench;
    }

    private void Awake()
    {
        interactableTable = workbench as IInteractable;
    }

    private void OpenWorkbench(IInteractable approachedWorkbench)
    {
        if (approachedWorkbench == interactableTable)
        {
            animator.SetTrigger("Open");
        }
    }

    private void CloseWorkbench(IInteractable leftWorkbench)
    {
        if (leftWorkbench == interactableTable)
        {
            animator.SetTrigger("Close");
        }
    }

    private void StartWorking()
    {
        animator.SetBool("isWorking", true);
    }

    private void StopWorking()
    {
        animator.SetBool("isWorking", false);
    }
}
