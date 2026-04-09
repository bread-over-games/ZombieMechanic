using UnityEngine;
using static Weapon;

[System.Serializable]
public class Armor : Object
{
    public enum ArmorType
    {
        BalisticVest
    }

    public ArmorType armorType;
    public int lootQualityBonus;

    public override Sprite GetObjectSprite()
    {
        switch (armorType)
        {
            default:
            case ArmorType.BalisticVest: return ArmorAssets.Instance.balisticVestSO.armorVisual;          
        }
    }
    public override GameObject GetObjectGameObject()
    {
        switch (armorType)
        {
            default:
            case ArmorType.BalisticVest: return ArmorAssets.Instance.balisticVestSO.armorVisualPrefab;
        }
    }

    public override void SetValues(float minimalLootQuality, float maximalLootQuality) //when creating new object
    {        
        switch (armorType)
        {
            default:
            case ArmorType.BalisticVest:               
                maxDurability = ArmorAssets.Instance.balisticVestSO.maxDurability;
                currentDurability = Random.Range((int)((maxDurability / 100f) * minimalLootQuality), (int)((maxDurability / 100f) * maximalLootQuality));
                objectName = ArmorAssets.Instance.balisticVestSO.armorName;
                lootQualityBonus = ArmorAssets.Instance.balisticVestSO.lootQualityBonus;

                break;
        }
    }
    public override void LoadValues(Object existingObject) // when object already exists
    {
        if (existingObject is Armor existingArmor)
        {

            maxDurability = existingArmor.maxDurability;
            currentDurability = existingArmor.currentDurability;
            objectName = existingArmor.objectName;
        }
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

    public override bool DamageObject(int decayAmount) // returns true when weapon is destroyed
    {
        currentDurability -= decayAmount;
        OnObjectDamage?.Invoke();

        if (currentDurability < 0)
        {
            return true;
        }

        return false;
    }

    public static ArmorType ChooseArmorTypeToGenerate()
    {
        ArmorAssets armorAssets = ArmorAssets.Instance;

        float total = armorAssets.balisticVestSO.spawnChance;
        float roll = UnityEngine.Random.Range(0f, total);

        ArmorType armorTypeToGenerate = ArmorType.BalisticVest;

        if ((roll -= armorAssets.balisticVestSO.spawnChance) < 0)
        {
            armorTypeToGenerate = ArmorType.BalisticVest;
        }
        /*else if ((roll -= armorAssets.crowbarSO.spawnChance) < 0)
        {
            weaponTypeToGenerate = Weapon.WeaponType.Crowbar;
        }*/

        return armorTypeToGenerate;
    }
}
