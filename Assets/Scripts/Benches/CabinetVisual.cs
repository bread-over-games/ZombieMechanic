using DG.Tweening;
using UnityEngine;

public class CabinetVisual : MonoBehaviour
{
    [SerializeField] private Bench parent;

    [SerializeField] private Transform doorLeft;
    [SerializeField] private Transform doorRight;
    [SerializeField] private Vector3 openRotationDoorLeft = new Vector3(0, 110f, 0);
    [SerializeField] private Vector3 openRotationDoorRight = new Vector3(0, -110f, 0);
    [SerializeField] private float duration = 0.4f;

    private Vector3 closedRotation = new Vector3(0, 0, 0);
    private bool isOpen = true;

    private void OnEnable()
    {
        MissionController.OnMissionStarted += ToggleDoor;
        MissionController.OnMissionCompleted += ToggleDoor;
    }

    private void OnDisable()
    {
        MissionController.OnMissionStarted -= ToggleDoor;
        MissionController.OnMissionCompleted -= ToggleDoor;
    }

    public void ToggleDoor(Mission mission)
    {
        if (mission.armoryOwner == parent)
        {
            Vector3 targetLeft = isOpen ? closedRotation : openRotationDoorLeft;
            Vector3 targetRight = isOpen ? closedRotation : openRotationDoorRight;
            doorLeft.DOLocalRotate(targetLeft, duration).SetEase(Ease.OutBack);
            doorRight.DOLocalRotate(targetRight, duration).SetEase(Ease.OutBack);
            isOpen = !isOpen;
        }
    }
}
