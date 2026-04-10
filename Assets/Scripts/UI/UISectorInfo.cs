using UnityEngine;
using System;
using TMPro;

public class UISectorInfo : MonoBehaviour
{
    [SerializeField] private GameObject sectorInfoWindow;
    [SerializeField] private GameObject atbDepletedWindow;
    [SerializeField] private GameObject atbLowWindow;
    [SerializeField] private TMP_Text[] zombiesLeft;

    private void OnEnable()
    {
        SectorController.OnAntibioticsDepleted += ShowAtbDepletedWindow;
        SectorController.OnAntibioticsRunningLow += ShowAtbLowWindow;
        PlayerInteraction.OnMessageConfirmed += HideWindows;
    }

    private void OnDisable()
    {
        SectorController.OnAntibioticsDepleted -= ShowAtbDepletedWindow;
        SectorController.OnAntibioticsRunningLow -= ShowAtbLowWindow;
        PlayerInteraction.OnMessageConfirmed -= HideWindows;
    }

    private void ShowAtbDepletedWindow()
    {
        UIFocusStack.Push(sectorInfoWindow);
        UpdateZombiesLeft();
        atbLowWindow.SetActive(false);
        atbDepletedWindow.SetActive(true);        
    }

    private void ShowAtbLowWindow()
    {
        UIFocusStack.Push(sectorInfoWindow);
        UpdateZombiesLeft();
        atbLowWindow.SetActive(true);
        atbDepletedWindow.SetActive(false);        
    }

    private void HideWindows()
    {
        atbLowWindow.SetActive(false);
        atbDepletedWindow.SetActive(false);
        UIFocusStack.Pop();
    }

    private void UpdateZombiesLeft()
    {
        foreach (TMP_Text zombLeft in zombiesLeft)
        {
            zombLeft.text = (ZombiesController.Instance.zombiesKillVictoryGoal - ZombiesController.Instance.zombiesKilledTotal).ToString();
        }
    }
}
