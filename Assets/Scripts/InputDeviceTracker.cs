using UnityEngine.InputSystem;
using UnityEngine;
using System;

public static class InputDeviceTracker
{
    public static bool UsingGamepad { get; private set; }
    public static Action OnSwitchedToGamepad;
    public static Action OnSwitchedToMouse;

    public static void Update()
    {
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame && !UsingGamepad)
        {
            Cursor.visible = false;            
            UsingGamepad = true;
            OnSwitchedToGamepad?.Invoke();
        }        

        if ((Mouse.current != null && Mouse.current.delta.ReadValue().magnitude > 5f) || (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame))
        {
            Cursor.visible = true;  
            UsingGamepad = false;
            OnSwitchedToMouse?.Invoke();
        }            
    }
}