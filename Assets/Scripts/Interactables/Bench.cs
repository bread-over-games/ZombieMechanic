/// master class for all other banches

using UnityEngine;

public class Bench : MonoBehaviour, IInteractable
{
    [SerializeField] protected Inventory inventory;
    [SerializeField] private string interactableName;

    private Weapon currentObject;

    public void StartInteractionPrimary()
    {
        if (InventoriesController.Instance.playerInventory.GetObjectList().Count == 0)
        {
            inventory.SendObject(InventoriesController.Instance.playerInventory);
        }
    }

    public void EndInteractionPrimary()
    {

    }

    public virtual bool IsInteractionPossible()
    {
        return true;
    }

    public virtual void StartInteractionSecondary()
    {

    }

    public virtual void EndInteractionSecondary()
    {

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
