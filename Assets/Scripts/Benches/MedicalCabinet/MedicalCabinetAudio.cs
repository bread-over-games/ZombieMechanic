using System;
using UnityEngine;

public class MedicalCabinetAudio : MonoBehaviour
{
    [SerializeField] private Bench medicalCab;
    private IInteractable interactableMedicalCab;

    [SerializeField] private AudioSource medicalCabinetAudio;
    [SerializeField] private AudioClip atbUseClip;
    [SerializeField] private AudioClip medicalCabinetOpenClip;
    [SerializeField] private AudioClip medicalCabinetCloseClip;

    private void OnEnable()
    {
        MedicalCabinet.OnAntibioticsUsed += AtbUsedAudio;
        PlayerInteraction.OnInteractableApproached += MedicalCabinetOpenAudio;
        PlayerInteraction.OnInteractableLeft += MedicalCabinetCloseAudio;
    }

    private void OnDisable()
    {
        MedicalCabinet.OnAntibioticsUsed -= AtbUsedAudio;
        PlayerInteraction.OnInteractableApproached -= MedicalCabinetOpenAudio;
        PlayerInteraction.OnInteractableLeft -=  MedicalCabinetCloseAudio;
    }

    private void Awake()
    {
        interactableMedicalCab = medicalCab as IInteractable;
    }

    private void AtbUsedAudio()
    {
        medicalCabinetAudio.clip = atbUseClip;
        medicalCabinetAudio.Play();
    }

    private void MedicalCabinetOpenAudio(IInteractable approachedBench)
    {
        if (approachedBench != interactableMedicalCab) return;
        medicalCabinetAudio.clip = medicalCabinetOpenClip;
        medicalCabinetAudio.Play();
    }

    private void MedicalCabinetCloseAudio(IInteractable leftBench)
    {
        if (leftBench != interactableMedicalCab) return;
        medicalCabinetAudio.clip = medicalCabinetCloseClip;
        medicalCabinetAudio.Play();
    }
}
