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
    public Weapon currentWeapon;
    private Coroutine lootingCoroutine;

    public void Awake()
    {
        acceptedTypes.Add(typeof(Weapon));
        acceptedTypes.Add(typeof(Scrap));
    }

    IEnumerator DoSalvage()
    {
        switch (inventory.GetObjectList()[0])
        {
            case Weapon weapon:
                currentWeapon = weapon;

                while (!weapon.DamageWeapon(inventory, 2))
                {
                    yield return new WaitForSeconds(salvagingInterval);
                    ResourceController.Instance.ChangeSalvageAmount(salvagingValue);
                }

                break;
            case Scrap scrap:
                currentScrap = scrap;

                while (currentScrap.salvageAmount > 0)
                {
                    yield return new WaitForSeconds(salvagingInterval);
                    currentScrap.LootSalvage(salvagingValue, inventory);
                    ResourceController.Instance.ChangeSalvageAmount(salvagingValue);

                    if (!currentScrap.CanLootSalvage())
                    {
                        EndInteractionSecondary();
                        break;
                    }
                }

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
