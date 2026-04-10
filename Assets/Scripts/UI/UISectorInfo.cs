using UnityEngine;

public class UISectorInfo : MonoBehaviour
{
    [SerializeField] private GameObject sectorInfoWindow;
    [SerializeField] private GameObject atbDepletedWindow;
    [SerializeField] private GameObject atbLowWindow;

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
        atbLowWindow.SetActive(false);
        atbDepletedWindow.SetActive(true);        
    }

    private void ShowAtbLowWindow()
    {
        UIFocusStack.Push(sectorInfoWindow);
        atbLowWindow.SetActive(true);
        atbDepletedWindow.SetActive(false);        
    }

    private void HideWindows()
    {
        atbLowWindow.SetActive(false);
        atbDepletedWindow.SetActive(false);
        UIFocusStack.Pop();
    }
}
