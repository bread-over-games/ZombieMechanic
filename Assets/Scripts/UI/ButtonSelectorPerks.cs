using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectorPerks : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public enum PerkSlot
    {
        FirstPerkSlot,
        SecondPerkSlot
    }

    [SerializeField] private PerkSlot perkSlot;
    [SerializeField] private UIPerkSystem perkSysUI;

    public void OnSelect(BaseEventData eventData)
    {
        perkSysUI.OnButtonSelected(perkSlot);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        perkSysUI.OnButtonDeselected(perkSlot);
    }
}
