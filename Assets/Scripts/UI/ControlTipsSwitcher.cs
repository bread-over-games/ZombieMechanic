using UnityEngine;

public class ControlTipsSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject gamepadControlTips;
    [SerializeField] private GameObject mouseKeyboardControlTips;

    private void OnEnable()
    {
        InputDeviceTracker.OnSwitchedToGamepad += ShowGamepadControls;
        InputDeviceTracker.OnSwitchedToMouse += ShowMouseKeyboardControls;

        if (InputDeviceTracker.UsingGamepad)
        {
            ShowGamepadControls();
        } else
        {
            ShowMouseKeyboardControls();    
        }
    }

    private void OnDisable()
    {
        InputDeviceTracker.OnSwitchedToGamepad -= ShowGamepadControls;
        InputDeviceTracker.OnSwitchedToMouse -= ShowMouseKeyboardControls;
    }

    private void ShowGamepadControls()
    {
        if (gamepadControlTips) gamepadControlTips?.SetActive(true);
        if (mouseKeyboardControlTips) mouseKeyboardControlTips?.SetActive(false);
    }

    private void ShowMouseKeyboardControls()
    {
        if (gamepadControlTips) gamepadControlTips?.SetActive(false);
        if (mouseKeyboardControlTips) mouseKeyboardControlTips?.SetActive(true);
    }
}
