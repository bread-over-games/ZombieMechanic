/// This is the main source of weapons, salvage, medicine, etc. from outside world. 
/// Player does not work here, only sift thru scavenged loot
/// He unpacks salvage and sends it to storage
/// Or he sends weapons to workbenches to salvage, repair or modify

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LootTable : Bench, IInteractable
{    
    [SerializeField] private float lootingInterval; // tick of looting, how often can player get salvage from loot
    [SerializeField] private int lootingValue; // how much salvage is looted per tick

    private Scrap currentScrap;
    private Weapon currentWeapon;
    private Coroutine lootingCoroutine;


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

    public override void StartInteractionSecondary()
    {
        if (inventory.GetObjectList().Count == 0)
        {
            return;
        }

        switch (inventory.GetObjectList()[0])
        {
            case Weapon weapon:
                return;
            case Scrap scrap:
                currentScrap = scrap;
                lootingCoroutine = StartCoroutine(DoLoot());
                break;
        }
    }

    public override void EndInteractionSecondary()
    {
        if (lootingCoroutine != null)
        {
            StopCoroutine(lootingCoroutine);
            lootingCoroutine = null;
        }
    }
}
