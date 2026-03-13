using System;
using UnityEngine;
using static Weapon;

public class Backpack : Object
{
    public enum BackpackType
    {
        SmallBackpack
    }

    public static Action OnBackpackRepair; // called when weapon is repaired
    public static Action OnBackpackDamage;
    public static Action<Inventory> OnBackpackDestroyed;

    public BackpackType backpackType;
    public string backpackName;
    public int maxDurability;
    public int currentDurability;

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

    public bool DamageBackpack(int decayAmount) // returns true when weapon is destroyed
    {
        currentDurability -= decayAmount;
        OnBackpackDamage?.Invoke();        

        if (currentDurability <= 0)
        {
            return true;
        }

        return false;
    }

    public override void DestroyObject()
    {
        inInventory.RemoveObject(this);
        OnBackpackDestroyed?.Invoke(inInventory);
        Debug.Log("Backpack was destroyed in " + inInventory.ToString());
    }

    public void RepairBackpack(int repairAmount)
    {
        currentDurability += repairAmount;
        OnBackpackRepair?.Invoke();
    }
}
