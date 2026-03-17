using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Image infectionLevel;

    private void OnEnable()
    {
        Infection.OnInfectionLevelChange += UpdateInfectionLevel;
    }

    private void OnDisable()
    {
        Infection.OnInfectionLevelChange -= UpdateInfectionLevel;
    }

    private void UpdateInfectionLevel(float currentInfectionLevel)
    {
        infectionLevel.fillAmount = currentInfectionLevel / 100;
    }
}
