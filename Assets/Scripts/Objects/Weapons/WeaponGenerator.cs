using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{    
    [SerializeField] private int generateWeaponChance; // a chance that weapon will be generated

    private void Start()
    {
        GenerateFirstWeapon();
    }

    private void OnEnable()
    {
        OutsideController.OnSurvivorReturned += GenerateWeapon;
    }

    private void OnDisable()
    {
        OutsideController.OnSurvivorReturned -= GenerateWeapon;
    }

    private void GenerateFirstWeapon() // generates first weapon for tutorial purpose
    {
        Weapon newWeapon = (new Weapon { weaponType = Weapon.WeaponType.BaseballBat });
        newWeapon.SetValues();
        InventoriesController.Instance.lootTableInventory.ReceiveObject(newWeapon);
    }

    private void GenerateWeapon() // generates completely new weapon
    {
        if (Random.Range(0, 100) < generateWeaponChance)
        {
            Weapon newWeapon = (new Weapon { weaponType = Weapon.WeaponType.BaseballBat });
            newWeapon.SetValues();
            InventoriesController.Instance.lootTableInventory.ReceiveObject(newWeapon);
        }        
    }
}
