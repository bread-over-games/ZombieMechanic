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

    [SerializeReference] private List<Object> objectList = new List<Object>();

    public static Action<InventoryOfType, Object, Inventory> OnObjectReceive; // whben Inventory receives wepaon
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
                OnObjectReceive?.Invoke(inventoryOfType, weapon, this);
                break;
            case Backpack backpack:
                backpack.LoadValues(backpack);
                OnObjectReceive?.Invoke(inventoryOfType, backpack, this);
                break;
            case Armor armor:
                armor.LoadValues(armor);
                OnObjectReceive?.Invoke(inventoryOfType, armor, this);
                break;
            case Scrap scrap:
                scrap.LoadValues(scrap);
                OnObjectReceive?.Invoke(inventoryOfType, scrap, this);
                break;
            /*case Medicine medicine:
                medicine.LoadValues(medicine);
                break;*/
        }   
        OnInventoryChange?.Invoke();
    }

    public void SendObject(Inventory target, Object objToSend) // moves weapon from list to another list
    {
        if (objectList.Count > 0)
        {            
            objectList.Remove(objToSend);            
            OnObjectSend?.Invoke(inventoryOfType, objToSend);

            target.ReceiveObject(objToSend);
        } 
    }

    public void SendObjectOnMission(Object objectToSendOnMission)
    {
        if (objectToSendOnMission is Object obj)
        {
            objectToSendOnMission.ClearOwnerInventory();
            RemoveObject(objectToSendOnMission);
            OnObjectSend?.Invoke(inventoryOfType, objectToSendOnMission);
            OnInventoryChange?.Invoke();
        }        
    }

    public int GetCapacity()
    {
        return capacity;
    }
}
