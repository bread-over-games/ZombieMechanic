using UnityEngine;

public class MedicalCabinetAudio : MonoBehaviour
{
    [SerializeField] private AudioSource atbUseAudio;

    private void OnEnable()
    {
        MedicalCabinet.OnAntibioticsUsed += AtbUsedAudio;
    }

    private void OnDisable()
    {
        MedicalCabinet.OnAntibioticsUsed -= AtbUsedAudio;
    }

    private void AtbUsedAudio()
    {
        atbUseAudio.Play();
    }
}
