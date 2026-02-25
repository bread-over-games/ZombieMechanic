using UnityEngine;

public class Armory : MonoBehaviour, IInteractable
{

    [SerializeField] private Inventory inventory;

    public void StartInteractionPrimary()
    {

    }

    public void EndInteractionPrimary()
    {

    }

    public void StartInteractionSecondary()
    {
        inventory.SetOutsideTimes();
        inventory.SendWeapon(InventoriesController.Instance.outsideInventory);        
    }

    public void EndInteractionSecondary()
    {

    }

    public bool IsInteractionPossible()
    {
        return true;
    }
}
