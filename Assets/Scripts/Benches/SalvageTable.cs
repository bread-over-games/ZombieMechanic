using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class SalvageTable : Bench
{
    [SerializeField] private float salvagingInterval; // tick of looting, how often can player get salvage from loot
    private float defaultSalvagingInterval;
    [SerializeField] private int salvagingValue; // how much salvage is looted per tick

    private Coroutine salvagingCoroutine;

    private bool isEnabled = true;

    public static Action OnTutorialSparePartsPlaced;
    public static Action OnTutorialSparePartsSalvaged;

    public static Action OnSalvageStart;
    public static Action OnSalvageTick;
    public static Action OnSalvageStop;

    private void OnEnable()
    {
        Inventory.OnObjectReceive += AssignCurrentObject;
        InstantSalvageHandler.OnSalvageInstantly += InstantSalvageInterval;
    }

    private void OnDisable()
    {
        Inventory.OnObjectReceive -= AssignCurrentObject;
        InstantSalvageHandler.OnSalvageInstantly -= InstantSalvageInterval;
    }

    public void Awake()
    {
        acceptedTypes.Add(typeof(Weapon));
        acceptedTypes.Add(typeof(Backpack));
        acceptedTypes.Add(typeof(Armor));
        acceptedTypes.Add(typeof(Scrap));

        defaultSalvagingInterval = salvagingInterval;
    }

    private void AssignCurrentObject(Object obj, Inventory myInventory)
    {
        if (myInventory == inventory)
        {
            currentObject = obj;
        }        
    }

    private void InstantSalvageInterval()
    {
        salvagingInterval = 0;
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
            OnSalvageTick?.Invoke();

            if (currentObject.DamageObject(2)) // damage, check if destroyed
            {
                currentObject.DestroyObject();
                OnSalvageStop?.Invoke();

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

        salvagingInterval = defaultSalvagingInterval;
        OnSalvageStart?.Invoke();

        salvagingCoroutine = StartCoroutine(DoSalvage());
    }

    public override void EndInteractionSecondary()
    {
        if (salvagingCoroutine != null)
        {
            StopCoroutine(salvagingCoroutine);
            OnSalvageStop?.Invoke();
            salvagingCoroutine = null;
        }
    }
}
