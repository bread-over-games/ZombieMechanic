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
        Outside
    }

    [SerializeField] private InventoryOfType inventoryOfType;

    private List<Object> objectList = new List<Object>();

    public static Action<InventoryOfType, Object> OnObjectReceive; // whben Inventory receives wepaon
    public static Action<InventoryOfType> OnObjectSend; // when inventory sends weapon
    public static Action OnInventoryChange; // when something changes in inventory
    [SerializeField] private int capacity;

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
        switch (obj)
        {
            case Weapon weapon:
                weapon.LoadValues(weapon);
                OnObjectReceive?.Invoke(inventoryOfType, weapon);
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
            RemoveObject(objectList[0]);
            OnObjectSend?.Invoke(inventoryOfType);

            target.ReceiveObject(objToSend);
            OnInventoryChange?.Invoke();
        } 
    }

    public void SetOutsideTimes() // sets times for outside - how long it should be outside and when it left
    {
        if (objectList.Count > 0)
        {
            if (objectList[0] is Weapon weapon)
            {
                weapon.timeAddedToOutside = Time.time;
                weapon.timeToSpendOutside = UnityEngine.Random.Range(3, 7);
            }            
        }       
    }   
}
