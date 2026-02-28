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
    private int salvageAmount = 100;
    [SerializeField] private Inventory inventory;
    [SerializeField] private string interactableName;

    [SerializeField] private float lootingInterval; // tick of looting, how often can player get salvage from loot
    [SerializeField] private int lootingValue; // how much salvage is looted per tick

    private Coroutine lootingCoroutine;

    public void ChangeSalvage(int amount)
    {
        salvageAmount += amount;

        if (salvageAmount <= 0)
        {
            salvageAmount = 0;
            EndInteractionPrimary();
        }
    }

    public void StartInteractionPrimary()
    {
        lootingCoroutine = StartCoroutine(DoLoot());
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
        while (salvageAmount > 0)
        {
            yield return new WaitForSeconds(lootingInterval);
            ChangeSalvage(-lootingValue);
            ResourceController.Instance.ChangeSalvageAmount(lootingValue);
        }
    }

    public bool IsInteractionPossible()
    {     
        if (salvageAmount > 0 || inventory.GetWeaponList()[0] != null)
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
        inventory.SendWeapon(InventoriesController.Instance.workbenchInventory);
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
