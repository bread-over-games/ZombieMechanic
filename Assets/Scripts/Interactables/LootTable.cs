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
    [HideInInspector] public bool isLooting = false;
    [SerializeField] private Transform itemPivot;
    private WeaponWorld currentWeapon;
    private int salvageAmount = 100;
    [SerializeField] private Inventory inventory;


    private void Start()
    {
        inventory.AddWeapon();
    }

    public void Update()
    {
        if (isLooting)
        {
            DoLoot();
        }
    }

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
        isLooting = true;
    }

    public void EndInteractionPrimary()
    {
        isLooting = false;
    }

    private void DoLoot()
    {
        if (salvageAmount <= 0)
        {
            salvageAmount = 0;
            return;
        }

        Debug.Log("Looting...");
        ChangeSalvage(-1);
        ResourceController.Instance.ChangeSalvageAmount(1);
    }

    public bool IsInteractionPossible()
    {     
        if (salvageAmount > 0)
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
}
