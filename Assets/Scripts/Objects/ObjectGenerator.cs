using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{    
    [SerializeField] private int generateWeaponChance; // a chance that weapon will be generated
    [SerializeField] private int generateScrapChance;
    [SerializeField] private int generateMedicineChance;

    private void Start()
    {
        GenerateFirstWeapon();
    }

    private void OnEnable()
    {
        OutsideController.OnSurvivorReturned += GenerateObject;
    }

    private void OnDisable()
    {
        OutsideController.OnSurvivorReturned -= GenerateObject;
    }

    private void GenerateFirstWeapon() // generates first weapon for tutorial purpose
    {
        Weapon newWeapon = (new Weapon { weaponType = Weapon.WeaponType.BaseballBat });
        newWeapon.SetValues();
        InventoriesController.Instance.lootTableInventory.ReceiveObject(newWeapon);
    }

    private void GenerateFirstScrap() // generates first scrap for tutorial purpose
    {
        Scrap newScrap = (new Scrap { scrapType = Scrap.ScrapType.SparePartsBox });
        newScrap.SetValues();
        InventoriesController.Instance.lootTableInventory.ReceiveObject(newScrap);
    }

    private void GenerateObject() // generates completely new object
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
    }
}
