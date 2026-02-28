using UnityEngine;

public class Workbench : MonoBehaviour, IInteractable
{
    [HideInInspector] public bool isRepairing = false;
    [SerializeField] private Transform weaponPivot;
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
            Weapon currentWeapon = inventory.GetWeaponList()[0];

            if (currentWeapon.currentDurability < currentWeapon.maxDurability)
            {
                ResourceController.Instance.ChangeSalvageAmount(-2);
                currentWeapon.RepairWeapon(1);
            }            
        }        
    }

    private void DoSalvage() // scraps weapon for salvage
    {

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
