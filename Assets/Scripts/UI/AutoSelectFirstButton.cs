using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoSelectFirstButton : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(SelectNextFrame());
    }

    private IEnumerator SelectNextFrame()
    {
        yield return new WaitForSecondsRealtime(0);
        var first = GetComponentInChildren<Selectable>();
        if (first != null)
            EventSystem.current?.SetSelectedGameObject(first.gameObject);
    }
}