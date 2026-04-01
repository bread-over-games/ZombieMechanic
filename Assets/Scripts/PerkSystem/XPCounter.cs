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

    private int currentXP = 0;
    private int currentLvl = 0;
    private int maxLevel = 12;

    private bool maxLevelReached = false;

    public List<int> xpRequiredForNextLevel = new List<int>();

    public static Action OnLevelUp;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Workbench.OnRepair += AddRepairXP;
        SalvageTable.OnSalvage += AddSalvageXP; 
    }

    private void OnDisable()
    {
        Workbench.OnRepair -= AddRepairXP;
        SalvageTable.OnSalvage -= AddSalvageXP;
    }

    private void AddRepairXP()
    {
        if (maxLevelReached) return;
        currentXP += repairXP;
        CheckLevel();
    }

    public void AddZombieKillXP(int amount)
    {
        if (maxLevelReached) return;
        currentXP += zombieKillXP * amount;
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
        Debug.Log(currentXP);
        if (currentXP >= xpRequiredForNextLevel[currentLvl])
        {            
            currentLvl += 1;
            currentXP = 0;
            OnLevelUp?.Invoke();
            Debug.Log(currentLvl);

            if (currentLvl == maxLevel)
            {
                maxLevelReached = true;
            }
        }
    }
}
