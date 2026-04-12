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
    [SerializeField] private UIMissionSelect missionSelectUI;

    public void OnSelect(BaseEventData eventData)
    {
        missionSelectUI.OnButtonSelected(missionTypeSlot);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        missionSelectUI.OnButtonDeselected(missionTypeSlot);
    }
}