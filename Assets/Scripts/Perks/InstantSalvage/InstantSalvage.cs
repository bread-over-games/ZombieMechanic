using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Perks/InstantSalvage")]
public class InstantSalvage : Perk
{
    public int instantSalvageChance;

    public static Action OnInstantSalvageActivated;

    public override void ActivatePerk()
    {
        base.ActivatePerk();
        OnInstantSalvageActivated?.Invoke();
    }
}
