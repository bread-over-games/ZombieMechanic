using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIMissionInfoPanel : MonoBehaviour
{
    public TMP_Text missionDuration;
    public TMP_Text missionTypeName;
    public Image timerImage;
    public ScalePulse scalePulse;

    public GameObject currentMissionWindow;
    public GameObject missionCompleteWindow;
    
    [HideInInspector] public Mission currentMission;
    private Coroutine currentCoroutine;

    public UIFlyoutVisual flyoutVisualPrefab;
    public RectTransform xpFlyoutPosition;
    public RectTransform zombieFlyoutPosition;

    private void OnEnable()
    {
        MissionController.OnMissionCompleted += ShowMissionComplete;
    }

    private void OnDisable()
    {
        MissionController.OnMissionCompleted -= ShowMissionComplete;
    }

    public void Initialize(Mission mission)
    {
        currentMissionWindow.SetActive(true);  
        currentMission = mission;
        currentCoroutine = StartCoroutine(ChangeTimer());
        missionTypeName.text = mission.missionType.ToString() + " mission";
        scalePulse.Pulse();
    }

    private void ShowMissionComplete(Mission mission)
    {
        if (currentMission == mission)
        {
            StopCoroutine(currentCoroutine);
            currentMissionWindow.SetActive(false);
            missionCompleteWindow.SetActive(true);  
            scalePulse.Pulse();
            Invoke("Delete", 2.5f);
            SpawnXPFlyout();
            SpawnZombiesFlyout();
        }        
    }

    private void SpawnXPFlyout()
    {
        UIFlyoutVisual xpFlyout = Instantiate(flyoutVisualPrefab, xpFlyoutPosition.position, xpFlyoutPosition.rotation, xpFlyoutPosition);
        xpFlyout.Initialize(UIFlyoutVisual.FlyoutTypes.XP, currentMission.zombiesKilled * XPCounter.Instance.zombieKillXP, 2f, 1f, GameObject.Find("CurrentLevel").GetComponent<RectTransform>().position);
    }

    private void SpawnZombiesFlyout()
    {
        UIFlyoutVisual zombiesFlyout = Instantiate(flyoutVisualPrefab, zombieFlyoutPosition.position, zombieFlyoutPosition.rotation, zombieFlyoutPosition);
        zombiesFlyout.Initialize(UIFlyoutVisual.FlyoutTypes.Zombies, currentMission.zombiesKilled, 2f, 1f, GameObject.Find("ZombiesKilledIcon").GetComponent<RectTransform>().position);
    }

    private IEnumerator ChangeTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            timerImage.fillAmount = currentMission.elapsedTime / currentMission.missionDuration;
            missionDuration.text = Mathf.RoundToInt(currentMission.missionDuration - currentMission.elapsedTime).ToString() + "s";
        }
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}
