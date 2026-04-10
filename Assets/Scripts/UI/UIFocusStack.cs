using System.Collections.Generic;
using UnityEngine;
using System;

public static class UIFocusStack
{
    private static readonly Stack<GameObject> _stack = new();
    public static event Action OnFocusReturned;

    public static void Push(GameObject ui)
    {
        if (_stack.TryPeek(out var top)) top.SetActive(false); // hide current
        _stack.Push(ui);
        ui.SetActive(true);
    }

    public static void Pop()
    {
        if (_stack.TryPop(out var top)) top.SetActive(false);
        foreach (var window in _stack)
            window.SetActive(true); // restore all remaining windows
    }

    public static IEnumerable<GameObject> GetStack() => _stack;
}