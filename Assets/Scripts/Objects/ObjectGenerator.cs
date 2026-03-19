using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public static ObjectGenerator Instance { get; private set; }

    [SerializeField] private int generateWeaponChance; // a chance that weapon will be generated
    [SerializeField] private int generateScrapChance;
    [SerializeField] private int generateBackpackChance;
    [SerializeField] private int generateArmorChance;
    [SerializeField] private int generateMedicineChance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        //GenerateFirstWeapon();
        GenerateFirstAntibiotics();
    }

    private void GenerateFirstWeapon() // generates first weapon for tutorial purpose
    {
        Weapon newWeapon = (new Weapon { weaponType = Weapon.WeaponType.BaseballBat });
        newWeapon.SetValues(40f);
        InventoriesController.Instance.lootTableInventory.ReceiveObject(newWeapon);
    }

    private void GenerateFirsBackpack() // generates first backpack for tutorial purpose
    {
        Backpack newBackpack = (new Backpack { backpackType = Backpack.BackpackType.SmallBackpack });
        newBackpack.SetValues(40f);
        InventoriesController.Instance.lootTableInventory.ReceiveObject(newBackpack);
    }

    private void GenerateFirsArmor() // generates first backpack for tutorial purpose
    {
        Armor newArmor = (new Armor { armorType = Armor.ArmorType.BalisticVest });
        newArmor.SetValues(40f);
        InventoriesController.Instance.lootTableInventory.ReceiveObject(newArmor);
    }

    private void GenerateFirstScrap() // generates first scrap for tutorial purpose
    {
        Scrap newScrap = (new Scrap { scrapType = Scrap.ScrapType.SparePartsBox });
        newScrap.SetValues(40f);
        InventoriesController.Instance.lootTableInventory.ReceiveObject(newScrap);
    }

    private void GenerateFirstAntibiotics()
    {
        Antibiotics newAntibiotics = (new Antibiotics { antibioType = Antibiotics.AntibioticsType.BSAntibiotics });
        newAntibiotics.SetValues(40f);
        InventoriesController.Instance.lootTableInventory.ReceiveObject(newAntibiotics);
    }

    public void GenerateLoot(Mission mission, float lootQuality) // generates completely new object
    {
        float total = generateWeaponChance + generateScrapChance + generateBackpackChance + generateArmorChance;
        float roll = Random.Range(0f, total);

        Object loot = null;

        if ((roll -= generateWeaponChance) < 0)
        {
            loot = new Weapon { weaponType = Weapon.WeaponType.BaseballBat };
        }
        else if ((roll -= generateScrapChance) < 0)
        {
            loot = new Scrap { scrapType = Scrap.ScrapType.SparePartsBox };
        }
        else if ((roll -= generateBackpackChance) < 0)
        {
            loot = new Backpack { backpackType = Backpack.BackpackType.SmallBackpack };
        }
        else if ((roll -= generateArmorChance) < 0)
        {
            loot = new Armor { armorType = Armor.ArmorType.BalisticVest };
        }

        if (loot == null) return;

        loot.SetValues(lootQuality);
        InventoriesController.Instance.lootTableInventory.ReceiveObject(loot);
    }
}
