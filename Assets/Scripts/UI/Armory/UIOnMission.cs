using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIOnMission : MonoBehaviour
{
    [SerializeField] private GameObject onMissionWindow;

    public void OpenWindow()
    {
        onMissionWindow.SetActive(true);   
    }

    public void CloseWindow()
    {
        onMissionWindow.SetActive(false);
    }
}
