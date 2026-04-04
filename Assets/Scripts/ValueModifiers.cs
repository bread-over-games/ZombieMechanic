using UnityEngine;

public class ValueModifiers : MonoBehaviour
{
    [HideInInspector] public float zombieKillsModifier = 1;
    [HideInInspector] public float gearWearModifier = 1;

    public static ValueModifiers Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeZombieKillsModifier(float modifierChange)
    {
        zombieKillsModifier += modifierChange - 1;
    }

    public void ChangeGearWearModifier(float modifierChange)
    {
        gearWearModifier += modifierChange - 1;
    }
}
