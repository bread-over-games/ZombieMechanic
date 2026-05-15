using UnityEngine;

public class ArmoryAudio : MonoBehaviour
{
    [SerializeField] private Bench armory;
    [SerializeField] private AudioSource armoryAudio;

    [SerializeField] private AudioClip armoryOpenClip;
    [SerializeField] private AudioClip armoryCloseClip;
    private IInteractable interactableArmory;

    private void OnEnable()
    {
        MissionController.OnMissionStarted += CloseArmoryAudio;
        MissionController.OnMissionCompleted += OpenArmoryAudio;
    }

    private void OnDisable()
    {
        MissionController.OnMissionStarted -= CloseArmoryAudio;
        MissionController.OnMissionCompleted -= OpenArmoryAudio;
    }

    private void Awake()
    {
        interactableArmory = armory as IInteractable;
    }

    private void OpenArmoryAudio(Mission mission)
    {
        if (mission.armoryOwner == armory)
        {
            armoryAudio.clip = armoryOpenClip;
            armoryAudio.Play();
        }
    }

    private void CloseArmoryAudio(Mission mission)
    {
        if (mission.armoryOwner == armory)
        {
            armoryAudio.clip = armoryCloseClip;
            armoryAudio.Play();
        }
    }
}
