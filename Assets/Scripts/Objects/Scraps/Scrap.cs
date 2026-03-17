using UnityEngine;
using System; 

public class Scrap : Object
{
    public string scrapName;

    public enum ScrapType
    {
        SparePartsBox
    }

    public ScrapType scrapType;

    public override Sprite GetObjectSprite()
    {
        switch (scrapType)
        {
            default:
            case ScrapType.SparePartsBox: return ScrapAssets.Instance.sparePartsBoxSO.scrapVisual;
        }
    }
    public override GameObject GetObjectGameObject()
    {
        switch (scrapType)
        {
            default:
            case ScrapType.SparePartsBox: return ScrapAssets.Instance.sparePartsBoxSO.scrapVisualPrefab;
        }
    }
    public override void SetValues(float qualityMultiplier) //when creating new object
    {
        switch (scrapType)
        {
            case ScrapType.SparePartsBox:

                maxDurability = ScrapAssets.Instance.sparePartsBoxSO.maxDurability;
                currentDurability = UnityEngine.Random.Range((int)((ScrapAssets.Instance.sparePartsBoxSO.maxDurability / 100f) * qualityMultiplier), ScrapAssets.Instance.sparePartsBoxSO.maxDurability);
                scrapName = ScrapAssets.Instance.sparePartsBoxSO.scrapName;
                break;
        }
    }
    public override void LoadValues(Object existingObject) // when object already exists
    {
        if (existingObject is Scrap existingScrap)
        {
            maxDurability = existingScrap.maxDurability;    
            currentDurability = existingScrap.currentDurability;
            scrapName = existingScrap.scrapName;
        }
    }

    public override void DestroyObject() // should be destroyed when salvage amount raches zero
    {
        inInventory.RemoveObject(this);
        OnObjectDestroyed?.Invoke(inInventory);        
    }

    public override void RepairObject(int repairAmount)
    {
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
