using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class SalvageTable : Bench
{
    [SerializeField] private float salvagingInterval; // tick of looting, how often can player get salvage from loot
    [SerializeField] private int salvagingValue; // how much salvage is looted per tick

    private Coroutine lootingCoroutine;

    private bool isEnabled = true;

    public static Action OnTutorialSparePartsPlaced;
    public static Action OnTutorialSparePartsSalvaged;

    public static Action OnSalvage;

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

    private void AssignCurrentObject(Object obj, Inventory myInventory)
    {
        if (myInventory == inventory)
        {
            currentObject = obj;
        }        
    }

    public void EnableSalvageTable()
    {
        isEnabled = true;
    }

    public void DisableSalvageTable()
    {
        isEnabled = false;
    }

    public override bool IsInteractionPossible()
    {
        return isEnabled;
    }

    IEnumerator DoSalvage()
    {        
        if (currentObject == null)
        {
            yield break;
        }

        while (true)
        {
            yield return new WaitForSeconds(salvagingInterval); // player must hold for this long

            ResourceController.Instance.ChangeSparePartsAmount(salvagingValue); // reward
            OnSalvage?.Invoke();

            if (currentObject.DamageObject(2)) // damage, check if destroyed
            {
                currentObject.DestroyObject();

                if (!TutorialController.Instance.skipTutorial)
                {
                    if (!TutorialController.Instance.sparePartsSalvaged)
                    {
                        OnTutorialSparePartsSalvaged?.Invoke();
                    }
                }
                break;
            }
        }
    }

    public override void StartInteractionPrimary()
    {
        base.StartInteractionPrimary();

        if (!TutorialController.Instance.skipTutorial)
        {
            if (!TutorialController.Instance.sparePartsPlacedSalvage)
            {
                OnTutorialSparePartsPlaced?.Invoke();
            }
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
