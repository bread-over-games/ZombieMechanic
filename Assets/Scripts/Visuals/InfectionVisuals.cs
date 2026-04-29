using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class InfectionVisuals : MonoBehaviour
{
    [SerializeField] private Volume infectionVolume;

    private void OnEnable()
    {
        Infection.OnInfectionLevelChange += ChangeInfectionEffectIntensity;       
    }

    private void OnDisable()
    {
        Infection.OnInfectionLevelChange -= ChangeInfectionEffectIntensity;
    }

    private void ChangeInfectionEffectIntensity(float currentInfectionLevel)
    {
        infectionVolume.weight = Mathf.InverseLerp(70f, 90f, currentInfectionLevel);
    }
}
