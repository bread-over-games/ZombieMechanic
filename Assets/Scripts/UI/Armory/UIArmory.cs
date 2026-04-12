using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIArmory : MonoBehaviour
{
    public Inventory inventory;
    public Armory armory;
    [SerializeField] private GameObject armoryWindow;
    [SerializeField] private UIGearOverview gearOverviewUI;
    [SerializeField] private UIOnMission onMissionUI;
    [SerializeField] private UIMissionSelect missionSelectUI;

    private void OnEnable()
    {
        PlayerInteraction.OnInteractableApproached += ShowArmoryWindow;
        PlayerInteraction.OnInteractableLeft += HideArmoryWindow;
        MissionController.OnMissionStarted += ArmoryWindowMissionStateChange;
        MissionController.OnMissionCompleted += ArmoryWindowMissionStateChange;
        Armory.OnMissionGearSelected += OpenMissionTypeSelectWindow;
    }

    private void OnDisable()
    {
        PlayerInteraction.OnInteractableApproached -= ShowArmoryWindow;
        PlayerInteraction.OnInteractableLeft -= HideArmoryWindow;
        MissionController.OnMissionStarted -= ArmoryWindowMissionStateChange;
        MissionController.OnMissionCompleted -= ArmoryWindowMissionStateChange;
        Armory.OnMissionGearSelected -= OpenMissionTypeSelectWindow;
    }

    private void ShowArmoryWindow(IInteractable interactableType)
    {
        if (interactableType is IBench bench)
        {
            if (bench.GetBenchType() != Bench.BenchType.Armory)
            {
                return;
            }
            UIFocusStack.Push(armoryWindow);           
            ArmoryWindowMissionStateChange(null);
        }        
    }

    private void HideArmoryWindow(IInteractable interactableType)
    {
        if (interactableType is IBench bench)
        {
            if (bench.GetBenchType() != Bench.BenchType.Armory)
            {
                return;
            }            
            DropInventory();
            DropArmory();
            UIFocusStack.Pop();
        }     
    }

    public void SetInventory(Inventory currentInventory)
    {
        inventory = currentInventory;
    }

    public void SetArmory(Armory currentArmory)
    {
        armory = currentArmory;
    }

    private void DropInventory()
    {
        inventory = null;
    }

    private void DropArmory()
    {
        armory = null;
    }

    private void ArmoryWindowMissionStateChange(Mission mission)
    {
        if (armory == null) return;

        if (armory.isAvailableForMission)
        {
            gearOverviewUI.OpenWindow();
            onMissionUI.CloseWindow();  
        } else
        {
            gearOverviewUI.CloseWindow();
            onMissionUI.OpenWindow();
        }
    }

    private void OpenMissionTypeSelectWindow()
    {
        if (armory == null) return;

        gearOverviewUI.CloseWindow();
        missionSelectUI.OpenWindow();
    }
}