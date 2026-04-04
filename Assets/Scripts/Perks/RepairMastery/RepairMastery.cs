using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Perks/RepairMastery")]
public class RepairMastery : Perk
{
    public float freeRepairChance;
    public int sparePartsReturnAmount;

    public static Action OnRepairMasteryActivated;

    public override void ActivatePerk()
    {
        base.ActivatePerk();
        OnRepairMasteryActivated?.Invoke();
    }
}