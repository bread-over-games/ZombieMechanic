using UnityEngine;

public class DoorOpenerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource doorAudio;

    [SerializeField] private AudioClip doorOpenClip;
    [SerializeField] private AudioClip doorCloseClip;
    [SerializeField] private AudioClip doorOpenCloseClip;

    private void OnEnable()
    {
        DoorOpener.OnDoorClose += DoorCloseAudio;
        DoorOpener.OnDoorOpen += DoorOpenAudio;
        DoorOpener.OnDoorOpenClose += DoorOpenCloseAudio;
    }

    private void DoorOpenAudio(GameObject obj)
    {
        doorAudio.clip = doorOpenClip;
        doorAudio.Play();
    }

    private void DoorCloseAudio(GameObject obj)
    {
        doorAudio.clip = doorCloseClip;
        doorAudio.Play();
    }

    private void DoorOpenCloseAudio(GameObject obj)
    {
        doorAudio.clip = doorOpenCloseClip;
        doorAudio.Play();
    }
}
