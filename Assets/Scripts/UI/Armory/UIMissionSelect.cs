using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class UIMissionSelect : MonoBehaviour
{
    [SerializeField] private UIArmory uiArmory;
    [SerializeField] private GameObject missionSelectWindow;
    [SerializeField] private GameObject firstSelected;

    public ButtonSelectorMissionTypes.MissionTypeSlot currentSlotSelected;

    public static Action<ButtonSelectorMissionTypes.MissionTypeSlot> OnCurrentMissionTypeSlotSelected;

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
        OnCurrentMissionTypeSlotSelected?.Invoke(currentSlotSelected);
        CloseWindow();
    }

    public void OnButtonSelected(ButtonSelectorMissionTypes.MissionTypeSlot missionTypeSlot)
    {
        currentSlotSelected = missionTypeSlot;
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

        MissionEstimate missionEstimates = MissionCalculator.EstimateMission(uiArmory.armory.storedWeapon, uiArmory.armory.storedBackpack, uiArmory.armory.storedArmor);

        durationText.text = missionEstimates.estimatedDuration.ToString() + "s +-";
        lootQualityText.text = missionEstimates.estimatedLootQualityMinimal.ToString("F0") + "% - " + missionEstimates.estimatedLootQualityMaximal.ToString("F0") + "%";
        lootAmountText.text = missionEstimates.estimatedLootAmount.ToString() + "-" + (missionEstimates.estimatedLootAmount + 1).ToString();
        zombieKillsText.text = missionEstimates.estimatedZombiesKills.ToString() + "+-";
        gearWearText.text = missionEstimates.estimatedGearWear.ToString() + "+- per item";
    }
}
