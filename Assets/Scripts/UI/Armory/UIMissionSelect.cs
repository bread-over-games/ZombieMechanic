using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;
using static Mission;

public class UIMissionSelect : MonoBehaviour
{
    [SerializeField] private UIArmory uiArmory;
    [SerializeField] private GameObject missionSelectWindow;
    [SerializeField] private GameObject firstSelected;

    public ButtonSelectorMissionTypes.MissionTypeSlot currentSlotSelected;
    private MissionType currentMissionType;

    public static Action<MissionType> OnCurrentMissionTypeSlotSelected;

    [Header("Mission Estimates")]
    [SerializeField] private GameObject missionEstimates;
    [SerializeField] private TMP_Text durationText;
    [SerializeField] private TMP_Text lootQualityText;
    [SerializeField] private TMP_Text lootAmountText;
    [SerializeField] private TMP_Text gearWearText;
    [SerializeField] private TMP_Text zombieKillsText;

    private void OnEnable()
    {
        PlayerInteraction.OnMisisonTypeSelected += SelectCurrentMissionType;
    }

    private void OnDisable()
    {
        PlayerInteraction.OnMisisonTypeSelected -= SelectCurrentMissionType;
    }

    public void OpenWindow()
    {
        missionSelectWindow.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelected);
        RefreshEstimatesUI();
    }

    public void CloseWindow()
    {
        EventSystem.current.SetSelectedGameObject(null);
        missionSelectWindow.SetActive(false);        
    }

    private void SelectCurrentMissionType()
    {
        OnCurrentMissionTypeSlotSelected?.Invoke(currentMissionType);
        CloseWindow();
    }

    public void OnButtonSelected(ButtonSelectorMissionTypes.MissionTypeSlot missionTypeSlot)
    {
        switch (missionTypeSlot)
        {
            case ButtonSelectorMissionTypes.MissionTypeSlot.Scavenge:
                currentMissionType = Mission.MissionType.Scavenge;
                break;
            case ButtonSelectorMissionTypes.MissionTypeSlot.Extermination:
                currentMissionType = Mission.MissionType.Extermination;
                break;
            case ButtonSelectorMissionTypes.MissionTypeSlot.Antibiotics:
                currentMissionType = Mission.MissionType.Antibiotics;
                break;
        }

        currentSlotSelected = missionTypeSlot;
        RefreshEstimatesUI();
    }

    public void OnButtonDeselected(ButtonSelectorMissionTypes.MissionTypeSlot missionTypeSlot)
    {

    }

    private void RefreshEstimatesUI()
    {        
        if (uiArmory.inventory == null)
        {
            return;
        }

        MissionEstimate missionEstimates = MissionCalculator.EstimateMission(uiArmory.armory.storedWeapon, uiArmory.armory.storedBackpack, uiArmory.armory.storedArmor, currentMissionType);

        durationText.text = missionEstimates.estimatedDuration.ToString() + "s +-";
        lootQualityText.text = missionEstimates.estimatedLootQualityMinimal.ToString("F0") + "% - " + missionEstimates.estimatedLootQualityMaximal.ToString("F0") + "%";
        lootAmountText.text = missionEstimates.estimatedLootAmount.ToString() + "-" + (missionEstimates.estimatedLootAmount + 1).ToString();
        zombieKillsText.text = missionEstimates.estimatedZombiesKills.ToString() + "+-";
        gearWearText.text = missionEstimates.estimatedGearWear.ToString() + "+- per item";
    }
}
