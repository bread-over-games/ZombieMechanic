using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private UnityEvent onSelected;
    [SerializeField] private UnityEvent onDeselected;

    private Vector2 lastMousePos;

    void Update()
    {
        if (!InputDeviceTracker.UsingGamepad)
        {
            Vector2 mousePos = Input.mousePosition;
            if (mousePos != lastMousePos)
            {
                EventSystem.current.SetSelectedGameObject(null);
                lastMousePos = mousePos;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!InputDeviceTracker.UsingGamepad)
            onSelected?.Invoke();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData is AxisEventData)
            onSelected?.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        onDeselected?.Invoke();
    }    
}