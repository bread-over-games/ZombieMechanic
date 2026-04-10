using UnityEngine;

public class UIStorageRack : MonoBehaviour
{
    [SerializeField] private GameObject storageRackWindow;

    private void OnEnable()
    {
        PlayerInteraction.OnInteractableApproached += ShowStorageRackWindow;
        PlayerInteraction.OnInteractableLeft += HideStorageRackWindow;
    }

    private void ShowStorageRackWindow(IInteractable interactableType)
    {
        if (interactableType is IBench bench)
        {
            if (bench.GetBenchType() != Bench.BenchType.StorageRack)
            {
                return;
            }
            UIFocusStack.Push(storageRackWindow);
        }
    }

    private void HideStorageRackWindow(IInteractable interactableType)
    {
        if (interactableType is IBench bench)
        {
            if (bench.GetBenchType() != Bench.BenchType.StorageRack)
            {
                return;
            }
            UIFocusStack.Pop();
        }
    }
}
