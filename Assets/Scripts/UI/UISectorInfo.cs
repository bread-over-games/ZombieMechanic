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

    private void Start()
    {
        UIFocusStack.Push(sectorInfoWindow);
    }

    private void ShowAtbDepletedWindow()
    {
        atbLowWindow.SetActive(false);
        atbDepletedWindow.SetActive(true);
        UIFocusStack.Push(sectorInfoWindow);
    }

    private void ShowAtbLowWindow()
    {
        atbLowWindow.SetActive(true);
        atbDepletedWindow.SetActive(false);
        UIFocusStack.Push(sectorInfoWindow);
    }

    private void HideWindows()
    {
        atbLowWindow.SetActive(false);
        atbDepletedWindow.SetActive(false);
        UIFocusStack.Pop();
    }
}
