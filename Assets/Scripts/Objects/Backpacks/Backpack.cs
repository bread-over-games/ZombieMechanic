using System;
using UnityEngine;

public class Backpack : Object
{
    public enum BackpackType
    {
        SmallBackpack
    }

    public BackpackType backpackType;
    public string backpackName;

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
                backpackName = BackpackAssets.Instance.smallBackpackSO.backpackName;
                break;
        }
    }
    public override void LoadValues(Object existingObject) // when object already exists
    {
        if (existingObject is Backpack existingBackpack)
        {            
            maxDurability = existingBackpack.maxDurability;
            currentDurability = existingBackpack.currentDurability;
            backpackName = existingBackpack.backpackName;
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
        Debug.Log("Backpack was destroyed in " + inInventory.ToString());
    }

    public override void RepairObject(int repairAmount)
    {
        currentDurability += repairAmount;
        OnObjectRepair?.Invoke();
    }
}
