#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class MissingScriptFinder
{
    [MenuItem("Tools/Find Missing Scripts")]
    static void FindMissingScripts()
    {
        foreach (var go in GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            foreach (var c in go.GetComponents<Component>())
            {
                if (c == null)
                    Debug.Log($"Missing script on: {go.name}", go);
            }
        }
    }
}
#endif