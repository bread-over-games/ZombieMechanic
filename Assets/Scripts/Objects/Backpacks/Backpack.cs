using System;
using UnityEngine;

[System.Serializable]
public class Backpack : Object
{
    public enum BackpackType
    {
        SmallBackpack
    }

    public BackpackType backpackType;

    public override Sprite GetObjectSprite()
    {
        switch (backpackType)
        {
            default:
            case BackpackType.SmallBackpack: return BackpackAssets.Instance.smallBackpackSO.backpackVisual;
        }
    }
    public override GameObject GetObjectGameObject()
    {
        switch (backpackType)
        {
            default:
            case BackpackType.SmallBackpack: return BackpackAssets.Instance.smallBackpackSO.backpackVisualPrefab;
        }
    }

    public override void SetValues() //when creating new object
    {
        switch (backpackType)
        {
            default:
            case BackpackType.SmallBackpack:
                maxDurability = BackpackAssets.Instance.smallBackpackSO.maxDurability;
                currentDurability = UnityEngine.Random.Range(1, BackpackAssets.Instance.smallBackpackSO.maxDurability);
                objectName = BackpackAssets.Instance.smallBackpackSO.backpackName;
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
}
