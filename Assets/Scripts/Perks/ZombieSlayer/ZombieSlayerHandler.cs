using UnityEngine;

public class ZombieSlayerHandler : MonoBehaviour
{
    public ZombieSlayer zombieSlayerSO;
    private bool isActive = false;

    private void OnEnable()
    {
        ZombieSlayer.OnZombieSlayerActivated += ActivateZombieSlayer;
    }

    private void OnDisable()
    {
        ZombieSlayer.OnZombieSlayerActivated -= ActivateZombieSlayer;
    }

    private void ActivateZombieSlayer()
    {
        isActive = true;
        ValueModifiers.Instance.ChangeZombieKillsModifier(zombieSlayerSO.zombieKillsMultiplier);
        ValueModifiers.Instance.ChangeGearWearModifier(zombieSlayerSO.durabilityLossMultiplier);
    }
}
