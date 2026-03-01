using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventory : MonoBehaviour
{
    private Inventory inventory;        
    [SerializeField] private GameObject singleItemInventoryWindow;
    [SerializeField] private TMP_Text interactableName;

    [Header("Weapons")]
    [SerializeField] private GameObject currentWeaponInfo;
    [SerializeField] private Image weaponImage;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text durabilityText;    

    [Header("Scraps")]
    [SerializeField] private GameObject currentScrapInfo;
    [SerializeField] private Image scrapImage;
    [SerializeField] private TMP_Text sparePartsNameText;
    [SerializeField] private TMP_Text sparePartsText;

    [Header("Empty inventory messages")]
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
        currentScrapInfo.SetActive(false);

        if (inventory.GetObjectList().Count > 0)
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
        switch (inventory.GetObjectList()[0])
        {
            case Weapon weapon:
                currentWeaponInfo.SetActive(true);
                weaponImage.sprite = inventory.GetObjectList()[0].GetObjectSprite();
                break;
            case Scrap scrap:
                currentScrapInfo.SetActive(true); 
                scrapImage.sprite = inventory.GetObjectList()[0].GetObjectSprite();
                break;
        }

        RefreshInventoryValues();
    }

    private void RefreshInventoryValues()
    {
        if (inventory == null) return;

        switch (inventory.GetObjectList()[0])
        {
            case Weapon weapon:
                damageText.text = weapon.baseDamage.ToString() + "+" + weapon.bonusDamage.ToString();
                durabilityText.text = weapon.currentDurability.ToString() + "/" + weapon.maxDurability.ToString();
                weaponNameText.text = weapon.weaponName.ToString();
                break;
            case Scrap scrap:
                sparePartsNameText.text = scrap.scrapName.ToString();
                sparePartsText.text = scrap.salvageAmount.ToString();
                break;
                /*case Medicine medicine:
                    medicine.LoadValues(medicine);
                    break;*/
        }        
    }
}