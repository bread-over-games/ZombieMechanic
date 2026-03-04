using UnityEngine;
using System;
using System.Collections.Generic;

public class MissionController : MonoBehaviour
{
    public static MissionController Instance { get; private set; }

    private List<Mission> activeMissions = new List<Mission>();

    public static Action<Mission> OnMissionStarted;
    public static Action<Mission> OnMissionCompleted;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Tick all active missions
        // Iterate backwards so we can safely remove during the loop
        for (int i = activeMissions.Count - 1; i >= 0; i--)
        {
            Mission mission = activeMissions[i];
            mission.Tick(Time.deltaTime);

            if (mission.isComplete)
            {
                ResolveMission(mission, i);
            }                
        }
    }

    private void ResolveMission(Mission mission, int index)
    {
        OnMissionCompleted?.Invoke(mission);
        activeMissions.RemoveAt(index); // deletes mission when it's done
        // 
    }

    public void SendMission (Weapon weaponInArmory, Armory survivorArmory)
    {
        Mission mission = new Mission(weaponInArmory);

        activeMissions.Add(mission); 
    }
}
