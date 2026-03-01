using UnityEngine;
using System; 

public class Scrap : Object
{
    public static Action<Inventory> OnScrapDestroyed;
    public static Action OnScrapLooted;
    public int salvageAmount;
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
    public override void SetValues() //when creating new object
    {
        switch (scrapType)
        {
            case ScrapType.SparePartsBox:
                salvageAmount = ScrapAssets.Instance.sparePartsBoxSO.salvageAmount;
                scrapName = ScrapAssets.Instance.sparePartsBoxSO.scrapName;
                break;
        }
    }
    public override void LoadValues(Object existingObject) // when object already exists
    {
        if (existingObject is Scrap existingScrap)
        {
            salvageAmount = existingScrap.salvageAmount;
            scrapName = existingScrap.scrapName;
        }
    }

    public void LootSalvage(int amount, Inventory currentlyInInventory)
    {
        salvageAmount -= amount;
        OnScrapLooted?.Invoke();

        if (salvageAmount <= 0)
        {
            DestroyObject(currentlyInInventory);
        }
    }

    public bool CanLootSalvage()
    {
        if (salvageAmount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void DestroyObject(Inventory currentlyInInventory) // should be destroyed when salvage amount raches zero
    {
        currentlyInInventory.RemoveObject(this);
        OnScrapDestroyed?.Invoke(currentlyInInventory);        
    }
}
