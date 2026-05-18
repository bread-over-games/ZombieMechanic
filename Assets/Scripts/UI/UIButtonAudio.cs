using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIAudioController.Instance.PlayHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIAudioController.Instance.PlayClick();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData is PointerEventData) return;
        UIAudioController.Instance.PlayHover();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        UIAudioController.Instance.PlayClick();
    }
}
