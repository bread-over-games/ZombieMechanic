using UnityEngine;
using System;
using System.Collections.Generic;

public class MissionController : MonoBehaviour
{
    public static MissionController Instance { get; private set; }

    [SerializeReference] private List<Mission> activeMissions = new List<Mission>();

    [Header("Mission duration settings")]
    public float missionMaximumLength; // placeholder, will be later calculated based on strongest equip. This value is used in formula to calculate loot durability
    public float missionLengthMultiplier;
    public float missionLengthWeaponWeight;
    public float missionLengthArmorWeight;
    public float missionLengthBackpackWeight;

    [Header("Mission loadout wear")]
    public float loadoutWearWeaponWeight;
    public float loadoutWearArmorWeight;
    public float loadoutWearBackpackWeight;

    [Header("Mission loot quality outcome")]
    public float minimalLootQuality;

    public static Action<Mission> OnMissionStarted;
    public static Action<Mission> OnMissionCompleted;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {        
        for (int i = activeMissions.Count - 1; i >= 0; i--) // tick all active missions
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
    }

    public void SendMission (Weapon weaponInArmory, Backpack backpackInArmory, Armor armorInArmory, Inventory missionInventory)
    {
        Mission mission = new Mission(weaponInArmory, backpackInArmory, armorInArmory, missionInventory);

        activeMissions.Add(mission); 
    }
}
