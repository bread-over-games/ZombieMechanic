using UnityEngine;
using DG.Tweening;

public class DoorOpening : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private Vector3 doorOpenRotation;
    [SerializeField] private Vector3 doorClosedRotation;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float openHoldDuration = 0.1f;

    private void OnEnable()
    {
        MissionController.OnMissionStarted += OpenCloseDoor;
        MissionController.OnMissionCompleted += OpenCloseDoor;
    }

    private void OnDisable()
    {
        MissionController.OnMissionStarted -= OpenCloseDoor;
        MissionController.OnMissionCompleted -= OpenCloseDoor;
    }    

    private void OpenCloseDoor(Mission mission)
    {
        door.DOKill();
        door.DOLocalRotate(doorOpenRotation, duration).OnComplete(() => door.DOLocalRotate(doorClosedRotation, duration).SetEase(Ease.OutBounce).SetDelay(openHoldDuration)
            );
    }
}
