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

    [Header("Backpacks")]
    [SerializeField] private GameObject currentBackpackInfo;
    [SerializeField] private Image backpackImage;
    [SerializeField] private TMP_Text backpackNameText;    
    [SerializeField] private TMP_Text backpackDurabilityText;

    [Header("Armors")]
    [SerializeField] private GameObject currentArmorInfo;
    [SerializeField] private Image armorImage;
    [SerializeField] private TMP_Text armorNameText;
    [SerializeField] private TMP_Text armorDurabilityText;

    [Header("Scraps")]
    [SerializeField] private GameObject currentScrapInfo;
    [SerializeField] private Image scrapImage;
    [SerializeField] private TMP_Text scrapNameText;
    [SerializeField] private TMP_Text scrapDurabilityText;

    [Header("Empty inventory messages")]
    [SerializeField] private GameObject workbenchEmptyMessage;
    [SerializeField] private GameObject loottableEmptyMessage;

    private void Start()
    {
        singleItemInventoryWindow.SetActive(false);
    }

    private void OnEnable()
    {
        Inventory.OnInventoryChange += RefreshInventoryUI;        
        PlayerInteraction.OnInteractableApproached += ShowSingleItemInventoryWindow;
        PlayerInteraction.OnInteractableLeft += HideSingleItemInventoryWindow;
        Object.OnObjectRepair += RefreshInventoryValues;
        Object.OnObjectDamage += RefreshInventoryValues;
    }

    private void OnDisable()
    {
        Inventory.OnInventoryChange -= RefreshInventoryUI;        
        PlayerInteraction.OnInteractableApproached -= ShowSingleItemInventoryWindow;
        PlayerInteraction.OnInteractableLeft -= HideSingleItemInventoryWindow;
        Object.OnObjectRepair -= RefreshInventoryValues;
        Object.OnObjectDamage -= RefreshInventoryValues;
    }

    private void ShowSingleItemInventoryWindow(Bench.BenchType benchType)
    {
        if (benchType == Bench.BenchType.Armory)
        {
            return;
        }

        singleItemInventoryWindow.SetActive(true);
        RefreshInventoryUI();
    }

    private void HideSingleItemInventoryWindow(Bench.BenchType benchType)
    {
        if (benchType == Bench.BenchType.Armory)
        {
            return;
        }

        singleItemInventoryWindow.SetActive(false);
        DropInventory();
    }

    private void ToggleSingleItemInventoryWindow(Bench.BenchType benchType)
    {
        if (benchType == Bench.BenchType.Armory)
        {
            return;
        }

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
        
        currentWeaponInfo.SetActive(false);
        currentArmorInfo.SetActive(false);  
        currentBackpackInfo.SetActive(false);
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
            case Backpack backpack:
                currentBackpackInfo.SetActive(true);
                backpackImage.sprite = inventory.GetObjectList()[0].GetObjectSprite();
                break;
            case Armor armor:
                currentArmorInfo.SetActive(true);
                armorImage.sprite = inventory.GetObjectList()[0].GetObjectSprite();
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
        if (inventory == null || inventory.GetObjectList().Count == 0) return;

        switch (inventory.GetObjectList()[0])
        {
            case Weapon weapon:
                damageText.text = weapon.baseDamage.ToString() + "+" + weapon.bonusDamage.ToString();
                durabilityText.text = weapon.currentDurability.ToString() + "/" + weapon.maxDurability.ToString();
                weaponNameText.text = weapon.objectName.ToString();
                break;
            case Backpack backpack:
                backpackDurabilityText.text = backpack.currentDurability.ToString() + "/" + backpack.maxDurability.ToString();
                backpackNameText.text = backpack.objectName.ToString();
                break;
            case Armor armor:
                armorDurabilityText.text = armor.currentDurability.ToString() + "/" + armor.maxDurability.ToString();
                armorNameText.text = armor.objectName.ToString();
                break;
            case Scrap scrap:
                scrapNameText.text = scrap.scrapName.ToString();
                scrapDurabilityText.text = scrap.currentDurability.ToString() + "/" + scrap.maxDurability.ToString();
                break;
                /*case Medicine medicine:
                    medicine.LoadValues(medicine);
                    break;*/
        }        
    }
}