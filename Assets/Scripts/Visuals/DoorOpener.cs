using UnityEngine;
using DG.Tweening;
using System;

public class DoorOpener : MonoBehaviour
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

    public static Action<GameObject> OnDoorOpen;
    public static Action<GameObject> OnDoorClose;
    public static Action<GameObject> OnDoorOpenClose;

    private void OnEnable()
    {
        switch (trigger)
        {
            case DoorTrigger.MissionStarted:
                MissionController.OnMissionStarted += _ => OpenCloseDoor();
                MissionController.OnMissionCompleted += _ => OpenCloseDoor();
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
                MissionController.OnMissionStarted -= _ => OpenCloseDoor();
                MissionController.OnMissionCompleted -= _ => OpenCloseDoor();
                break;
            case DoorTrigger.TutorialEnd:
                TutorialController.OnTutorialEnd -= OpenDoor;
                break;
        }
    }

    public void OpenCloseDoor()
    {
        door.DOKill();
        door.DOLocalRotate(doorOpenRotation, duration).OnComplete(() => door.DOLocalRotate(doorClosedRotation, duration).SetEase(Ease.OutBounce).SetDelay(openHoldDuration));
        OnDoorOpenClose?.Invoke(gameObject);
    }

    public void OpenDoor()
    {
        door.DOKill();
        door.DOLocalRotate(doorOpenRotation, duration).SetEase(Ease.OutBounce);      
        OnDoorOpen?.Invoke(gameObject);
    }

    public void CloseDoor()
    {
        door.DOKill();
        door.DOLocalRotate(doorClosedRotation, duration).SetEase(Ease.OutBounce);
        OnDoorClose?.Invoke(gameObject);
    }
}
