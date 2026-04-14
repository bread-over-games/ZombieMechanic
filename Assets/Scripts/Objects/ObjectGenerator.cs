using UnityEngine;
using System;

public class ObjectGenerator : MonoBehaviour
{
    public static ObjectGenerator Instance { get; private set; }

    [SerializeField] private int generateWeaponChance; // a chance that weapon will be generated
    [SerializeField] private int generateScrapChance;
    [SerializeField] private int generateBackpackChance;
    [SerializeField] private int generateArmorChance;
    [SerializeField] private int generateAntibioticsChance;

    public static Action<int> OnAntibioticsGenerated;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void GenerateLoot(Mission mission, float minimalLootQuality, float maximalLootQuality, Mission.MissionType missionType) // generates completely new object
    {
        int genWeapChanceHolder = generateWeaponChance;
        int genScrapChanceHolder = generateScrapChance;
        int genBackpackChanceHolder = generateBackpackChance;
        int genArmorChanceHolder = generateArmorChance;
        int genAtbChanceHolder = generateAntibioticsChance;

        switch (missionType)
        {
            case Mission.MissionType.Scavenge:
                break;
            case Mission.MissionType.Antibiotics:
                genWeapChanceHolder = 0;
                genScrapChanceHolder = 0;
                genBackpackChanceHolder = 0;
                genArmorChanceHolder = 0;
                genAtbChanceHolder = 100;

                break;
            case Mission.MissionType.Extermination:
                genWeapChanceHolder = 0;
                genScrapChanceHolder = 20;
                genBackpackChanceHolder = 0;
                genArmorChanceHolder = 0;
                genAtbChanceHolder = 0;
                break;
        }    

        if (SectorController.Instance.antibioticsDepleted)
        {
            genAtbChanceHolder = 0;
        }

        float total = genWeapChanceHolder + genScrapChanceHolder + genBackpackChanceHolder + genArmorChanceHolder + genAtbChanceHolder;
        float roll = UnityEngine.Random.Range(0f, total);

        Object loot = null;

        if ((roll -= genWeapChanceHolder) < 0)
        {
            loot = new Weapon { weaponType = Weapon.ChooseWeaponTypeToGenerate() };
        }
        else if ((roll -= genScrapChanceHolder) < 0)
        {
            loot = new Scrap { scrapType = Scrap.ScrapType.SparePartsBox };
        }
        else if ((roll -= genBackpackChanceHolder) < 0)
        {
            loot = new Backpack { backpackType = Backpack.ChooseBackpackTypeToGenerate() };
        }
        else if ((roll -= genArmorChanceHolder) < 0)
        {
            loot = new Armor { armorType = Armor.ChooseArmorTypeToGenerate() };
        }
        else if ((roll -= genAtbChanceHolder) < 0)
        {
            loot = new Antibiotics { antibioType = Antibiotics.AntibioticsType.BSAntibiotics };
            if (loot.currentDurability < 1)
            {
                loot.currentDurability = 1;
            }
            OnAntibioticsGenerated?.Invoke(loot.currentDurability);     
        }

        if (loot == null) return;        

        loot.SetValues(minimalLootQuality, maximalLootQuality);
        InventoriesController.Instance.lootTableInventory.ReceiveObject(loot);        
    }
}
