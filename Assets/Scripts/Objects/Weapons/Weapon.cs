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

    public override void SetValues(float minimalLootQuality, float maximalLootQuality) // when creating new weapon
    {
        switch (weaponType)
        {
            default:
            case WeaponType.BaseballBat:
                baseDamage = UnityEngine.Random.Range(WeaponAssets.Instance.baseballBatSO.minimalBaseDamage, WeaponAssets.Instance.baseballBatSO.maximalBaseDamage);
                maxDurability = WeaponAssets.Instance.baseballBatSO.maxDurability;
                currentDurability = UnityEngine.Random.Range((int)((maxDurability / 100f) * minimalLootQuality), (int)((maxDurability / 100f) * maximalLootQuality));
                objectName = WeaponAssets.Instance.baseballBatSO.weaponName;

                break;
            case WeaponType.Crowbar:
                baseDamage = UnityEngine.Random.Range(WeaponAssets.Instance.crowbarSO.minimalBaseDamage, WeaponAssets.Instance.crowbarSO.maximalBaseDamage);
                maxDurability = WeaponAssets.Instance.crowbarSO.maxDurability;
                currentDurability = UnityEngine.Random.Range((int)((maxDurability / 100f) * minimalLootQuality), (int)((maxDurability / 100f) * maximalLootQuality));
                objectName = WeaponAssets.Instance.crowbarSO.weaponName;
                break;                
        }
    }  
    
    public override void LoadValues(Object existingObject) // when weapon already exists
    {
        if (existingObject is Weapon existingWeapon)
        {
            baseDamage = existingWeapon.baseDamage;
            maxDurability = existingWeapon.maxDurability;
            currentDurability = existingWeapon.currentDurability;
            objectName = existingWeapon.objectName;
        }        
    }

    public static WeaponType ChooseWeaponTypeToGenerate()
    {
        WeaponAssets weaponAssets = WeaponAssets.Instance;

        float total = weaponAssets.baseballBatSO.spawnChance + weaponAssets.crowbarSO.spawnChance;
        float roll = UnityEngine.Random.Range(0f, total);

        WeaponType weaponTypeToGenerate = WeaponType.Crowbar;

        if ((roll -= weaponAssets.baseballBatSO.spawnChance) < 0)
        {
            weaponTypeToGenerate = WeaponType.BaseballBat;
        }
        else if ((roll -= weaponAssets.crowbarSO.spawnChance) < 0)
        {
            weaponTypeToGenerate = WeaponType.Crowbar;
        }

        return weaponTypeToGenerate;
    }

    public override bool DamageObject(int damageAmount) // returns true when weapon is destroyed
    {
        currentDurability -= damageAmount;
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
