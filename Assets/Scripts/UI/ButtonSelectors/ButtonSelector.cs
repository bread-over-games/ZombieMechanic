using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    [SerializeField] private UnityEvent onSelected;
    [SerializeField] private UnityEvent onDeselected;

    public void OnSelect(BaseEventData eventData)
    {
        onSelected?.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        onDeselected?.Invoke();
    }
}