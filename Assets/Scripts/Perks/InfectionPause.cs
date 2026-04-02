using UnityEngine;

[CreateAssetMenu(menuName = "Perks/InfectionPause")]
public class InfectionPause : Perk
{
    public override void ActivatePerk()
    {
        base.ActivatePerk();
        Debug.Log("Infection paused");
    }
}
