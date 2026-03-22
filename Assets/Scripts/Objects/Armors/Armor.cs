using UnityEngine;

[System.Serializable]
public class Armor : Object
{
    public enum ArmorType
    {
        BalisticVest
    }

    public ArmorType armorType;

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
}
