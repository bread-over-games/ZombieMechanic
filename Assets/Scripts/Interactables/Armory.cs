using UnityEngine;

public class Armory : MonoBehaviour, IInteractable
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private string interactableName;


    public void StartInteractionPrimary()
    {

    }

    public void EndInteractionPrimary()
    {

    }

    public void StartInteractionSecondary()
    {
        inventory.SetOutsideTimes();
        inventory.SendObject(InventoriesController.Instance.outsideInventory);        
    }

    public void EndInteractionSecondary()
    {

    }

    public bool IsInteractionPossible()
    {
        return true;
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    public string GetName()
    {
        return interactableName;
    }
}
