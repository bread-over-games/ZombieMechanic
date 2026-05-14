using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource objectPlacePickAudio;
    [SerializeField] private AudioClip pickUpAudio;
    [SerializeField] private AudioClip placeAudio;

    private void OnEnable()
    {
        Bench.OnObjectDeposited += PlayPlaceAudio;
        Bench.OnObjectPicked += PlayPickUpAudio;
    }

    private void PlayPlaceAudio()
    {
        objectPlacePickAudio.clip = placeAudio;
        objectPlacePickAudio.Play();
    }

    private void PlayPickUpAudio()
    {
        objectPlacePickAudio.clip = pickUpAudio;
        objectPlacePickAudio.Play();
    }
}
