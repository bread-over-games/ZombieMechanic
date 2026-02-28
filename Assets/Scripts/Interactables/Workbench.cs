using UnityEngine;

public class Workbench : MonoBehaviour, IInteractable
{
    [HideInInspector] public bool isRepairing = false;
    [SerializeField] private Transform weaponPivot;
    private WeaponWorld currentWeapon;
    [SerializeField] private Inventory inventory;
    [SerializeField] private string interactableName;

    public void Update()
    {
        if (isRepairing)
        {
            DoRepair();            
        }
    }

    public void StartInteractionPrimary()
    {
        isRepairing = true;
    }

    public void EndInteractionPrimary()
    {
        isRepairing = false;
    }

    private void DoRepair()
    {        
        if (ResourceController.Instance.CanRepair())
        {
            ResourceController.Instance.ChangeSalvageAmount(-1);            
            inventory.GetWeaponList()[0].RepairWeapon(1);       
            
            // refresh inventory UI
        }        
    }

    public bool IsInteractionPossible()
    {
        return true;
    }

    public void StartInteractionSecondary()
    {
        inventory.SendWeapon(InventoriesController.Instance.armoryInventory);
    }

    public void EndInteractionSecondary()
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
