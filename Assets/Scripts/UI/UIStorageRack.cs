using UnityEngine;

public class UIStorageRack : MonoBehaviour
{
    [SerializeField] private GameObject storageRackWindow;

    private void OnEnable()
    {
        PlayerInteraction.OnInteractableApproached += ShowStorageRackWindow;
        PlayerInteraction.OnInteractableLeft += HideStorageRackWindow;
    }

    private void Start()
    {
        UIFocusStack.Push(storageRackWindow);
    }

    private void ShowStorageRackWindow(IInteractable interactableType)
    {
        if (interactableType is IBench bench)
        {
            if (bench.GetBenchType() != Bench.BenchType.StorageRack)
            {
                return;
            }
            storageRackWindow.SetActive(true);
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
            storageRackWindow.SetActive(false);
        }
    }
}
