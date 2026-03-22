using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIArmory : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private Armory armory;
    [SerializeField] private GameObject armoryWindow;
    [SerializeField] private GameObject firstSelected;
    public ButtonSelector.ArmorySlot currentSlotSelected;
    public static Action<ButtonSelector.ArmorySlot> OnCurrentArmorySlotSelected;
        
    [SerializeField] private GameObject objectsSlots;
    [SerializeField] private GameObject survivorOnMission;

    [Header("Weapon")]
    [SerializeField] private GameObject currentWeaponInfo;
    [SerializeField] private Image weaponImage;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private TMP_Text weaponDurabilityText;
    [SerializeField] private TMP_Text weaponDamageText;

    [Header("Backpack")]
    [SerializeField] private GameObject currentBackpackInfo;
    [SerializeField] private Image backpackImage;
    [SerializeField] private TMP_Text backpackNameText;
    [SerializeField] private TMP_Text backpackDurabilityText;
    [SerializeField] private TMP_Text backpackItemAmountText;

    [Header("Armor")]
    [SerializeField] private GameObject currentArmorInfo;
    [SerializeField] private Image armorImage;
    [SerializeField] private TMP_Text armorNameText;
    [SerializeField] private TMP_Text armorDurabilityText;
    [SerializeField] private TMP_Text armorLootQualityText;

    [Header("Mission Estimates")]
    [SerializeField] private GameObject missionEstimates;
    [SerializeField] private TMP_Text durationText;
    [SerializeField] private TMP_Text lootQualityText;
    [SerializeField] private TMP_Text lootAmountText;
    [SerializeField] private TMP_Text gearWearText;
    [SerializeField] private TMP_Text zombieKillsText;

    [Header("Empty messages")]
    [SerializeField] private GameObject weaponEmptyMessage;
    [SerializeField] private GameObject backpackEmptyMessage;
    [SerializeField] private GameObject armorEmptyMessage;

    private void OnEnable()
    {
        Inventory.OnInventoryChange += RefreshInventoryUI;
        Inventory.OnInventoryChange += RefreshEstimatesUI;
        PlayerInteraction.OnInteractableApproached += ShowArmoryWindow;
        PlayerInteraction.OnInteractableLeft += HideArmoryWindow;
        MissionController.OnMissionStarted += ChangeMissionStateGUI;
        MissionController.OnMissionCompleted += ChangeMissionStateGUI;
    }

    private void OnDisable()
    {
        Inventory.OnInventoryChange -= RefreshInventoryUI;
        Inventory.OnInventoryChange -= RefreshEstimatesUI;
        PlayerInteraction.OnInteractableApproached -= ShowArmoryWindow;
        PlayerInteraction.OnInteractableLeft -= HideArmoryWindow;
        MissionController.OnMissionStarted -= ChangeMissionStateGUI;
        MissionController.OnMissionCompleted -= ChangeMissionStateGUI;
    }

    private void ShowArmoryWindow(Bench.BenchType benchType)
    {
        if (benchType != Bench.BenchType.Armory)
        {
            return;
        }

        armoryWindow.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelected);
        RefreshInventoryUI();
        RefreshEstimatesUI();
        ChangeMissionStateGUI(null);
    }

    private void HideArmoryWindow(Bench.BenchType benchType)
    {
        if (benchType != Bench.BenchType.Armory)
        {
            return;
        }

        armoryWindow.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        DropInventory();
        DropArmory();        
    }

    public void SetInventory(Inventory currentInventory)
    {
        inventory = currentInventory;
    }

    public void SetArmory(Armory currentArmory)
    {
        armory = currentArmory;
    }

    private void DropInventory()
    {
        inventory = null;
    }

    private void DropArmory()
    {
        armory = null;
    }

    private void RefreshEstimatesUI()
    {
        if (inventory == null)
        {
            return;
        }

        MissionEstimate missionEstimates = MissionCalculator.EstimateMission(armory.storedWeapon, armory.storedBackpack, armory.storedArmor);

        durationText.text = missionEstimates.estimatedDuration.ToString() + "s +-";
        lootQualityText.text = missionEstimates.estimatedLootQualityMinimal.ToString("F0") + "% - " + missionEstimates.estimatedLootQualityMaximal.ToString("F0") + "%";
        lootAmountText.text = missionEstimates.estimatedLootAmount.ToString();
        zombieKillsText.text = missionEstimates.estimatedZombiesKills.ToString() + "+-";
        gearWearText.text = missionEstimates.estimatedGearWear.ToString() + "+- per item";
    }

    private void RefreshInventoryUI()
    {
        if (inventory == null)
        {
            return;
        }

        weaponEmptyMessage.SetActive(false);
        armorEmptyMessage.SetActive(false);
        backpackEmptyMessage.SetActive(false);

        currentWeaponInfo.SetActive(false);
        currentArmorInfo.SetActive(false);
        currentBackpackInfo.SetActive(false);

        if (armory.storedArmor is Armor armor)
        {
            currentArmorInfo.SetActive(true);
            armorImage.sprite = armor.GetObjectSprite();
            RefreshInventoryValues();
        } else
        {
            armorEmptyMessage.SetActive(true);
        }

        if (armory.storedWeapon is Weapon weapon)
        {
            currentWeaponInfo.SetActive(true);
            weaponImage.sprite = weapon.GetObjectSprite();
            RefreshInventoryValues();
        }
        else
        {
            weaponEmptyMessage.SetActive(true);
        }

        if (armory.storedBackpack is Backpack backpack)
        {
            currentBackpackInfo.SetActive(true);
            backpackImage.sprite = backpack.GetObjectSprite();
            RefreshInventoryValues();
        }
        else
        {
            backpackEmptyMessage.SetActive(true);
        }        
    }

    private void RefreshInventoryValues()
    {
        if (inventory == null || inventory.GetObjectList().Count == 0) return;

        if (armory.storedArmor is Armor armor)
        {
            armorDurabilityText.text = armor.currentDurability.ToString() + "/" + armor.maxDurability.ToString();
            armorNameText.text = armor.objectName.ToString();
        }

        if (armory.storedWeapon is Weapon weapon)
        {
            weaponDurabilityText.text = weapon.currentDurability.ToString() + "/" + weapon.maxDurability.ToString();
            weaponDamageText.text = weapon.baseDamage.ToString();
            weaponNameText.text = weapon.objectName.ToString();
        }

        if (armory.storedBackpack is Backpack backpack)
        {
            backpackDurabilityText.text = backpack.currentDurability.ToString() + "/" + backpack.maxDurability.ToString();  
            backpackNameText.text = backpack.objectName.ToString();
        }
    }

    private void ChangeMissionStateGUI(Mission mission)
    {
        if (!armory.isAvailableForMission)
        {
            objectsSlots.SetActive(false);
            missionEstimates.SetActive(false);
            survivorOnMission.SetActive(true);            
        } else
        {            
            objectsSlots.SetActive(true);
            missionEstimates.SetActive(true);
            survivorOnMission.SetActive(false);
        }
    }

    public void OnButtonSelected(ButtonSelector.ArmorySlot armorySlot)
    {        
        currentSlotSelected = armorySlot;
        OnCurrentArmorySlotSelected?.Invoke(currentSlotSelected);
    }

    public void OnButtonDeselected(ButtonSelector.ArmorySlot armorySlot)
    {
        
    }
}
;