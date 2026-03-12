using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIArmory : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private Armory armory;
    [SerializeField] private GameObject armoryWindow;

    [Header("Weapon")]
    [SerializeField] private GameObject currentWeaponInfo;
    [SerializeField] private Image weaponImage;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private TMP_Text weaponDurabilityText;
    [SerializeField] private TMP_Text survivalChanceText;

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
    [SerializeField] private TMP_Text armorRunLengthText;
    [SerializeField] private TMP_Text armorItemQualityText;

    [Header("Empty messages")]
    [SerializeField] private GameObject weaponEmptyMessage;
    [SerializeField] private GameObject backpackEmptyMessage;
    [SerializeField] private GameObject armorEmptyMessage;

    private void OnEnable()
    {
        Inventory.OnInventoryChange += RefreshInventoryUI;
        PlayerInteraction.OnInteractableApproached += ToggleArmoryWindow;
        PlayerInteraction.OnInteractableLeft += ToggleArmoryWindow;
    }

    private void OnDisable()
    {
        Inventory.OnInventoryChange -= RefreshInventoryUI;
        PlayerInteraction.OnInteractableApproached -= ToggleArmoryWindow;
        PlayerInteraction.OnInteractableLeft -= ToggleArmoryWindow;
    }

    private void ToggleArmoryWindow(Bench.BenchType benchType)
    {
        if (benchType != Bench.BenchType.Armory)
        {
            return;
        }

        if (armoryWindow.activeSelf)
        {
            armoryWindow.SetActive(false);
            DropInventory();
        }
        else
        {
            armoryWindow.SetActive(true);
            RefreshInventoryUI();
        }
    }

    public void SetInventory(Inventory currentInventory)
    {
        inventory = currentInventory;
    }

    private void DropInventory()
    {
        inventory = null;
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

        if (armory.storedArmor == null)
        {
            armorEmptyMessage.SetActive(true);
        } else
        {
            currentArmorInfo.SetActive(true);
        }

        if (armory.storedWeapon == null)
        {
            weaponEmptyMessage.SetActive(true);
        }
        else
        {
            currentWeaponInfo.SetActive(true);
        }

        if (armory.storedBackpack == null)
        {
            backpackEmptyMessage.SetActive(true);
        }
        else
        {
            currentBackpackInfo.SetActive(true);
        }
    }


}
