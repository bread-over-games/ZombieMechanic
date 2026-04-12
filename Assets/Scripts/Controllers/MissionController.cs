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

    [Header("Mission loot quality outcome")]
    public float minimalLootQuality;

    public static Action<Armory> OnMissionStarting;
    public static Action<Mission> OnMissionStarted;
    public static Action<Mission> OnMissionCompleted;

    private Weapon preparingMissionWeapon;
    private Backpack preparingMissionBackpack;
    private Armor preparingMissionArmor;
    private Inventory preparingMissionInventory;
    private Armory preparingMissionArmory;
    private Mission.MissionType preparingMissionType;

    public bool isSelectingMissionType = false;

    private void OnEnable()
    {
        Armory.OnMissionGearSelected += SelectingMissionType;
        UIMissionSelect.OnCurrentMissionTypeSlotSelected += ConfirmMissionType;
    }

    private void OnDisable()
    {
        Armory.OnMissionGearSelected -= SelectingMissionType;
        UIMissionSelect.OnCurrentMissionTypeSlotSelected -= ConfirmMissionType;
    }


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

    private void SelectingMissionType()
    {
        isSelectingMissionType = true;
    }

    private void ResolveMission(Mission mission, int index)
    {
        OnMissionCompleted?.Invoke(mission);
        activeMissions.RemoveAt(index); // deletes mission when it's done
    }

    public void ConfirmMissionGear(Weapon weaponInArmory, Backpack backpackInArmory, Armor armorInArmory, Inventory missionInventory, Armory missionArmory)
    {
        preparingMissionWeapon = weaponInArmory;
        preparingMissionBackpack = backpackInArmory;
        preparingMissionArmor = armorInArmory;
        preparingMissionInventory = missionInventory;
        preparingMissionArmory = missionArmory;
    }

    public void ConfirmMissionType(Mission.MissionType missionType)
    {
        preparingMissionType = missionType;
        SendMission();
    }

    public void SendMission()
    {
        OnMissionStarting?.Invoke(preparingMissionArmory);
        Mission mission = new Mission(preparingMissionWeapon, preparingMissionBackpack, preparingMissionArmor, preparingMissionInventory, preparingMissionArmory, preparingMissionType);

        activeMissions.Add(mission);
        OnMissionStarted?.Invoke(mission);

        preparingMissionWeapon = null;
        preparingMissionBackpack = null;
        preparingMissionArmor = null;
        preparingMissionInventory = null;
        preparingMissionArmory = null;
        preparingMissionType = default;
        isSelectingMissionType = false;
    }        
}
