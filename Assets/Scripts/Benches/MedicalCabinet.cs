using UnityEngine;
using System;

public class MedicalCabinet : Bench, IInteractable
{

    public static Action OnAntibioticsUsed;

    public override void StartInteractionPrimary()
    {
        InsertAntibiotics();
    }

    public override void StartInteractionSecondary()
    {
        // takes medicine from cabinet and use it if any is in inventory
        UseAntibiotics();
    }

    private void InsertAntibiotics()
    {

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
