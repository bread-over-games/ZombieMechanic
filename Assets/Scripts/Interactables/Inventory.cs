using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    private List<Weapon> weaponList = new List<Weapon>();

    public static Action OnWeaponReceive;
    public static Action OnWeaponSend;
    [SerializeField] private int capacity;

    public void RemoveWeapon(Weapon weapon)
    {
        weaponList.Remove(weapon);
    }

    public List<Weapon> GetWeaponList()
    {
        return weaponList;
    }

    public Inventory GetInventory()
    {
        return this;
    }

    public void ReceiveWeapon(Weapon weapon)
    {
        weaponList.Add(weapon);
        weapon.LoadValues(weapon);
        OnWeaponReceive?.Invoke();
    }

    public void SendWeapon(Inventory target) // moves weapon from list to another list
    {
        if (weaponList.Count > 0)
        {
            target.GetInventory().ReceiveWeapon(weaponList[0]);

            RemoveWeapon(weaponList[0]);
            OnWeaponSend?.Invoke();
        }
    }

    public void SetOutsideTimes() // sets times for outside - how long it should be outside and when it left
    {
        weaponList[0].timeAddedToOutside = Time.time;
        weaponList[0].timeToSpendOutside = UnityEngine.Random.Range(3, 7);
    }    
}
