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
    public static Action OnTutorialSparePartsPicked;
    public static Action OnTutorialBaseballBatPicked;

    private void OnEnable()
    {
        Inventory.OnObjectReceive += InsertAntibiotics;
    }

    private void OnDisable()
    {
        Inventory.OnObjectReceive -= InsertAntibiotics;
    }

    public override void StartInteractionPrimary()
    {
        base.StartInteractionPrimary();

        if (!TutorialController.Instance.skipTutorial)
        {
            if (!TutorialController.Instance.sparePartsPicked)
            {
                OnTutorialSparePartsPicked?.Invoke();
                return;
            }

            if (!TutorialController.Instance.baseballBatPicked)
            {
                OnTutorialBaseballBatPicked?.Invoke();
            }
        }
    }

    private void InsertAntibiotics(Object obj, Inventory myInventory) // when antibiotics are found on mission they are automatically added to medical cabinet
    {
        if (myInventory != inventory)
        {
            return;
        }

        if (obj is Antibiotics antibiotics)
        {
            ResourceController.Instance.ChangeAntibioticsAmount(antibiotics.currentDurability);
            antibiotics.DestroyObject();
        }
    }
}
