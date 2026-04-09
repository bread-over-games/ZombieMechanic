using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Bench;

public class UIInventory : MonoBehaviour
{
    private Inventory inventory;        
    [SerializeField] private GameObject singleItemInventoryWindow;
    [SerializeField] private TMP_Text interactableName;

    [Header("Weapons")]
    [SerializeField] private GameObject currentWeaponInfo;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image weaponDurabilityImage;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text durabilityText;

    [Header("Backpacks")]
    [SerializeField] private GameObject currentBackpackInfo;
    [SerializeField] private Image backpackImage;
    [SerializeField] private Image backpackDurabilityImage;
    [SerializeField] private TMP_Text backpackNameText;    
    [SerializeField] private TMP_Text backpackDurabilityText;
    [SerializeField] private TMP_Text backpackLootAmountText;

    [Header("Armors")]
    [SerializeField] private GameObject currentArmorInfo;
    [SerializeField] private Image armorImage;
    [SerializeField] private Image armorDurabilityImage;
    [SerializeField] private TMP_Text armorNameText;
    [SerializeField] private TMP_Text armorDurabilityText;
    [SerializeField] private TMP_Text armorLootQualityText;

    [Header("Scraps")]
    [SerializeField] private GameObject currentScrapInfo;
    [SerializeField] private Image scrapImage;
    [SerializeField] private Image scrapDurabilityImage;
    [SerializeField] private TMP_Text scrapNameText;
    [SerializeField] private TMP_Text scrapDurabilityText;

    [Header("Antibiotics")]
    [SerializeField] private GameObject currentAntibioticsInfo;
    [SerializeField] private Image antibioticsImage;
    [SerializeField] private TMP_Text antibioticsNameText;
    [SerializeField] private TMP_Text antibioticsAmountText;

    [Header("Empty inventory messages")]
    [SerializeField] private GameObject workbenchEmptyMessage;
    [SerializeField] private GameObject loottableEmptyMessage;
    [SerializeField] private GameObject medicalCabinetEmptyMessage;
    [SerializeField] private GameObject salvageTableEmptyMessage;
    [SerializeField] private GameObject toolboxEmptyMessage;

    [Header("ControlTips")]
    [SerializeField] private GameObject workbenchPickPlaceControls;
    [SerializeField] private GameObject workbenchRepairControls;
    [SerializeField] private GameObject salvageTablePickPlaceControls;
    [SerializeField] private GameObject salvageTableSalvageControls;
    [SerializeField] private GameObject tableControls;
    [SerializeField] private GameObject medicalCabinetControls;
    [SerializeField] private GameObject lootTableControls;

