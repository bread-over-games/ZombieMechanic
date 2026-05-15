using UnityEngine;

public class TableAudio : MonoBehaviour
{
    [SerializeField] private Bench table;
    private IInteractable interactableTable;

    [SerializeField] private AudioSource tableAudio;
    [SerializeField] private AudioClip tableOpenClip;
    [SerializeField] private AudioClip tableCloseClip;

    private void OnEnable()
    {
        PlayerInteraction.OnInteractableApproached += TableOpenAudio;
        PlayerInteraction.OnInteractableLeft += TableCloseAudio;
    }

    private void OnDisable()
    {
        PlayerInteraction.OnInteractableApproached -= TableOpenAudio;
        PlayerInteraction.OnInteractableLeft -= TableCloseAudio;
    }

    private void Awake()
    {
        interactableTable = table as IInteractable;
    }

    private void TableOpenAudio(IInteractable approachedBench)
    {
        if (approachedBench != interactableTable) return;
        tableAudio.clip = tableOpenClip;
        tableAudio.Play();
        Debug.Log("Play");
    }

    private void TableCloseAudio(IInteractable leftBench)
    {
        if (leftBench != interactableTable) return;
        tableAudio.clip = tableCloseClip;
        tableAudio.Play();
        Debug.Log("Play");
    }
}
