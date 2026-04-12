using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public enum ArmorySlot 
    { 
            Weapon,
            Armor, 
            Backpack
    }
    
    [SerializeField] private ArmorySlot armorySlot;
    [SerializeField] private UIGearOverview gearOverviewUI;

    public void OnSelect(BaseEventData eventData)
    {
        gearOverviewUI.OnButtonSelected(armorySlot);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        gearOverviewUI.OnButtonDeselected(armorySlot);
    }
}