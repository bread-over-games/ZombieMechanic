using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventory : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private Transform weaponSlotContainer;
    [SerializeField] private Transform weaponSlotTemplate;
        
    [SerializeField] private Image weaponImage;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text durabilityText;
    [SerializeField] private TMP_Text interactableName;

    [SerializeField] private GameObject singleItemInventoryWindow;
    [SerializeField] private GameObject currentWeaponInfo;
    [SerializeField] private GameObject workbenchEmptyMessage;
    [SerializeField] private GameObject armoryEmptyMessage;
    [SerializeField] private GameObject loottableEmptyMessage;

    private void Start()
    {
        singleItemInventoryWindow.SetActive(false);
    }

    private void OnEnable()
    {
        Inventory.OnInventoryChange += RefreshInventoryUI;        
        PlayerInteraction.OnInteractableApproached += ToggleSingleItemInventoryWindow;
        PlayerInteraction.OnInteractableLeft += ToggleSingleItemInventoryWindow;
        Weapon.OnWeaponRepair += RefreshInventoryValues;
    }

    private void OnDisable()
    {
        Inventory.OnInventoryChange -= RefreshInventoryUI;        
        PlayerInteraction.OnInteractableApproached -= ToggleSingleItemInventoryWindow;
        PlayerInteraction.OnInteractableLeft -= ToggleSingleItemInventoryWindow;
        Weapon.OnWeaponRepair -= RefreshInventoryValues;
    }

    private void ToggleSingleItemInventoryWindow()
    {
        if (singleItemInventoryWindow.activeSelf)
        {
            singleItemInventoryWindow.SetActive(false);
            DropInventory();
        } else
        {
            singleItemInventoryWindow.SetActive(true);
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

        interactableName.text = inventory.gameObject.GetComponent<IInteractable>().GetName();
        workbenchEmptyMessage.SetActive(false);
        loottableEmptyMessage.SetActive(false);
        armoryEmptyMessage.SetActive(false);
        currentWeaponInfo.SetActive(false);

        if (inventory.GetWeaponList().Count > 0)
        {
            DisplayInventory();
        } else
        {
            DisplayEmptyMessage();
        }
    }

    private void DisplayEmptyMessage() 
    {        
        switch (inventory.GetInventoryOfType())
        {
            case Inventory.InventoryOfType.Workbench:
                workbenchEmptyMessage.SetActive(true);
                break;
            case Inventory.InventoryOfType.LootTable:
                loottableEmptyMessage.SetActive(true);
                break;
            case Inventory.InventoryOfType.Armory:
                armoryEmptyMessage.SetActive(true);
                break;
        }
    }

    private void DisplayInventory()
    {
        currentWeaponInfo.SetActive(true);

        weaponImage.sprite = inventory.GetWeaponList()[0].GetWeaponSprite();
        RefreshInventoryValues();
    }

    private void RefreshInventoryValues()
    {
        if (inventory == null) return;

        damageText.text = inventory.GetWeaponList()[0].baseDamage.ToString() + "+" + inventory.GetWeaponList()[0].bonusDamage.ToString();
        durabilityText.text = inventory.GetWeaponList()[0].currentDurability.ToString() + "/" + inventory.GetWeaponList()[0].maxDurability.ToString();
    }
}