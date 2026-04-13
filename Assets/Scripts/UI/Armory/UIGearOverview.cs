using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIGearOverview : MonoBehaviour
{
    [SerializeField] private UIArmory uiArmory;

    [SerializeField] private GameObject gearOverviewWindow;
    public ButtonSelector.ArmorySlot currentSlotSelected;
    public static Action<ButtonSelector.ArmorySlot> OnCurrentArmorySlotSelected;

    [Header("Weapon")]
    [SerializeField] private GameObject currentWeaponInfo;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image weaponDurabilityImage;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private TMP_Text weaponDurabilityText;
    [SerializeField] private TMP_Text weaponDamageText;

    [Header("Backpack")]
    [SerializeField] private GameObject currentBackpackInfo;
    [SerializeField] private Image backpackImage;
    [SerializeField] private Image backpackDurabilityImage;
    [SerializeField] private TMP_Text backpackNameText;
    [SerializeField] private TMP_Text backpackDurabilityText;
    [SerializeField] private TMP_Text backpackItemAmountText;

    [Header("Armor")]
    [SerializeField] private GameObject currentArmorInfo;
    [SerializeField] private Image armorImage;
    [SerializeField] private Image armorDurabilityImage;
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

    [Header("Controls")]
    [SerializeField] private GameObject armoryPickPlaceControls;
    [SerializeField] private GameObject armorySendMissionControls;
    [SerializeField] private GameObject armorySelectGearControls;


    private void OnEnable()
    {
        Inventory.OnInventoryChange += RefreshInventoryUI;
        Inventory.OnInventoryChange += RefreshEstimatesUI;
    }

    private void OnDisable()
    {
        Inventory.OnInventoryChange -= RefreshInventoryUI;
        Inventory.OnInventoryChange -= RefreshEstimatesUI;
    }

    public void OpenWindow()
    {
        gearOverviewWindow.SetActive(true);
        RefreshEstimatesUI();
        RefreshInventoryUI();
    }

    public void CloseWindow()
    {
        EventSystem.current.SetSelectedGameObject(null);
        gearOverviewWindow.SetActive(false);
    }

    private void RefreshEstimatesUI()
    {
        if (uiArmory.inventory == null)
        {
            return;
        }

        MissionEstimate missionEstimates = MissionCalculator.EstimateMission(uiArmory.armory.storedWeapon, uiArmory.armory.storedBackpack, uiArmory.armory.storedArmor, Mission.MissionType.Scavenge);

        durationText.text = missionEstimates.estimatedDuration.ToString() + "s +-";
        lootQualityText.text = missionEstimates.estimatedLootQualityMinimal.ToString("F0") + "% - " + missionEstimates.estimatedLootQualityMaximal.ToString("F0") + "%";
        lootAmountText.text = missionEstimates.estimatedLootAmount.ToString() + "-" + (missionEstimates.estimatedLootAmount + 1).ToString();
        zombieKillsText.text = missionEstimates.estimatedZombiesKills.ToString() + "+-";
        gearWearText.text = missionEstimates.estimatedGearWear.ToString() + "+- per item";
    }

    private void RefreshInventoryUI()
    {
        if (uiArmory.inventory == null)
        {
            return;
        }

        weaponEmptyMessage.SetActive(false);
        armorEmptyMessage.SetActive(false);
        backpackEmptyMessage.SetActive(false);

        currentWeaponInfo.SetActive(false);
        currentArmorInfo.SetActive(false);
        currentBackpackInfo.SetActive(false);

        if (uiArmory.armory.storedArmor is Armor armor)
        {
            currentArmorInfo.SetActive(true);
            armorImage.sprite = armor.GetObjectSprite();
            RefreshInventoryValues();
        }
        else
        {
            armorEmptyMessage.SetActive(true);
        }

        if (uiArmory.armory.storedWeapon is Weapon weapon)
        {
            currentWeaponInfo.SetActive(true);
            weaponImage.sprite = weapon.GetObjectSprite();
            RefreshInventoryValues();
        }
        else
        {
            weaponEmptyMessage.SetActive(true);
        }

        if (uiArmory.armory.storedBackpack is Backpack backpack)
        {
            currentBackpackInfo.SetActive(true);
            backpackImage.sprite = backpack.GetObjectSprite();
            RefreshInventoryValues();
        }
        else
        {
            backpackEmptyMessage.SetActive(true);
        }

        DisplayControls();
    }

    private void DisplayControls()
    {
        armoryPickPlaceControls.SetActive(false);

        if (uiArmory.inventory.GetObjectList().Count > 0 || InventoriesController.Instance.playerInventory.GetObjectList().Count > 0)
        {
            armoryPickPlaceControls.SetActive(uiArmory.armory.isAvailableForMission);
        }

        armorySelectGearControls.SetActive(uiArmory.armory.isAvailableForMission);
        armorySendMissionControls.SetActive(uiArmory.armory.isAvailableForMission);
    }

    private void RefreshInventoryValues()
    {
        Color red = new Color(255f / 255f, 68f / 255f, 68f / 255f);
        Color standardBlue = new Color(77f / 255f, 135f / 255f, 147f / 255f);

        if (uiArmory.inventory == null || uiArmory.inventory.GetObjectList().Count == 0) return;

        if (uiArmory.armory.storedArmor is Armor armor)
        {
            armorDurabilityText.text = armor.currentDurability.ToString() + "/" + armor.maxDurability.ToString();
            armorNameText.text = armor.objectName.ToString();
            armorDurabilityImage.fillAmount = (float)armor.currentDurability / armor.maxDurability;            
            armorLootQualityText.text = armor.lootQualityBonus.ToString();
            
            if (armor.canBeDestroyed)
            {
                armorDurabilityImage.color = red;
            } else
            {
                armorDurabilityImage.color = standardBlue;
            }
        }

        if (uiArmory.armory.storedWeapon is Weapon weapon)
        {
            weaponDurabilityText.text = weapon.currentDurability.ToString() + "/" + weapon.maxDurability.ToString();
            weaponDamageText.text = weapon.baseDamage.ToString();
            weaponNameText.text = weapon.objectName.ToString();
            weaponDurabilityImage.fillAmount = (float)weapon.currentDurability / weapon.maxDurability;

            if (weapon.canBeDestroyed)
            {
                weaponDurabilityImage.color = red;
            }
            else
            {
                weaponDurabilityImage.color = standardBlue;
            }
        }

        if (uiArmory.armory.storedBackpack is Backpack backpack)
        {
            backpackDurabilityText.text = backpack.currentDurability.ToString() + "/" + backpack.maxDurability.ToString();
            backpackNameText.text = backpack.objectName.ToString();
            backpackDurabilityImage.fillAmount = (float)backpack.currentDurability / backpack.maxDurability;

            if (backpack.canBeDestroyed)
            {
                backpackDurabilityImage.color = red;
            }
            else
            {
                backpackDurabilityImage.color = standardBlue;
            }
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
