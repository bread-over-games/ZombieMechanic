/// this is the instance of a concrete weapon

using UnityEngine;
using System;

public class Weapon : Object
{
    public enum WeaponType
    {
        BaseballBat,
        Crowbar
    }

    public static Action OnWeaponRepair; // called when weapon is repaired
    public static Action<Inventory> OnWeaponDestroyed;

    public WeaponType weaponType;
    public string weaponName;
    public int baseDamage;
    public int bonusDamage;
    public int maxDurability;
    public int currentDurability;
    public float timeAddedToOutside;
    public float timeToSpendOutside; // how long a weapon should be outside, decided when leaving Armory
    public int weaponDecayRate = 5; // how much the weapon will decay after being outside

    public override Sprite GetObjectSprite()
    {
        switch (weaponType)
        {
            default:
                case WeaponType.BaseballBat: return WeaponAssets.Instance.baseballBatSO.weaponVisual;
                case WeaponType.Crowbar: return WeaponAssets.Instance.crowbarSO.weaponVisual;
        }
    }

    public override GameObject GetObjectGameObject()
    {
        switch (weaponType)
        {
            default:
            case WeaponType.BaseballBat: return WeaponAssets.Instance.baseballBatSO.weaponVisualPrefab;
            case WeaponType.Crowbar: return WeaponAssets.Instance.crowbarSO.weaponVisualPrefab;
        }
    }

    public override void SetValues() // when creating new weapon
    {
        switch (weaponType)
        {
            default:
            case WeaponType.BaseballBat:
                baseDamage = WeaponAssets.Instance.baseballBatSO.baseDamage;
                bonusDamage = UnityEngine.Random.Range(0, WeaponAssets.Instance.baseballBatSO.baseDamage);
                maxDurability = WeaponAssets.Instance.baseballBatSO.maxDurability;
                currentDurability = UnityEngine.Random.Range(1, WeaponAssets.Instance.baseballBatSO.maxDurability);
                weaponName = WeaponAssets.Instance.baseballBatSO.weaponName;

                break;
            case WeaponType.Crowbar:
                baseDamage = WeaponAssets.Instance.crowbarSO.baseDamage;
                bonusDamage = UnityEngine.Random.Range(0, WeaponAssets.Instance.crowbarSO.baseDamage);
                maxDurability = WeaponAssets.Instance.crowbarSO.maxDurability;
                currentDurability = UnityEngine.Random.Range(1, WeaponAssets.Instance.crowbarSO.maxDurability);
                weaponName = WeaponAssets.Instance.crowbarSO.weaponName;
                break;                
        }
    }  
    
    public override void LoadValues(Object existingObject) // when weapon already exists
    {
        if (existingObject is Weapon existingWeapon)
        {
            baseDamage = existingWeapon.baseDamage;
            bonusDamage = existingWeapon.bonusDamage;
            maxDurability = existingWeapon.maxDurability;
            currentDurability = existingWeapon.currentDurability;
            weaponName = existingWeapon.weaponName;
        }        
    }

    public bool DecayWeapon(Inventory currentlyInInventory)
    {
        currentDurability -= weaponDecayRate;

        if (currentDurability <= 0)
        {
            DestroyObject(currentlyInInventory);
            return true;
        }

        return false;
    }

    public override void DestroyObject(Inventory currentlyInInventory)
    {
        currentlyInInventory.RemoveObject(this);
        OnWeaponDestroyed?.Invoke(currentlyInInventory);
        Debug.Log("Weapon was destroyed in " + currentlyInInventory.ToString());
    }

    public void RepairWeapon(int repairAmount)
    {        
        currentDurability += repairAmount;
        OnWeaponRepair?.Invoke();
    }
}
