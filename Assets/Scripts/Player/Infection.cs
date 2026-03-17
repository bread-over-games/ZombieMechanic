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

    private void Start()
    {
        StartCoroutine(IncreaseInfection());
    }

    private IEnumerator IncreaseInfection()
    {
        while (true)
        {
            yield return new WaitForSeconds(infectionInterval);
            currentInfectionLevel++;
            OnInfectionLevelChange?.Invoke(currentInfectionLevel);
            Debug.Log($"Infection level: {currentInfectionLevel}");

            if (currentInfectionLevel >= maxInfectionLevel )
            {
                Debug.Log("Dead from infection");
                OnInfectionReachedMaxLevel?.Invoke();
                break;
            }
        }
    }    
}
