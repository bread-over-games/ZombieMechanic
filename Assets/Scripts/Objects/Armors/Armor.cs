using UnityEngine;
using static Weapon;

[System.Serializable]
public class Armor : Object
{
    public enum ArmorType
    {
        BalisticVest,
        RiotGear,
        LeatherJacker
    }

    public ArmorType armorType;
    public int lootQualityBonus;

    public override Sprite GetObjectSprite()
    {
        switch (armorType)
        {
            default:
            case ArmorType.BalisticVest: return ArmorAssets.Instance.balisticVestSO.armorVisual;
            case ArmorType.LeatherJacker: return ArmorAssets.Instance.leatherJacketSO.armorVisual;
            case ArmorType.RiotGear: return ArmorAssets.Instance.riotGearSO.armorVisual;
        }
    }
    public override GameObject GetObjectGameObject()
    {
        switch (armorType)
        {
            default:
            case ArmorType.BalisticVest: return ArmorAssets.Instance.balisticVestSO.armorVisualPrefab;
            case ArmorType.LeatherJacker: return ArmorAssets.Instance.leatherJacketSO.armorVisualPrefab;
            case ArmorType.RiotGear: return ArmorAssets.Instance.riotGearSO.armorVisualPrefab;
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
            case ArmorType.RiotGear:
                maxDurability = ArmorAssets.Instance.riotGearSO.maxDurability;
                currentDurability = Random.Range((int)((maxDurability / 100f) * minimalLootQuality), (int)((maxDurability / 100f) * maximalLootQuality));
                objectName = ArmorAssets.Instance.riotGearSO.armorName;
                lootQualityBonus = ArmorAssets.Instance.riotGearSO.lootQualityBonus;
                break;
            case ArmorType.LeatherJacker:
                maxDurability = ArmorAssets.Instance.leatherJacketSO.maxDurability;
                currentDurability = Random.Range((int)((maxDurability / 100f) * minimalLootQuality), (int)((maxDurability / 100f) * maximalLootQuality));
                objectName = ArmorAssets.Instance.leatherJacketSO.armorName;
                lootQualityBonus = ArmorAssets.Instance.leatherJacketSO.lootQualityBonus;
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
        CanBeDestroyedChange();
        OnObjectRepair?.Invoke();
    }

    public override bool DamageObject(int decayAmount) // returns true when weapon is destroyed
    {
        currentDurability -= decayAmount;
        CanBeDestroyedChange();
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

        float total = armorAssets.balisticVestSO.spawnChance + armorAssets.riotGearSO.spawnChance + armorAssets.leatherJacketSO.spawnChance;
        float roll = Random.Range(0f, total);

        ArmorType armorTypeToGenerate = ArmorType.BalisticVest;

        if ((roll -= armorAssets.balisticVestSO.spawnChance) < 0)
        {
            armorTypeToGenerate = ArmorType.BalisticVest;
        }
        else if ((roll -= armorAssets.leatherJacketSO.spawnChance) < 0)
        {
            armorTypeToGenerate = ArmorType.LeatherJacker;
        }
        else if ((roll -= armorAssets.riotGearSO.spawnChance) < 0)
        {
            armorTypeToGenerate = ArmorType.RiotGear;
        }

        return armorTypeToGenerate;
    }

    private void CanBeDestroyedChange()
    {
        canBeDestroyed = (float)currentDurability / maxDurability < 0.20f;
    }
}
