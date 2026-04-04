using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName = "Perks/InfectionPause")]
public class InfectionPause : Perk
{
    public float infectionPauseDuration;

    public static Action OnInfectioanPauseActivated;        

    public override void ActivatePerk()
    {
        base.ActivatePerk();
        OnInfectioanPauseActivated?.Invoke();
    }
}
