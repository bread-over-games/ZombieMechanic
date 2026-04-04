using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Perks/SalvageMastery")]
public class SalvageMastery : Perk
{
    public float extraSalvageChance;
    public int extraSalvageAmount;

    public static Action OnSalvageMasteryActivated;        

    public override void ActivatePerk()
    {
        base.ActivatePerk();
        OnSalvageMasteryActivated?.Invoke();        
    }
}
