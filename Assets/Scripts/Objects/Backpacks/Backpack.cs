using System;
using UnityEngine;
using static Armor;

[System.Serializable]
public class Backpack : Object
{
    public enum BackpackType
    {
        SmallBackpack,
        HuntingBackpack,
        DuffelBag
    }

    public BackpackType backpackType;
    public int backpackSize;

    public override Sprite GetObjectSprite()
    {
        switch (backpackType)
        {
            default:
            case BackpackType.SmallBackpack: return BackpackAssets.Instance.smallBackpackSO.backpackVisual;
            case BackpackType.DuffelBag: return BackpackAssets.Instance.duffelBagSO.backpackVisual;
            case BackpackType.HuntingBackpack: return BackpackAssets.Instance.huntingBackpackSO.backpackVisual;
        }
    }
    public override GameObject GetObjectGameObject()
    {
        switch (backpackType)
        {
            default:
            case BackpackType.SmallBackpack: return BackpackAssets.Instance.smallBackpackSO.backpackVisualPrefab;
            case BackpackType.DuffelBag: return BackpackAssets.Instance.duffelBagSO.backpackVisualPrefab;
            case BackpackType.HuntingBackpack: return BackpackAssets.Instance.huntingBackpackSO.backpackVisualPrefab;
        }
    }

    public override void SetValues(float minimalLootQuality, float maximalLootQuality) //when creating new object
    {
        switch (backpackType)
        {
            default:
            case BackpackType.SmallBackpack:
                maxDurability = BackpackAssets.Instance.smallBackpackSO.maxDurability;
                currentDurability = UnityEngine.Random.Range((int)((maxDurability / 100f) * minimalLootQuality), (int)((maxDurability / 100f) * maximalLootQuality));
                backpackSize = BackpackAssets.Instance.smallBackpackSO.backpackSize;
                objectName = BackpackAssets.Instance.smallBackpackSO.backpackName;
                break;
            case BackpackType.DuffelBag:
                maxDurability = BackpackAssets.Instance.duffelBagSO.maxDurability;
                currentDurability = UnityEngine.Random.Range((int)((maxDurability / 100f) * minimalLootQuality), (int)((maxDurability / 100f) * maximalLootQuality));
                backpackSize = BackpackAssets.Instance.duffelBagSO.backpackSize;
                objectName = BackpackAssets.Instance.duffelBagSO.backpackName;
                break;
            case BackpackType.HuntingBackpack:
                maxDurability = BackpackAssets.Instance.huntingBackpackSO.maxDurability;
                currentDurability = UnityEngine.Random.Range((int)((maxDurability / 100f) * minimalLootQuality), (int)((maxDurability / 100f) * maximalLootQuality));
                backpackSize = BackpackAssets.Instance.huntingBackpackSO.backpackSize;
                objectName = BackpackAssets.Instance.huntingBackpackSO.backpackName;
                break;
        }
    }
    public override void LoadValues(Object existingObject) // when object already exists
    {
        if (existingObject is Backpack existingBackpack)
        {            
            maxDurability = existingBackpack.maxDurability;
            currentDurability = existingBackpack.currentDurability;
            objectName = existingBackpack.objectName;
        }
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

    public static BackpackType ChooseBackpackTypeToGenerate()
    {
        BackpackAssets backpackAssets = BackpackAssets.Instance;

        float total = backpackAssets.smallBackpackSO.spawnChance + backpackAssets.duffelBagSO.spawnChance + backpackAssets.huntingBackpackSO.spawnChance;
        float roll = UnityEngine.Random.Range(0f, total);

        BackpackType backpackTypeToGenerate = BackpackType.SmallBackpack;

        if ((roll -= backpackAssets.smallBackpackSO.spawnChance) < 0)
        {
            backpackTypeToGenerate = BackpackType.SmallBackpack;
        }
        else if ((roll -= backpackAssets.duffelBagSO.spawnChance) < 0)
        {
            backpackTypeToGenerate = BackpackType.DuffelBag;
        }
        else if ((roll -= backpackAssets.huntingBackpackSO.spawnChance) < 0)
        {
            backpackTypeToGenerate = BackpackType.HuntingBackpack;
        }

        return backpackTypeToGenerate;
    }
}
