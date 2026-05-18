using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource objectPlacePickAudio;
    [SerializeField] private AudioSource heartbeatAudio;
    [SerializeField] private AudioClip pickUpAudio;
    [SerializeField] private AudioClip placeAudio;    

    private void OnEnable()
    {
        Bench.OnObjectDeposited += PlayPlaceAudio;
        Bench.OnObjectPicked += PlayPickUpAudio;
        Infection.OnInfectionLevelChange += ChangeHeartbeatVolume;
    }

    private void OnDisable()
    {
        Bench.OnObjectDeposited -= PlayPlaceAudio;
        Bench.OnObjectPicked -= PlayPickUpAudio;
        Infection.OnInfectionLevelChange -= ChangeHeartbeatVolume;
    }

    private void Awake()
    {
        heartbeatAudio.Play();
        heartbeatAudio.volume = 0f;
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

    private void ChangeHeartbeatVolume(float currentInfectionLevel)
    {
        heartbeatAudio.volume = Mathf.InverseLerp(70f, 90f, currentInfectionLevel);
    }
}
