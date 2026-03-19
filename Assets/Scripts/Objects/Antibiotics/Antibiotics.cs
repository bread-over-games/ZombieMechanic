using UnityEngine;
using static Armor;

[System.Serializable]
public class Antibiotics : Object
{
    public enum AntibioticsType
    {
        BSAntibiotics
    }

    public AntibioticsType antibioType;

    public override Sprite GetObjectSprite()
    {
        switch (antibioType)
        {
            default:
            case AntibioticsType.BSAntibiotics: return AntibioticsAssets.Instance.bsAntibioticsSO.antibioticsVisual;
        }
    }
    public override GameObject GetObjectGameObject()
    {
        switch (antibioType)
        {
            default:
            case AntibioticsType.BSAntibiotics: return AntibioticsAssets.Instance.bsAntibioticsSO.antibioticsVisualPrefab;
        }
    }

    public override void SetValues(float qualityMultiplier) //when creating new object
    {
        switch (antibioType)
        {
            default:
            case AntibioticsType.BSAntibiotics:
                maxDurability = AntibioticsAssets.Instance.bsAntibioticsSO.maxAmount;
                currentDurability = Random.Range((int)((maxDurability / 100f) * qualityMultiplier), maxDurability);
                objectName = AntibioticsAssets.Instance.bsAntibioticsSO.antibioticsName;
                break;
        }
    }
    public override void LoadValues(Object existingObject) // when object already exists
    {
        if (existingObject is Antibiotics existingAntibiotics)
        {
            maxDurability = existingAntibiotics.maxDurability;
            currentDurability = existingAntibiotics.currentDurability;
            objectName = existingAntibiotics.objectName;
        }
    }

    public override void DestroyObject()
    {
        inInventory.RemoveObject(this);
        OnObjectDestroyed?.Invoke(inInventory);
    }

    public override void RepairObject(int repairAmount)
    {

    }
    public override bool DamageObject(int decayAmount) // returns true when weapon is destroyed
    {
        if (currentDurability < 0)
        {
            return true;
        }

        return false;
    }
}
