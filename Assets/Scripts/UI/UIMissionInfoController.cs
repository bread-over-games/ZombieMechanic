using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIMissionInfoController : MonoBehaviour
{
    public UIMissionInfoPanel missionPanelPrefab;
    public Transform missionPanelsParent;

    private List<UIMissionInfoPanel> activeMissionPanels = new List<UIMissionInfoPanel>();

    private void OnEnable()
    {
        MissionController.OnMissionStarted += SpawnMissionPanel;
        MissionController.OnMissionCompleted += DeleteMissionPanel;
    }

    private void OnDisable()
    {
        MissionController.OnMissionStarted -= SpawnMissionPanel;
        MissionController.OnMissionCompleted -= DeleteMissionPanel;
    }

    private void SpawnMissionPanel(Mission mission)
    {
        UIMissionInfoPanel missionPanel = Instantiate(missionPanelPrefab, missionPanelsParent);
        missionPanel.Initialize(mission);
        activeMissionPanels.Add(missionPanel);
    }

    private void DeleteMissionPanel(Mission mission)
    {
        foreach (UIMissionInfoPanel infoPanel in activeMissionPanels)
        {
            if (infoPanel.currentMission == mission)
            {
                activeMissionPanels.Remove(infoPanel);
                return;
            }
        }
    }
}
