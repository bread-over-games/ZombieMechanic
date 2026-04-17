using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance { get; private set; }

    public static Action OnGamePaused;
    public static Action OnGameResumed;

    public bool isGamePaused = false;
    public bool isPauseBlocked = false; // this and all attached methods are jsut temporary solution once all UI states could be resolved

    private void OnEnable()
    {
        XPCounter.OnLevelUp += PauseGame;
        XPCounter.OnLevelUp += BlockPause;
        PerkController.OnPerkSelected += ResumeGame;
        PerkController.OnPerkSelected += UnblockPause;
        SectorController.OnAntibioticsDepleted += PauseGame;
        SectorController.OnAntibioticsDepleted += BlockPause;
        SectorController.OnAntibioticsRunningLow += PauseGame;
        SectorController.OnAntibioticsRunningLow += BlockPause;
        Armory.OnMissionGearSelected += PauseGame;
        Armory.OnMissionGearSelected += BlockPause;
        UIMissionSelect.OnCurrentMissionTypeSlotSelected += _ => ResumeGame();
        UIMissionSelect.OnCurrentMissionTypeSlotSelected += _ => UnblockPause();
        Infection.OnInfectionReachedMaxLevel += PauseGame;
        Infection.OnInfectionReachedMaxLevel += BlockPause;
        UISectorInfo.OnMessageConfirmed += ResumeGame;
        UISectorInfo.OnMessageConfirmed += UnblockPause;
        UIPauseMenu.OnResumeGameButtonSelected += ResumeGame;
        UIPauseMenu.OnResumeGameButtonSelected += UnblockPause;
    }

    private void OnDisable()
    {
        XPCounter.OnLevelUp -= PauseGame;
        XPCounter.OnLevelUp -= BlockPause;
        PerkController.OnPerkSelected -= ResumeGame;
        PerkController.OnPerkSelected -= UnblockPause;
        SectorController.OnAntibioticsDepleted -= PauseGame;
        SectorController.OnAntibioticsDepleted -= BlockPause;
        SectorController.OnAntibioticsRunningLow -= PauseGame;
        SectorController.OnAntibioticsRunningLow -= BlockPause;
        Armory.OnMissionGearSelected -= PauseGame;
        Armory.OnMissionGearSelected -= BlockPause;
        UIMissionSelect.OnCurrentMissionTypeSlotSelected -= _ => ResumeGame();
        UIMissionSelect.OnCurrentMissionTypeSlotSelected -= _ => UnblockPause();
        Infection.OnInfectionReachedMaxLevel -= PauseGame;
        Infection.OnInfectionReachedMaxLevel -= BlockPause;
        UISectorInfo.OnMessageConfirmed -= ResumeGame;
        UISectorInfo.OnMessageConfirmed -= UnblockPause;
        UIPauseMenu.OnResumeGameButtonSelected -= ResumeGame;
        UIPauseMenu.OnResumeGameButtonSelected -= UnblockPause;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.started) return;        
        if (!isGamePaused && !isPauseBlocked)
        {
            PauseGame();
            OnGamePaused?.Invoke(); 
        } 
        else if (isGamePaused && !isPauseBlocked)
        {
            ResumeGame();
            OnGameResumed?.Invoke();            
        }        
    }

    private void Awake()
    {
        Instance = this;
    }

    private void BlockPause()
    {
        isPauseBlocked = true;
    }

    private void UnblockPause()
    {
        isPauseBlocked = false;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }
}
