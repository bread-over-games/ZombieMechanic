/// master class for all other banches

using UnityEngine;
using System.Collections.Generic;
using System;

public class Bench : MonoBehaviour, IInteractable
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
        if (InventoriesController.Instance.playerInventory.GetObjectList().Count == 0) // if player has no item in hands
        {
            if (inventory.GetObjectList().Count > 0)
            {
                inventory.SendObject(InventoriesController.Instance.playerInventory, inventory.GetObjectList()[0]);
                OnObjectPicked?.Invoke(); // player picked item on bench
            }
        }
        else // player has item in hands
        {
            OnObjectDeposited?.Invoke(); // player deposited item on bench
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
