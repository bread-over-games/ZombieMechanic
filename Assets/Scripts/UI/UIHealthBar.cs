using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Image infectionLevel;
    public GameObject healthBarUI;

    private void OnEnable()
    {
        Infection.OnInfectionLevelChange += UpdateInfectionLevel;
        MissionController.OnMissionCompleted += DisplayInfectionBar;
    }

    private void OnDisable()
    {
        Infection.OnInfectionLevelChange -= UpdateInfectionLevel;
        MissionController.OnMissionCompleted -= DisplayInfectionBar;
    }

    private void Awake()
    {
        healthBarUI.SetActive(false);   
    }

    private void DisplayInfectionBar(Mission mission)
    {
        healthBarUI.SetActive(true);
    }

    private void UpdateInfectionLevel(float currentInfectionLevel)
    {
        infectionLevel.fillAmount = currentInfectionLevel / 100;
    }
}
