using UnityEngine;
using System;
using TMPro;

public class UISectorInfo : MonoBehaviour
{
    [SerializeField] private GameObject sectorInfoWindow;
    [SerializeField] private GameObject atbDepletedWindow;
    [SerializeField] private GameObject atbLowWindow;
    [SerializeField] private TMP_Text[] zombiesLeft;

    public static Action OnMessageConfirmed;

    private void OnEnable()
    {
        SectorController.OnAntibioticsDepleted += ShowAtbDepletedWindow;
        SectorController.OnAntibioticsRunningLow += ShowAtbLowWindow;
    }

    private void OnDisable()
    {
        SectorController.OnAntibioticsDepleted -= ShowAtbDepletedWindow;
        SectorController.OnAntibioticsRunningLow -= ShowAtbLowWindow;
    }

    private void ShowAtbDepletedWindow()
    {
        PlayerInteraction.OnSecondaryInteractionInterceptor = HideWindows;
        UIFocusStack.Push(sectorInfoWindow);
        UpdateZombiesLeft();
        atbLowWindow.SetActive(false);
        atbDepletedWindow.SetActive(true);        
    }

    private void ShowAtbLowWindow()
    {
        PlayerInteraction.OnSecondaryInteractionInterceptor = HideWindows;
        UIFocusStack.Push(sectorInfoWindow);
        UpdateZombiesLeft();
        atbLowWindow.SetActive(true);
        atbDepletedWindow.SetActive(false);        
    }

    private void HideWindows()
    {
        OnMessageConfirmed?.Invoke();
        atbLowWindow.SetActive(false);
        atbDepletedWindow.SetActive(false);
        UIFocusStack.Pop();
        PlayerInteraction.OnSecondaryInteractionInterceptor = null;        
    }

    private void UpdateZombiesLeft()
    {
        foreach (TMP_Text zombLeft in zombiesLeft)
        {
            zombLeft.text = (ZombiesController.Instance.zombiesKillVictoryGoal - ZombiesController.Instance.zombiesKilledTotal).ToString();
        }
    }
}
