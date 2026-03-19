using UnityEngine;
using System;

public class MedicalCabinet : Bench, IInteractable
{
    public static Action OnAntibioticsUsed;

    public void Awake()
    {
        acceptedTypes.Add(typeof(Antibiotics));
    }

    private void OnEnable()
    {
        Inventory.OnObjectReceive += InsertAntibiotics;
    }

    private void OnDisable()
    {
        Inventory.OnObjectReceive -= InsertAntibiotics;
    }


    public override void StartInteractionSecondary()
    {
        UseAntibiotics();
    }

    private void InsertAntibiotics(Object obj, Inventory myInventory)
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

    private void UseAntibiotics()
    {
        if (ResourceController.Instance.GetAntibioticsAmount() <= 0)
        {
            ResourceController.Instance.ChangeAntibioticsAmount(0);
        } 
        else
        {
            ResourceController.Instance.ChangeAntibioticsAmount(-1);
            OnAntibioticsUsed?.Invoke();
        }        
    }
}
