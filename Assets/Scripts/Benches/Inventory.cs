using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public enum InventoryOfType
    {
        Workbench,
        LootTable,
        Armory,
        Storage,
        Outside,
        Player,
        Table,
        SalvageTable
    }

    [SerializeField] private InventoryOfType inventoryOfType;

    private List<Object> objectList = new List<Object>();

    public static Action<InventoryOfType, Object> OnObjectReceive; // whben Inventory receives wepaon
    public static Action<InventoryOfType, Object> OnObjectSend; // when inventory sends weapon
    public static Action OnInventoryChange; // when something changes in inventory
    [SerializeField] public int capacity;

    public void RemoveObject(Object obj)
    {
        objectList.Remove(obj);
        OnInventoryChange?.Invoke();
    }

    public List<Object> GetObjectList()
    {
        return objectList;
    }

    public InventoryOfType GetInventoryOfType()
    {
        return inventoryOfType;
    }

    public void ReceiveObject(Object obj)
    {
        objectList.Add(obj);
        obj.AssignOwnerInventory(this);
        switch (obj)
        {
            case Weapon weapon:
                weapon.LoadValues(weapon);
                OnObjectReceive?.Invoke(inventoryOfType, weapon);
                break;
            case Backpack backpack:
                backpack.LoadValues(backpack);
                OnObjectReceive?.Invoke(inventoryOfType, backpack);
                break;
            case Scrap scrap:
                scrap.LoadValues(scrap);
                OnObjectReceive?.Invoke(inventoryOfType, scrap);
                break;
            /*case Medicine medicine:
                medicine.LoadValues(medicine);
                break;*/
        }        
        OnInventoryChange?.Invoke();
    }

    public void SendObject(Inventory target) // moves weapon from list to another list
    {
        if (objectList.Count > 0)
        {
            Object objToSend = objectList[0];
            objectList.Remove(objectList[0]);
            OnObjectSend?.Invoke(inventoryOfType, objToSend);

            target.ReceiveObject(objToSend);
        } 
    }

    public void SendObjectOnMission() // this needs huge refactor
    {
        Object objToSend = objectList[0];
        objectList[0].ClearOwnerInventory();
        RemoveObject(objectList[0]);        
        OnObjectSend?.Invoke(inventoryOfType, objToSend);
        OnInventoryChange?.Invoke();
    }

    public int GetCapacity()
    {
        return capacity;
    }

}
