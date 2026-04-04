using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Perks/ZombieSlayer")]
public class ZombieSlayer : Perk
{
    public float zombieKillsMultiplier;
    public float durabilityLossMultiplier;

    public static Action OnZombieSlayerActivated;

    public override void ActivatePerk()
    {
        base.ActivatePerk();
        OnZombieSlayerActivated?.Invoke();
    }
}