    private void Start()
    {
        UIFocusStack.Push(singleItemInventoryWindow);
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

    private void ShowSingleItemInventoryWindow(IInteractable interactableType)
    {
        if (interactableType is IBench bench)
        {
            if (bench.GetBenchType() == Bench.BenchType.Armory || bench.GetBenchType() == Bench.BenchType.StorageRack)
            {
                return;
            }
            singleItemInventoryWindow.SetActive(true);
            RefreshInventoryUI();
        }
    }

    private void HideSingleItemInventoryWindow(IInteractable interactableType)
    {
        if (interactableType is IBench bench)
        {
            if (bench.GetBenchType() == Bench.BenchType.Armory || bench.GetBenchType() == Bench.BenchType.StorageRack)
            {
                return;
            }
            singleItemInventoryWindow.SetActive(false);
            DropInventory();
        }
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

        interactableName.text = inventory.gameObject.GetComponent<IBench>().GetName();
        workbenchEmptyMessage.SetActive(false);
        loottableEmptyMessage.SetActive(false);
        salvageTableEmptyMessage.SetActive(false);
        medicalCabinetEmptyMessage.SetActive(false);
        toolboxEmptyMessage.SetActive(false);   

        lootTableControls.SetActive(false);
        salvageTablePickPlaceControls.SetActive(false);
        salvageTableSalvageControls.SetActive(false);
        tableControls.SetActive(false);
        medicalCabinetControls.SetActive(false);
        workbenchPickPlaceControls.SetActive(false);
        workbenchRepairControls.SetActive(false);

        currentWeaponInfo.SetActive(false);
        currentArmorInfo.SetActive(false);  
        currentBackpackInfo.SetActive(false);
        currentScrapInfo.SetActive(false);
        currentAntibioticsInfo.SetActive(false);

        if (inventory.GetObjectList().Count > 0)
        {
            DisplayInventory();
        } else
        {
            DisplayEmptyMessage();
        }

        DisplayControls();
    }

    private void DisplayControls()
    {
        switch (inventory.GetInventoryOfType())
        {
            case Inventory.InventoryOfType.Workbench:
                if (inventory.GetObjectList().Count > 0 || InventoriesController.Instance.playerInventory.GetObjectList().Count > 0)
                {
                    if (inventory.GetObjectList().Count > 0)
                    {                        
                        workbenchRepairControls.SetActive(true);
                    }
                    workbenchPickPlaceControls.SetActive(true);
                }
                break;
            case Inventory.InventoryOfType.LootTable:
                if (inventory.GetObjectList().Count > 0)
                {
                    lootTableControls.SetActive(true);
                }
                break;
            case Inventory.InventoryOfType.SalvageTable:
                if (inventory.GetObjectList().Count > 0 || InventoriesController.Instance.playerInventory.GetObjectList().Count > 0)
                {
                    if (inventory.GetObjectList().Count > 0)
                    {
                        salvageTableSalvageControls.SetActive(true);    
                    }
                    salvageTablePickPlaceControls.SetActive(true);
                }
                break;
            case Inventory.InventoryOfType.Table:
                if (inventory.GetObjectList().Count > 0 || InventoriesController.Instance.playerInventory.GetObjectList().Count > 0)
                {
                    tableControls.SetActive(true);
                }
                break;
            case Inventory.InventoryOfType.MedicalCabinet:
                if (ResourceController.Instance.GetAntibioticsAmount() > 0)
                {
                    medicalCabinetControls.SetActive(true);
                }                
                break;            
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
            case Inventory.InventoryOfType.SalvageTable:
                salvageTableEmptyMessage.SetActive(true);
                break;
            case Inventory.InventoryOfType.Table:
                toolboxEmptyMessage.SetActive(true);
                break;
            case Inventory.InventoryOfType.MedicalCabinet:
                medicalCabinetEmptyMessage.SetActive(true);
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
            case Antibiotics antibiotics:
                currentAntibioticsInfo.SetActive(true);
                antibioticsImage.sprite = inventory.GetObjectList()[0].GetObjectSprite();
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
                damageText.text = weapon.baseDamage.ToString();
                durabilityText.text = weapon.currentDurability.ToString() + "/" + weapon.maxDurability.ToString();
                weaponNameText.text = weapon.objectName.ToString();
                weaponDurabilityImage.fillAmount = (float)weapon.currentDurability / weapon.maxDurability;
                break;
            case Backpack backpack:
                backpackDurabilityText.text = backpack.currentDurability.ToString() + "/" + backpack.maxDurability.ToString();
                backpackNameText.text = backpack.objectName.ToString();
                backpackLootAmountText.text = backpack.backpackSize.ToString();
                backpackDurabilityImage.fillAmount = (float)backpack.currentDurability / backpack.maxDurability;
                break;
            case Armor armor:
                armorDurabilityText.text = armor.currentDurability.ToString() + "/" + armor.maxDurability.ToString();
                armorNameText.text = armor.objectName.ToString();
                armorLootQualityText.text = armor.lootQualityBonus.ToString();
                armorDurabilityImage.fillAmount = (float)armor.currentDurability / armor.maxDurability;
                break;
            case Scrap scrap:
                scrapNameText.text = scrap.scrapName.ToString();
                scrapDurabilityText.text = scrap.currentDurability.ToString() + "/" + scrap.maxDurability.ToString();
                scrapDurabilityImage.fillAmount = (float)scrap.currentDurability / scrap.maxDurability;
                break;
            case Antibiotics antibiotics:
                antibioticsNameText.text = antibiotics.objectName.ToString();
                antibioticsAmountText.text = antibiotics.currentDurability.ToString() + "/" + antibiotics.maxDurability.ToString();
                break;
        }        
    }
}