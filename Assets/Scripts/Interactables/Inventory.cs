using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    private List<Weapon> weaponList = new List<Weapon>();

    [SerializeField] private UIInventory uiInventory;

    public static Action OnWeaponReceive;
    public static Action OnWeaponSend;


    public void AddWeapon() // when new weapon is created
    {
        Weapon newWeapon = (new Weapon { weaponType = Weapon.WeaponType.BaseballBat });     
        weaponList.Add(newWeapon);
        newWeapon.SetValues();
        OnWeaponReceive?.Invoke();
    }

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

    public void SendWeapon(Transform target) // moves weapon from list to another list
    {
        if (weaponList.Count > 0)
        {
            target.GetComponent<Inventory>().GetInventory().ReceiveWeapon(weaponList[0]);

            RemoveWeapon(weaponList[0]);
            OnWeaponSend?.Invoke();
        }
    }
}
