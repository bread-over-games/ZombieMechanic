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
    [SerializeField] private UIArmory armoryUI;

    public void OnSelect(BaseEventData eventData)
    {
       armoryUI.OnButtonSelected(armorySlot);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        armoryUI.OnButtonDeselected(armorySlot);
    }
}