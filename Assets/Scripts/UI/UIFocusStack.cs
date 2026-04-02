using System.Collections.Generic;
using UnityEngine;

public static class UIFocusStack
{
    private static readonly Stack<GameObject> _stack = new();

    public static void Push(GameObject ui)
    {
        if (_stack.TryPeek(out var top)) top.SetActive(false); // hide current
        _stack.Push(ui);
        ui.SetActive(true);
    }

    public static void Pop()
    {
        if (_stack.TryPop(out var top)) top.SetActive(false);
        if (_stack.TryPeek(out var next)) next.SetActive(true); // restore previous
    }
}