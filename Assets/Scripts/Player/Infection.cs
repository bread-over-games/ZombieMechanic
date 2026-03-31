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

    private bool isFirstMissionComplete = false;

    private void OnEnable()
    {
        MedicalCabinet.OnAntibioticsUsed += DecreaseInfection;
        TutorialController.OnTutorialEnd += StartInfection;
    }

    private void OnDisable()
    {
        MedicalCabinet.OnAntibioticsUsed -= DecreaseInfection;
        TutorialController.OnTutorialEnd -= StartInfection;
    }

    private void StartInfection()
    {
        StartCoroutine(IncreaseInfection());
        isFirstMissionComplete = true;      
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
                Debug.Log("Dead from infection");
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
