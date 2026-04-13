// is for hard coded spawning of objects in the beginning

using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [System.Serializable]
    public struct ObjectSpawnConfig
    {
        public GeneratableObjectType objectType;
        [Range(0f, 100f)] public float minQuality;
        [Range(0f, 100f)] public float maxQuality;
    }

    public enum GeneratableObjectType
    {
        Weapon,
        Armor,
        Backpack,
        Scrap,
        Antibiotics
    }

    [SerializeField] private ObjectSpawnConfig spawnConfig;

    private void OnEnable()
    {
        TutorialController.OnTutorialEnd += SpawnObject;
        TutorialController.OnSpawnTutorialSpareParts += SpawnTutorialSpareParts;
        TutorialController.OnSpawnTutorialBaseballBat += SpawnTutorialWeapon;
    }

    private void OnDisable()
    {
        TutorialController.OnTutorialEnd -= SpawnObject; 
        TutorialController.OnSpawnTutorialSpareParts -= SpawnTutorialSpareParts;
        TutorialController.OnSpawnTutorialBaseballBat -= SpawnTutorialWeapon;
    }    

    private void SpawnTutorialSpareParts(Inventory sourceInventory)
    {
        if (sourceInventory == inventory)
        {
            Object loot = new Scrap { scrapType = Scrap.ScrapType.SparePartsBox };
            loot.SetValues(49, 51);
            inventory.ReceiveObject(loot);
        }
    }

    private void SpawnTutorialWeapon(Inventory sourceInventory)
    {
        if (sourceInventory == inventory)
        {
            Object loot = new Weapon { weaponType = Weapon.WeaponType.Baton };
            loot.SetValues(49, 51);
            inventory.ReceiveObject(loot);
        }
    }

    private void SpawnObject()
    {
        if (!TutorialController.Instance.skipTutorial && inventory.GetInventoryOfType() == Inventory.InventoryOfType.LootTable) // if this bench is LootTable and player did not skip tutorial, don't spawn another weapon
        {
            return;
        }

        Object loot = spawnConfig.objectType switch
        {
            GeneratableObjectType.Weapon => new Weapon { weaponType = Weapon.WeaponType.BaseballBat },
            GeneratableObjectType.Armor => new Armor { armorType = Armor.ArmorType.BalisticVest },
            GeneratableObjectType.Backpack => new Backpack { backpackType = Backpack.BackpackType.SmallBackpack },
            GeneratableObjectType.Scrap => new Scrap { scrapType = Scrap.ScrapType.SparePartsBox },
            GeneratableObjectType.Antibiotics => new Antibiotics { antibioType = Antibiotics.AntibioticsType.BSAntibiotics },
            _ => null
        };

        if (loot == null) return;

        float min = Mathf.Min(spawnConfig.minQuality, spawnConfig.maxQuality);
        float max = Mathf.Max(spawnConfig.minQuality, spawnConfig.maxQuality);
        loot.SetValues(min, max);
        inventory.ReceiveObject(loot);
    }
}