using UnityEngine;
using System;

public class InstantSalvageHandler : MonoBehaviour
{
    public InstantSalvage instantSalvageSO;
    private bool isActive = false;

    public static Action OnSalvageInstantly;

    private void OnEnable()
    {
        InstantSalvage.OnInstantSalvageActivated += ActivateInstantSalvage;
        SalvageTable.OnSalvageStart += InstantSalvageRoll;
    }

    private void OnDisable()
    {
        InstantSalvage.OnInstantSalvageActivated -= ActivateInstantSalvage;
        SalvageTable.OnSalvageStart -= InstantSalvageRoll;
    }

    private void ActivateInstantSalvage()
    {
        isActive = true;
    }

    private void InstantSalvageRoll()
    {
        if (!isActive) return;

        if (UnityEngine.Random.Range(0, 100) < instantSalvageSO.instantSalvageChance)
        {
            OnSalvageInstantly?.Invoke();            
        }
    }
}