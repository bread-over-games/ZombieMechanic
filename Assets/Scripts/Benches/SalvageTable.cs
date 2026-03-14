using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class SalvageTable : Bench
{
    [SerializeField] private float salvagingInterval; // tick of looting, how often can player get salvage from loot
    [SerializeField] private int salvagingValue; // how much salvage is looted per tick

    private Scrap currentScrap;
    private Coroutine lootingCoroutine;

    private void OnEnable()
    {
        Inventory.OnObjectReceive += AssignCurrentObject;
    }

    private void OnDisable()
    {
        Inventory.OnObjectReceive -= AssignCurrentObject;
    }

    public void Awake()
    {
        acceptedTypes.Add(typeof(Weapon));
        acceptedTypes.Add(typeof(Backpack));
        acceptedTypes.Add(typeof(Armor));
        acceptedTypes.Add(typeof(Scrap));
    }

    private void AssignCurrentObject(Inventory.InventoryOfType invOfType, Object obj)
    {
        if (invOfType == Inventory.InventoryOfType.SalvageTable)
        {
            currentObject = obj;
        }
    }

    IEnumerator DoSalvage()
    {
        switch (inventory.GetObjectList()[0])
        {
            default:

                while (!currentObject.DamageObject(2))
                {
                    yield return new WaitForSeconds(salvagingInterval);
                    ResourceController.Instance.ChangeSalvageAmount(salvagingValue);
                }

                currentObject.DestroyObject();

                break;
        }        
    }

    public override void StartInteractionSecondary()
    {
        if (inventory.GetObjectList().Count == 0)
        {
            return;
        }

        lootingCoroutine = StartCoroutine(DoSalvage());
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
