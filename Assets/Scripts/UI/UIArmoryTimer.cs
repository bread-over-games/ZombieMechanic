using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Numerics;

public class UIArmoryTimer : MonoBehaviour
{
    [SerializeField] private Armory armory;
    [SerializeField] private GameObject timerBackgroundImage;
    [SerializeField] private Image timerImage;

    private Coroutine currentTimerCoroutine;
    private Mission currentMission;

    private void OnEnable()
    {
        MissionController.OnMissionStarted += StartTimer;
        MissionController.OnMissionCompleted += StopTimer;
    }

    private void OnDisable()
    {
        MissionController.OnMissionStarted -= StartTimer;
        MissionController.OnMissionCompleted -= StopTimer;
    }

    private void Awake()
    {
        timerBackgroundImage.SetActive(false);
    }

    private void StartTimer(Mission startedMission)
    {
        if (startedMission.armoryOwner == armory)
        {
            timerBackgroundImage.SetActive(true);
            currentMission = startedMission;
            currentTimerCoroutine = StartCoroutine(ChangeTimer());
        }        
    }

    private void StopTimer(Mission completedMission)
    {
        if (armory == completedMission.armoryOwner)
        {
            StopCoroutine(currentTimerCoroutine);
            timerBackgroundImage.SetActive(false);
            currentMission = null;
            currentTimerCoroutine = null;
        }
    }

    private IEnumerator ChangeTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            timerImage.fillAmount = currentMission.elapsedTime / currentMission.missionDuration;
        }        
    }
}
