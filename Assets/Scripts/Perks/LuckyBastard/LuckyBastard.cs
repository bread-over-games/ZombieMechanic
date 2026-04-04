using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Perks/LuckyBastard")]
public class LuckyBastard : Perk
{
    public int noDurabilityLossChance;

    public static Action OnLuckyBastardActivated;

    public override void ActivatePerk()
    {
        base.ActivatePerk();
        OnLuckyBastardActivated?.Invoke();
    }
}