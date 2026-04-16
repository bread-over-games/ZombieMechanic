using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Mission;

public class UIMissionSelect : MonoBehaviour
{
    [SerializeField] private UIArmory uiArmory;
    [SerializeField] private GameObject missionSelectWindow;

    public ButtonSelectorMissionTypes.MissionTypeSlot currentSlotSelected;
    private MissionType currentMissionType;

    public static Action<MissionType> OnCurrentMissionTypeSlotSelected;

    public Button scavengeMissionButton;
    public Button exterminationMissionButton;
    public GameObject antibioticsMissionContainer;
    public GameObject antibioticsDepletedContainer;

    [Header("Mission Estimates")]
    [SerializeField] private GameObject missionEstimates;
    [SerializeField] private TMP_Text durationText;
    [SerializeField] private TMP_Text lootQualityText;
    [SerializeField] private TMP_Text lootAmountText;
    [SerializeField] private TMP_Text gearWearText;
    [SerializeField] private TMP_Text zombieKillsText;

    public void OpenWindow()
    {
        PlayerInteraction.OnSecondaryInteractionInterceptor = SelectCurrentMissionType;
        missionSelectWindow.SetActive(true);

        if (SectorController.Instance.antibioticsDepleted)
        {
            antibioticsMissionContainer.SetActive(false);
            antibioticsDepletedContainer.SetActive(true);

            Navigation scavengeMissionNav = scavengeMissionButton.navigation;
            scavengeMissionNav.selectOnUp = exterminationMissionButton;
            scavengeMissionNav.selectOnDown = exterminationMissionButton;
            scavengeMissionButton.navigation = scavengeMissionNav;

            Navigation exterminationMissionNav = exterminationMissionButton.navigation;
            exterminationMissionNav.selectOnDown = scavengeMissionButton;
            exterminationMissionNav.selectOnUp = scavengeMissionButton;
            exterminationMissionButton.navigation = exterminationMissionNav;    
        }

        RefreshEstimatesUI();
    }

    public void CloseWindow()
    {
        EventSystem.current.SetSelectedGameObject(null);
        missionSelectWindow.SetActive(false);
        PlayerInteraction.OnSecondaryInteractionInterceptor = null;
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
