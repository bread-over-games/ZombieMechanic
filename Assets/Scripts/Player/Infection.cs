using UnityEngine;
using System.Collections;
using System;

public class Infection : MonoBehaviour
{
    private float currentInfectionLevel;

    [SerializeField] private float infectionInterval;
    [SerializeField] private float maxInfectionLevel;

    public static Action<float> OnInfectionLevelChange;
    public static Action OnInfectionReachedMaxLevel;

    private Coroutine currentInfectionCoroutine;

    private void OnEnable()
    {
        MedicalCabinet.OnAntibioticsUsed += DecreaseInfection;
        TutorialController.OnTutorialEnd += StartInfection;
        InfectionPauseHandler.OnInfectionPause += StopInfection;
        InfectionPauseHandler.OnInfectionPause += ResetInfectionlevel;
        InfectionPauseHandler.OnInfectionResume += StartInfection;
    }

    private void OnDisable()
    {
        MedicalCabinet.OnAntibioticsUsed -= DecreaseInfection;
        TutorialController.OnTutorialEnd -= StartInfection;
        InfectionPauseHandler.OnInfectionPause -= StopInfection;
        InfectionPauseHandler.OnInfectionPause -= ResetInfectionlevel;
        InfectionPauseHandler.OnInfectionResume -= StartInfection;
    }

    private void StartInfection()
    {
        currentInfectionCoroutine = StartCoroutine(IncreaseInfection());    
    }

    private void StopInfection()
    {
        StopCoroutine(currentInfectionCoroutine);
    }

    private void ResetInfectionlevel()
    {
        currentInfectionLevel = 0f;
        OnInfectionLevelChange?.Invoke(currentInfectionLevel);
    }

    private IEnumerator IncreaseInfection()
    {
        while (true)
        {
            yield return new WaitForSeconds(infectionInterval);
            currentInfectionLevel++;
            OnInfectionLevelChange?.Invoke(currentInfectionLevel);            

            if (currentInfectionLevel >= maxInfectionLevel)
            {                
                OnInfectionReachedMaxLevel?.Invoke();                
                break;
            }
        }
    }
    
    private void DecreaseInfection()
    {
        currentInfectionLevel -= 30; // replace with antibiotics strength

        if (currentInfectionLevel <= 0)
        {
            currentInfectionLevel = 0;  
        }

        OnInfectionLevelChange?.Invoke(currentInfectionLevel);
    }
}
