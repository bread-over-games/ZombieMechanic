using UnityEngine;
using System;
using System.Collections.Generic;

public class XPCounter : MonoBehaviour
{
    public static XPCounter Instance;

    public int zombieKillXP;
    public int repairXP;
    public int salvageXP;
    public int upgradeXP;

    [HideInInspector] public int currentXP = 0;
    [HideInInspector] public int currentLvl = 0;
    private int maxLevel = 5;

    private bool maxLevelReached = false;

    public List<int> xpRequiredForNextLevel = new List<int>();

    public static Action OnLevelUp;
    public static Action OnXPChange;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Workbench.OnRepairTick += AddRepairXP;
        SalvageTable.OnSalvageTick += AddSalvageXP;
        ObjectDeconstruction.OnDeconstructionTick += AddSalvageXP;
        BenchConstruction.OnConstructionTick += AddRepairXP;
        UIFlyoutVisual.OnFlyoutReachedDestination += AddZombieKillXP;
    }

    private void OnDisable()
    {
        Workbench.OnRepairTick -= AddRepairXP;
        SalvageTable.OnSalvageTick -= AddSalvageXP;
        ObjectDeconstruction.OnDeconstructionTick -= AddSalvageXP;
        BenchConstruction.OnConstructionTick -= AddRepairXP;
        UIFlyoutVisual.OnFlyoutReachedDestination -= AddZombieKillXP;
    }

    private void AddRepairXP()
    {
        if (maxLevelReached) return;
        currentXP += repairXP;
        CheckLevel();
    }

    public void AddZombieKillXP(int amount, UIFlyoutVisual.FlyoutTypes flyoutType)
    {
        if (maxLevelReached) return;
        if (flyoutType != UIFlyoutVisual.FlyoutTypes.XP) return;
        currentXP += amount;
        CheckLevel();
    }    
    private void AddSalvageXP()
    {
        if (maxLevelReached) return;
        currentXP += salvageXP;
        CheckLevel();
    }

    private void AddUpgradeXP()
    {
        if (maxLevelReached) return;
        currentXP += upgradeXP;
        CheckLevel();
    }

    private void CheckLevel()
    {
        OnXPChange?.Invoke();
        if (currentXP > xpRequiredForNextLevel[currentLvl])
        {
            currentXP -= xpRequiredForNextLevel[currentLvl];
            currentLvl += 1;            
            OnLevelUp?.Invoke();

            if (currentLvl == maxLevel)
            {
                maxLevelReached = true;
            }
        }
    }
}
