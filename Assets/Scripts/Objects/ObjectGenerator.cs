using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{    
    [SerializeField] private int generateWeaponChance; // a chance that weapon will be generated
    [SerializeField] private int generateScrapChance;
    [SerializeField] private int generateBackpackChance;
    [SerializeField] private int generateArmorChance;
    [SerializeField] private int generateMedicineChance;

    private void Start()
    {
        GenerateFirstWeapon();
        GenerateFirsBackpack();
        GenerateFirsArmor();
    }

    private void OnEnable()
    {
        MissionController.OnMissionCompleted += GenerateLoot;
    }

    private void OnDisable()
    {
        MissionController.OnMissionCompleted -= GenerateLoot;
    }

    private void GenerateFirstWeapon() // generates first weapon for tutorial purpose
    {
        Weapon newWeapon = (new Weapon { weaponType = Weapon.WeaponType.BaseballBat });
        newWeapon.SetValues();
        InventoriesController.Instance.lootTableInventory.ReceiveObject(newWeapon);
    }

    private void GenerateFirsBackpack() // generates first backpack for tutorial purpose
    {
        Backpack newBackpack = (new Backpack { backpackType = Backpack.BackpackType.SmallBackpack });
        newBackpack.SetValues();
        InventoriesController.Instance.lootTableInventory.ReceiveObject(newBackpack);
    }

    private void GenerateFirsArmor() // generates first backpack for tutorial purpose
    {
        Armor newArmor = (new Armor { armorType = Armor.ArmorType.BalisticVest });
        newArmor.SetValues();
        InventoriesController.Instance.lootTableInventory.ReceiveObject(newArmor);
    }

    private void GenerateFirstScrap() // generates first scrap for tutorial purpose
    {
        Scrap newScrap = (new Scrap { scrapType = Scrap.ScrapType.SparePartsBox });
        newScrap.SetValues();
        InventoriesController.Instance.lootTableInventory.ReceiveObject(newScrap);
    }

    private void GenerateLoot(Mission mission) // generates completely new object
    {
        if (Random.Range(0, 100) < generateWeaponChance)
        {
            Weapon newWeapon = (new Weapon { weaponType = Weapon.WeaponType.BaseballBat });
            newWeapon.SetValues();
            Debug.Log(newWeapon.weaponType.ToString());
            InventoriesController.Instance.lootTableInventory.ReceiveObject(newWeapon);            
        }

        if (Random.Range(0, 100) < generateScrapChance)
        {
            Scrap newScrap = (new Scrap { scrapType = Scrap.ScrapType.SparePartsBox });
            newScrap.SetValues();
            Debug.Log(newScrap.scrapType.ToString());
            InventoriesController.Instance.lootTableInventory.ReceiveObject(newScrap);         
        }

        if (Random.Range(0, 100) < generateBackpackChance)
        {
            Backpack newBackpack = (new Backpack { backpackType = Backpack.BackpackType.SmallBackpack });
            newBackpack.SetValues();
            Debug.Log(newBackpack.backpackType.ToString());
            InventoriesController.Instance.lootTableInventory.ReceiveObject(newBackpack);
        }

        if (Random.Range(0, 100) < generateArmorChance)
        {
            Armor newArmor = (new Armor { armorType = Armor.ArmorType.BalisticVest });
            newArmor.SetValues();
            Debug.Log(newArmor.armorType.ToString());
            InventoriesController.Instance.lootTableInventory.ReceiveObject(newArmor);
        }
    }
}
