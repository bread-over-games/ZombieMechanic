/// this is the instance of a concrete weapon

using UnityEngine;

public class Weapon 
{
    public enum WeaponType
    {
        BaseballBat,
        Crowbar
    }

    public WeaponType weaponType;
    public int baseDamage;
    public int bonusDamage;
    public int maxDurability;
    public int currentDurability;
    public float timeAddedToOutside;
    public float timeToSpendOutside; // how long a weapon should be outside, decided when leaving Armory

    public Sprite GetWeaponSprite()
    {
        switch (weaponType)
        {
            default:
                case WeaponType.BaseballBat: return WeaponAssets.Instance.baseballBatVisual;
                case WeaponType.Crowbar: return WeaponAssets.Instance.crowbarVisual;
        }
    }

    public GameObject GetWeaponGameObject()
    {
        switch (weaponType)
        {
            default:
            case WeaponType.BaseballBat: return WeaponAssets.Instance.baseballBatVisualPrefab;
            case WeaponType.Crowbar: return WeaponAssets.Instance.crowbarVisualPrefab;
        }
    }

    public void SetValues() // when creating new weapon
    {
        switch (weaponType)
        {
            default:
            case WeaponType.BaseballBat:
                baseDamage = WeaponAssets.Instance.baseballBatSO.baseDamage;
                bonusDamage = Random.Range(0, WeaponAssets.Instance.baseballBatSO.baseDamage);
                maxDurability = WeaponAssets.Instance.baseballBatSO.maxDurability;
                currentDurability = Random.Range(1, WeaponAssets.Instance.baseballBatSO.maxDurability);

                break;
            case WeaponType.Crowbar:
                baseDamage = WeaponAssets.Instance.crowbarSO.baseDamage;
                bonusDamage = Random.Range(0, WeaponAssets.Instance.crowbarSO.baseDamage);
                maxDurability = WeaponAssets.Instance.crowbarSO.maxDurability;
                currentDurability = Random.Range(1, WeaponAssets.Instance.crowbarSO.maxDurability);
                break;                
        }
    }  
    
    public void LoadValues(Weapon existingWeapon) // when weapon already exists
    {
        baseDamage = existingWeapon.baseDamage;
        bonusDamage = existingWeapon.bonusDamage;
        maxDurability = existingWeapon.maxDurability;
        currentDurability = existingWeapon.currentDurability;
    }
}
