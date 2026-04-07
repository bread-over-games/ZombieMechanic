/// master class for all other banches

using UnityEngine;
using System.Collections.Generic;
using System;

public class Bench : MonoBehaviour, IBench
{
    public enum BenchType
    { 
        Armory,
        LootTable,
        SalvageTable,
        Table,
        Workbench,
        MedicalCabinet,
        StorageRack
    }

    public BenchType benchType;

    [SerializeField] protected Inventory inventory;
    [SerializeField] private string interactableName;
    protected List<System.Type> acceptedTypes = new List<System.Type>();

    public static Action OnObjectPicked;
    public static Action OnObjectDeposited;

    public Object currentObject;

    public virtual void StartInteractionPrimary()
    {
        if (inventory == null) return;

        Inventory playerInventory = InventoriesController.Instance.playerInventory;

        if (playerInventory.GetObjectList().Count == 0) // player has no item in hands - wants to pick up
        {
            if (inventory.GetObjectList().Count == 0) return; // bench emty, nothing to do
            
                inventory.SendObject(playerInventory, inventory.GetObjectList()[0]);
                OnObjectPicked?.Invoke(); // player picked item on bench - subscribe animation SetCarrying(true)
        }
        else // player has item in hands - try to depisot
        {
            if (inventory.GetObjectList().Count >= inventory.GetCapacity()) return; // bench full
            if (!CanAcceptObject(playerInventory.GetObjectList()[0])) return;
            playerInventory.SendObject(inventory, playerInventory.GetObjectList()[0]);
            OnObjectDeposited?.Invoke(); // player deposited item on bench - subscribe SetCarrying(false) animation
        }
    }

    public BenchType GetBenchType()
    {
        return benchType;
    }

    public bool CanAcceptObject(Object objectToPlace)
    {
        if (!acceptedTypes.Contains(objectToPlace.GetType()))
        {
            return false;
        }        

        return true;
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
