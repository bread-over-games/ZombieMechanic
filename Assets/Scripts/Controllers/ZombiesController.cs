/// This will be meged into Sector controller later on


using UnityEngine;
using System;
using System.Collections.Generic;

public class ZombiesController : MonoBehaviour
{
    public static ZombiesController Instance { get; private set; }

    public static Action OnZombiesKilledChanged;
    public static Action OnAllZombiesKilled;

    public int zombiesKilledTotal;
    public int zombiesKillVictoryGoal; // set by designer

    public void OnEnable()
    {
        Mission.OnMissionResolved += AddKilledZombies;
    }

    public void OnDisable()
    {
        Mission.OnMissionResolved -= AddKilledZombies;
    }

    void Awake()
    {
        Instance = this;
    }

    private void AddKilledZombies(Mission mission, int zombKilled)
    {
        zombiesKilledTotal += zombKilled;
        OnZombiesKilledChanged?.Invoke();

        CheckZombiesKilledVictoryGoal();
    }

    private void CheckZombiesKilledVictoryGoal()
    {
        if (zombiesKilledTotal >= zombiesKillVictoryGoal)
        {
            Debug.Log("All zombies killed");
            OnAllZombiesKilled?.Invoke();
        }
    }
}
