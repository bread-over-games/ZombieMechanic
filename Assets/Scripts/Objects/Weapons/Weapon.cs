/// this is the instance of a concrete weapon

using UnityEngine;
using System;

[System.Serializable]
public class Weapon : Object
{
    public enum WeaponType
    {
        BaseballBat,
        Crowbar
    }    

    public WeaponType weaponType;
    public int baseDamage;
    public int bonusDamage;

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
                objectName = WeaponAssets.Instance.baseballBatSO.weaponName;

                break;
            case WeaponType.Crowbar:
                baseDamage = WeaponAssets.Instance.crowbarSO.baseDamage;
                bonusDamage = UnityEngine.Random.Range(0, WeaponAssets.Instance.crowbarSO.baseDamage);
                maxDurability = WeaponAssets.Instance.crowbarSO.maxDurability;
                currentDurability = UnityEngine.Random.Range(1, WeaponAssets.Instance.crowbarSO.maxDurability);
                objectName = WeaponAssets.Instance.crowbarSO.weaponName;
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
            objectName = existingWeapon.objectName;
        }        
    }

    public override bool DamageObject(int decayAmount) // returns true when weapon is destroyed
    {
        currentDurability -= decayAmount;
        OnObjectDamage?.Invoke();

        if (currentDurability <= 0)
        {
            return true;
        }

        return false;
    }

    public override void DestroyObject()
    {
        inInventory.RemoveObject(this);
        OnObjectDestroyed?.Invoke(inInventory);
    }

    public override void RepairObject(int repairAmount)
    {        
        currentDurability += repairAmount;
        OnObjectRepair?.Invoke();
    }
}
