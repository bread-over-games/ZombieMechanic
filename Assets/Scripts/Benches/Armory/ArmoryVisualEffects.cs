using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ArmoryVisualEffects : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Bench armory;
    private IInteractable interactableArmory;

    private void OnEnable()
    {
        PlayerInteraction.OnInteractableApproached += OpenPlayerArmory;
        PlayerInteraction.OnInteractableLeft += ClosePlayerArmory;
        MissionController.OnMissionStarted += CloseArmoryMission;
        MissionController.OnMissionCompleted += OpenArmoryMission;
    }

    private void OnDisable()
    {
        PlayerInteraction.OnInteractableApproached -= OpenPlayerArmory;
        PlayerInteraction.OnInteractableLeft -= ClosePlayerArmory;
        MissionController.OnMissionStarted -= CloseArmoryMission;
        MissionController.OnMissionCompleted -= OpenArmoryMission;
    }

    private void Awake()
    {
        interactableArmory = armory as IInteractable;
    }

    private void OpenArmoryMission(Mission mission)
    {
        if (mission.armoryOwner == armory)
        {
            animator.SetTrigger("OpenMission");
        }
    }

    private void CloseArmoryMission(Mission mission)
    {
        if (mission.armoryOwner == armory)
        {
            animator.SetTrigger("CloseMission");
        }
    }

    private void OpenPlayerArmory(IInteractable approachedArmory)
    {
        if (approachedArmory == interactableArmory)
        {
            animator.SetTrigger("OpenPlayer");
        }
    }

    private void ClosePlayerArmory(IInteractable leftArmory)
    {
        if (leftArmory == interactableArmory)
        {
            animator.SetTrigger("ClosePlayer");
        }
    }
}
