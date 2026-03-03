using UnityEngine;
using System;
using System.Collections;
public class Workbench : MonoBehaviour, IInteractable
{
    [SerializeField] private float repairInterval;
    [SerializeField] private int repairSalvageCost;
    [SerializeField] private int repairValue;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private Inventory inventory;
    [SerializeField] private string interactableName;

    private Weapon currentWeapon;
    private Coroutine repairCoroutine;

    private void OnEnable()
    {
        Inventory.OnObjectReceive += AssignCurrentWeapon;
    }

    private void AssignCurrentWeapon(Inventory.InventoryOfType invOfType, Object obj)
    {
        if (invOfType == Inventory.InventoryOfType.Workbench && obj is Weapon weapon)
        {
            currentWeapon = weapon; 
        }
    }

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

    private void TryRepair()
    {        
        if (ResourceController.Instance.CanRepair(repairSalvageCost))
        {
            if (currentWeapon.currentDurability < currentWeapon.maxDurability)
            {
                repairCoroutine = StartCoroutine(DoRepair());
            }            
        }        
    }

    IEnumerator DoRepair()
    {
        while (currentWeapon.currentDurability < currentWeapon.maxDurability)
        {
            yield return new WaitForSeconds(repairInterval);
            ResourceController.Instance.ChangeSalvageAmount(-repairSalvageCost);
            currentWeapon.RepairWeapon(repairValue);

            if (!ResourceController.Instance.CanRepair(repairSalvageCost))
            {
                break;
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
        TryRepair();
    }

    public void EndInteractionSecondary()
    {
        if (repairCoroutine != null)
        {
            StopCoroutine(repairCoroutine);
            repairCoroutine = null;
        }
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
