using UnityEngine;
using System;
using System.Collections;
public class Workbench : Bench, IInteractable
{
    [SerializeField] private float repairInterval;
    [SerializeField] private int repairSalvageCost;
    [SerializeField] private int repairValue;    

    private Coroutine repairCoroutine;

    public void Awake()
    {
        acceptedTypes.Add(typeof(Weapon));
        acceptedTypes.Add(typeof(Backpack));
        acceptedTypes.Add(typeof(Armor));
    }

    private void OnEnable()
    {
        Inventory.OnObjectReceive += AssignCurrentObject;
    }

    private void OnDisable()
    {
        Inventory.OnObjectReceive -= AssignCurrentObject;
    }

    private void AssignCurrentObject(Inventory.InventoryOfType invOfType, Object obj)
    {
        if (invOfType == Inventory.InventoryOfType.Workbench)
        {
            currentObject = obj; 
        }
    }

    private void TryRepair()
    {        
        if (ResourceController.Instance.CanRepair(repairSalvageCost))
        {
            if (currentObject.currentDurability < currentObject.maxDurability)
            {
                repairCoroutine = StartCoroutine(DoRepair());
            }            
        }        
    }

    IEnumerator DoRepair()
    {
        while (currentObject.currentDurability < currentObject.maxDurability)
        {
            yield return new WaitForSeconds(repairInterval);
            ResourceController.Instance.ChangeSalvageAmount(-repairSalvageCost);
            currentObject.RepairObject(repairValue);

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
