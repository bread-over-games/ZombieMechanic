using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance { get; private set; }
    public bool isPaused = false;

    private void OnEnable()
    {
        XPCounter.OnLevelUp += PauseGame;
        PerkController.OnPerkSelected += ResumeGame;
        SectorController.OnAntibioticsDepleted += PauseGame;
        SectorController.OnAntibioticsRunningLow += PauseGame;
        Armory.OnMissionGearSelected += PauseGame;        
        UIMissionSelect.OnCurrentMissionTypeSlotSelected += _ => ResumeGame();
        Infection.OnInfectionReachedMaxLevel += PauseGame;
        UISectorInfo.OnMessageConfirmed += ResumeGame;
    }

    private void OnDisable()
    {
        XPCounter.OnLevelUp -= PauseGame;
        PerkController.OnPerkSelected -= ResumeGame;
        SectorController.OnAntibioticsDepleted -= PauseGame;
        SectorController.OnAntibioticsRunningLow -= PauseGame;
        Armory.OnMissionGearSelected -= PauseGame;
        UIMissionSelect.OnCurrentMissionTypeSlotSelected -= _ => ResumeGame();
        Infection.OnInfectionReachedMaxLevel -= PauseGame;
        UISectorInfo.OnMessageConfirmed -= ResumeGame;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
}
