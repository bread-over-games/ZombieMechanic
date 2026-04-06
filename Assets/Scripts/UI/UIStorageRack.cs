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

    private void ShowStorageRackWindow(Bench.BenchType benchType)
    {
        if (benchType != Bench.BenchType.StorageRack)
        {
            return;
        }

        storageRackWindow.SetActive(true);
    }

    private void HideStorageRackWindow(Bench.BenchType benchType)
    {
        if (benchType != Bench.BenchType.StorageRack)
        {
            return;
        }
        storageRackWindow.SetActive(false);
    }
}
