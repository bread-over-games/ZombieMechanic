using UnityEngine;
using System;

public class LuckyBastardHandler : MonoBehaviour
{
    public LuckyBastard luckyBastardSO;
    private bool isActive = false;

    public static Action OnNoWeaponWear;
    public static Action OnNoArmorWear;
    public static Action OnNoBackpackWear; 
    public static Action OnLuckyBastardProc;

    private void OnEnable()
    {
        LuckyBastard.OnLuckyBastardActivated += ActivateLuckyBastard;
        MissionController.OnMissionStarting += NoDurabilityWearRoll;
        MissionCalculator.Initialize();
    }

    private void OnDisable()
    {
        LuckyBastard.OnLuckyBastardActivated -= ActivateLuckyBastard;
        MissionController.OnMissionStarting -= NoDurabilityWearRoll;
        MissionCalculator.Cleanup();
    }

    private void ActivateLuckyBastard()
    {
        isActive = true;
    }

    private void NoDurabilityWearRoll(Armory armory)
    {
        NoWeaponWearRoll();
        NoArmorWearRoll();
        NoBackpackWearRoll();
    }

    private void NoWeaponWearRoll()
    {
        if (!isActive) return;

        if (UnityEngine.Random.Range(0,100) < luckyBastardSO.noDurabilityLossChance)
        {
            OnNoWeaponWear?.Invoke();
            OnLuckyBastardProc?.Invoke();   
        }
    }

    private void NoArmorWearRoll()
    {
        if (!isActive) return;

        if (UnityEngine.Random.Range(0, 100) < luckyBastardSO.noDurabilityLossChance)
        {
            OnNoArmorWear?.Invoke();
            OnLuckyBastardProc?.Invoke();
        }
    }

    private void NoBackpackWearRoll()
    {
        if (!isActive) return;

        if (UnityEngine.Random.Range(0, 100) < luckyBastardSO.noDurabilityLossChance)
        {
            OnNoBackpackWear?.Invoke();
            OnLuckyBastardProc?.Invoke();
        }
    }
}
