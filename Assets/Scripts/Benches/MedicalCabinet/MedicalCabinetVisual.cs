using UnityEngine;

public class MedicalCabinetVisual : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private Bench medicalCabinet;
    private IInteractable interactableTable;

    private void OnEnable()
    {
        PlayerInteraction.OnInteractableApproached += OpenMedicalCabinet;
        PlayerInteraction.OnInteractableLeft += CloseMedicalCabinet;
    }

    private void OnDisable()
    {

        PlayerInteraction.OnInteractableApproached -= OpenMedicalCabinet;
        PlayerInteraction.OnInteractableLeft -= CloseMedicalCabinet;
    }

    private void Awake()
    {
        interactableTable = medicalCabinet as IInteractable;
    }

    private void OpenMedicalCabinet(IInteractable approachedMedCabinet)
    {
        if (approachedMedCabinet == interactableTable)
        {
            animator.SetTrigger("Open");
        }
    }

    private void CloseMedicalCabinet(IInteractable leftMedCabinet)
    {
        if (leftMedCabinet == interactableTable)
        {
            animator.SetTrigger("Close");
        }
    }
}
