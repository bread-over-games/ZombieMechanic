/// This is the main source of weapons, salvage, medicine, etc. from outside world. 
/// Player does not work here, only sift thru scavenged loot
/// He unpacks salvage and sends it to storage
/// Or he sends weapons to workbenches to salvage, repair or modify

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LootTable : MonoBehaviour, IInteractable
{    
    [SerializeField] private Transform itemPivot;    
    [SerializeField] private Inventory inventory;
    [SerializeField] private string interactableName;

    [SerializeField] private float lootingInterval; // tick of looting, how often can player get salvage from loot
    [SerializeField] private int lootingValue; // how much salvage is looted per tick

    private Scrap currentScrap;
    private Weapon currentWeapon;
    private Coroutine lootingCoroutine;

    public void StartInteractionPrimary()
    {
        if (inventory.GetObjectList().Count == 0)
        {
            return; 
        }
        switch (inventory.GetObjectList()[0])
        {
            case Weapon weapon:
                currentWeapon = weapon;
                inventory.SendObject(InventoriesController.Instance.workbenchInventory);                
                break;
            case Scrap scrap:
                currentScrap = scrap;
                lootingCoroutine = StartCoroutine(DoLoot());                
                break;
        }
        
    }

    public void EndInteractionPrimary()
    {
        if (lootingCoroutine != null)
        {
            StopCoroutine(lootingCoroutine);
            lootingCoroutine = null;
        }
    }

    IEnumerator DoLoot()
    {        
        while (currentScrap.salvageAmount > 0)
        {
            yield return new WaitForSeconds(lootingInterval);
            currentScrap.LootSalvage(lootingValue, inventory);
            ResourceController.Instance.ChangeSalvageAmount(lootingValue);

            if (!currentScrap.CanLootSalvage())
            {
                EndInteractionPrimary();
                break;
            }
        }
    }

    public bool IsInteractionPossible()
    {     
        if (inventory.GetObjectList().Count > 0)
        {            
            return true;
        }
        else
        {         
            return false;
        }
    }

    public void StartInteractionSecondary()
    {
        
    }

    public void EndInteractionSecondary()
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
