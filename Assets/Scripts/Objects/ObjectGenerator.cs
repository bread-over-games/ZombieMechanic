using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public static ObjectGenerator Instance { get; private set; }

    [SerializeField] private int generateWeaponChance; // a chance that weapon will be generated
    [SerializeField] private int generateScrapChance;
    [SerializeField] private int generateBackpackChance;
    [SerializeField] private int generateArmorChance;
    [SerializeField] private int generateAntibioticsChance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void GenerateLoot(Mission mission, float minimalLootQuality, float maximalLootQuality) // generates completely new object
    {
        float total = generateWeaponChance + generateScrapChance + generateBackpackChance + generateArmorChance + generateAntibioticsChance;
        float roll = Random.Range(0f, total);

        Object loot = null;

        if ((roll -= generateWeaponChance) < 0)
        {
            loot = new Weapon { weaponType = Weapon.ChooseWeaponTypeToGenerate() };
        }
        else if ((roll -= generateScrapChance) < 0)
        {
            loot = new Scrap { scrapType = Scrap.ScrapType.SparePartsBox };
        }
        else if ((roll -= generateBackpackChance) < 0)
        {
            loot = new Backpack { backpackType = Backpack.ChooseBackpackTypeToGenerate() };
        }
        else if ((roll -= generateArmorChance) < 0)
        {
            loot = new Armor { armorType = Armor.ChooseArmorTypeToGenerate() };
        }
        else if ((roll -= generateAntibioticsChance) < 0)
        {
            loot = new Antibiotics { antibioType = Antibiotics.AntibioticsType.BSAntibiotics };
        }

        if (loot == null) return;

        loot.SetValues(minimalLootQuality, maximalLootQuality);
        InventoriesController.Instance.lootTableInventory.ReceiveObject(loot);
    }
}
