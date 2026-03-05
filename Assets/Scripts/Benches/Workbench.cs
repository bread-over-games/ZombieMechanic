using UnityEngine;
using System;
using System.Collections;
public class Workbench : Bench, IInteractable
{
    [SerializeField] private float repairInterval;
    [SerializeField] private int repairSalvageCost;
    [SerializeField] private int repairValue;    

    private Weapon currentWeapon;
    private Coroutine repairCoroutine;

    public void Awake()
    {
        acceptedTypes.Add(typeof(Weapon));
    }

    private void OnEnable()
    {
        Inventory.OnObjectReceive += AssignCurrentWeapon;
    }

    private void OnDisable()
    {
        Inventory.OnObjectReceive -= AssignCurrentWeapon;
    }

    private void AssignCurrentWeapon(Inventory.InventoryOfType invOfType, Object obj)
    {
        if (invOfType == Inventory.InventoryOfType.Workbench && obj is Weapon weapon)
        {
            currentWeapon = weapon; 
        }
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

    public override void StartInteractionSecondary()
    {
        TryRepair();
    }

    public override void EndInteractionSecondary()
    {
        if (repairCoroutine != null)
        {
            StopCoroutine(repairCoroutine);
            repairCoroutine = null;
        }
    }
}
