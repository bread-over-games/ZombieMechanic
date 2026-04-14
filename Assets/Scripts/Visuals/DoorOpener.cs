using UnityEngine;
using DG.Tweening;

public class DoorOpening : MonoBehaviour
{
    public enum DoorTrigger
    {
        MissionStarted,
        TutorialEnd
    }

    [SerializeField] private DoorTrigger trigger;
    [SerializeField] private Transform door;    
    [SerializeField] private Vector3 doorOpenRotation;
    [SerializeField] private Vector3 doorClosedRotation;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float openHoldDuration = 0.1f;

    private void OnEnable()
    {
        switch (trigger)
        {
            case DoorTrigger.MissionStarted:
                MissionController.OnMissionStarted += MissionTrigger;
                break;
            case DoorTrigger.TutorialEnd:
                TutorialController.OnTutorialEnd += OpenDoor;
                break;
        }
    }

    private void OnDisable()
    {
        switch (trigger)
        {
            case DoorTrigger.MissionStarted:
                MissionController.OnMissionStarted -= MissionTrigger;
                break;
            case DoorTrigger.TutorialEnd:
                TutorialController.OnTutorialEnd -= OpenDoor;
                break;
        }
    }

    private void MissionTrigger(Mission mission)
    {
        OpenCloseDoor();
    }

    public void OpenCloseDoor()
    {
        door.DOKill();
        door.DOLocalRotate(doorOpenRotation, duration).OnComplete(() => door.DOLocalRotate(doorClosedRotation, duration).SetEase(Ease.OutBounce).SetDelay(openHoldDuration));
    }

    public void OpenDoor()
    {
        door.DOKill();
        door.DOLocalRotate(doorOpenRotation, duration).SetEase(Ease.OutBounce);        
    }

    public void CloseDoor()
    {
        door.DOKill();
        door.DOLocalRotate(doorClosedRotation, duration).SetEase(Ease.OutBounce);
    }
}
