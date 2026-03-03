using UnityEngine;

public class Armory : Bench, IInteractable
{
    public override void StartInteractionSecondary()
    {
        inventory.SetOutsideTimes();
        inventory.SendObject(InventoriesController.Instance.outsideInventory);        
    }

    public override void EndInteractionSecondary()
    {

    }
}
