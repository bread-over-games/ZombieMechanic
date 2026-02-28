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

    private List<Weapon> weaponList = new List<Weapon>();

    public static Action<InventoryOfType> OnWeaponReceive; // whben Inventory receives wepaon
    public static Action OnWeaponSend; // when inventory sends weapon
    public static Action OnInventoryChange; // when something changes in inventory
    [SerializeField] private int capacity;

    public void RemoveWeapon(Weapon weapon)
    {
        weaponList.Remove(weapon);
    }

    public List<Weapon> GetWeaponList()
    {
        return weaponList;
    }

    public InventoryOfType GetInventoryOfType()
    {
        return inventoryOfType;
    }

    public void ReceiveWeapon(Weapon weapon)
    {
        weaponList.Add(weapon);
        weapon.LoadValues(weapon);        
        OnWeaponReceive?.Invoke(gameObject.GetComponent<Inventory>().GetInventoryOfType()); 
        OnInventoryChange?.Invoke();
    }

    //public void ReceiveObject(Object object)
    //{
    //    objectList.Add(object);
    //    switch (object)
    //    {
    //        case Weapon weapon:
    //            weapon.LoadValues(weapon);
    //            break;
    //        case Scrap scrap:
    //            scrap.LoadValues(scrap);
    //            break;
    //        case Medicine medicine:
    //            medicine.LoadValues(medicine);
    //            break;
    //    }

    //    OnObjectReceive?.Invoke(gameObject.GetComponent<Inventory>().GetInventoryOfType());
    //    OnInventoryChange?.Invoke();
    //}

    public void SendWeapon(Inventory target) // moves weapon from list to another list
    {
        if (weaponList.Count > 0)
        {
            target.ReceiveWeapon(weaponList[0]);

            RemoveWeapon(weaponList[0]);
            OnWeaponSend?.Invoke();
            OnInventoryChange?.Invoke();
        }
    }

    public void SetOutsideTimes() // sets times for outside - how long it should be outside and when it left
    {
        if (weaponList.Count > 0)
        {
            weaponList[0].timeAddedToOutside = Time.time;
            weaponList[0].timeToSpendOutside = UnityEngine.Random.Range(3, 7);
        }       
    }   
}
