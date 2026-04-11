using System.Linq;
using UnityEngine;

public class UIFocusStackDebugger : MonoBehaviour
{
    private void OnGUI()
    {
        var stack = UIFocusStack.GetStack().ToArray();

        GUILayout.BeginArea(new Rect(10, 10, 200, 400));
        GUILayout.Label("=== UIFocusStack ===");

        if (stack.Length == 0)
        {
            GUILayout.Label("(empty)");
        }
        else
        {
            for (int i = 0; i < stack.Length; i++)
            {
                string label = i == 0
                    ? $"► {stack[i].name}"   // top of stack
                    : $"  {stack[i].name}";
                GUILayout.Label(label);
            }
        }

        GUILayout.EndArea();
    }
}