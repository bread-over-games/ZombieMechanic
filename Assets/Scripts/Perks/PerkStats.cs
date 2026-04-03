using UnityEngine;
/// <summary>
/// keeps track of all value modifiers from perks
/// other classes reads from here as they need
/// </summary>

public class PerkStats : MonoBehaviour
{
    public static PerkStats Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
