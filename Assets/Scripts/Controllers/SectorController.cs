/// Controls current sector the player has selected from Map (handled from Map Controller)

using UnityEngine;
using System;

public class SectorController : MonoBehaviour
{
    public static SectorController Instance;
    public static Action OnAntibioticsDepleted;
    public static Action OnAntibioticsRunningLow;

    public int zombiesLeft;
    public int antibioticsLeft;

    public bool antibioticsDepleted = false;
    private bool antibioticsLowWarned = false;

    private void OnEnable()
    {
        ObjectGenerator.OnAntibioticsGenerated += ChangeAntibioticsValue;
    }

    private void OnDisable()
    {
        ObjectGenerator.OnAntibioticsGenerated -= ChangeAntibioticsValue;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void NewSectorInitialize()
    {
        antibioticsLowWarned = false;
        // zombiesLeft = info from map controller
        // antibioticsLeft = info from map controller
    }

    private void ChangeAntibioticsValue(int amount)
    {
        antibioticsLeft -= amount;

        if (antibioticsLeft <= 4 && !antibioticsLowWarned && antibioticsLeft > 0)
        {
            OnAntibioticsRunningLow?.Invoke();
            antibioticsLowWarned = true;
        }

        if (antibioticsLeft <= 0)
        {
            antibioticsDepleted = true;
            OnAntibioticsDepleted?.Invoke();
        }        
    }
}
