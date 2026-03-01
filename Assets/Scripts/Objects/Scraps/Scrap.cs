using UnityEngine;
using System; 

public class Scrap : Object
{
    public static Action<Inventory> OnScrapDestroyed;
    public int salvageAmount;
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
                break;
        }
    }
    public override void LoadValues(Object existingObject) // when object already exists
    {
        if (existingObject is Scrap existingScrap)
        {
            salvageAmount = existingScrap.salvageAmount;
        }
    }

    public override void DestroyObject(Inventory currentlyInInventory) // should be destroyed when salvage amount raches zero
    {
        currentlyInInventory.RemoveObject(this);
        OnScrapDestroyed?.Invoke(currentlyInInventory);
        Debug.Log("Weapon was destroyed in " + currentlyInInventory.ToString());
    }
}
