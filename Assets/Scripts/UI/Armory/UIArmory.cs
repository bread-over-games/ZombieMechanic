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

    private void OnEnable()
    {
        PlayerInteraction.OnInteractableApproached += ShowArmoryWindow;
        PlayerInteraction.OnInteractableLeft += HideArmoryWindow;
        MissionController.OnMissionStarted += ChangeSubWindow;
        MissionController.OnMissionCompleted += ChangeSubWindow;
    }

    private void OnDisable()
    {
        PlayerInteraction.OnInteractableApproached -= ShowArmoryWindow;
        PlayerInteraction.OnInteractableLeft -= HideArmoryWindow;
        MissionController.OnMissionStarted -= ChangeSubWindow;
        MissionController.OnMissionCompleted -= ChangeSubWindow;
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
           
            ChangeSubWindow(null);
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
            EventSystem.current.SetSelectedGameObject(null);
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

    private void ChangeSubWindow(Mission mission)
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
}