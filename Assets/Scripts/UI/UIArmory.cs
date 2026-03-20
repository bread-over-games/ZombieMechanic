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

    [Header("Weapon")]
    [SerializeField] private GameObject currentWeaponInfo;
    [SerializeField] private Image weaponImage;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private TMP_Text weaponDurabilityText;

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

    [Header("Empty messages")]
    [SerializeField] private GameObject weaponEmptyMessage;
    [SerializeField] private GameObject backpackEmptyMessage;
    [SerializeField] private GameObject armorEmptyMessage;

    private void OnEnable()
    {
        Inventory.OnInventoryChange += RefreshInventoryUI;
        PlayerInteraction.OnInteractableApproached += ShowArmoryWindow;
        PlayerInteraction.OnInteractableLeft += HideArmoryWindow;
    }

    private void OnDisable()
    {
        Inventory.OnInventoryChange -= RefreshInventoryUI;
        PlayerInteraction.OnInteractableApproached -= ShowArmoryWindow;
        PlayerInteraction.OnInteractableLeft -= HideArmoryWindow;
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
            weaponNameText.text = weapon.objectName.ToString();
        }

        if (armory.storedBackpack is Backpack backpack)
        {
            backpackDurabilityText.text = backpack.currentDurability.ToString() + "/" + backpack.maxDurability.ToString();  
            backpackNameText.text = backpack.objectName.ToString();
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
