using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectorMissionTypes : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public enum MissionTypeSlot 
    { 
            Scavenge,
            Extermination, 
            Antibiotics
    }
    
    [SerializeField] private MissionTypeSlot missionTypeSlot;
    [SerializeField] private UIArmory armoryUI;

    public void OnSelect(BaseEventData eventData)
    {
       //armoryUI.OnButtonSelected(missionTypeSlot);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //armoryUI.OnButtonDeselected(missionTypeSlot);
    }
}