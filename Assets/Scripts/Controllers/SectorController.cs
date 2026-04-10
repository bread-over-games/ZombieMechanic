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
    public bool isReadingMessage = false;

    private void OnEnable()
    {
        ObjectGenerator.OnAntibioticsGenerated += ChangeAntibioticsValue;
        PlayerInteraction.OnMessageConfirmed += MessageConfirmed;
    }

    private void OnDisable()
    {
        ObjectGenerator.OnAntibioticsGenerated -= ChangeAntibioticsValue;
        PlayerInteraction.OnMessageConfirmed -= MessageConfirmed;
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

        if (antibioticsLeft <= 3 && !antibioticsLowWarned)
        {
            OnAntibioticsRunningLow?.Invoke();
            isReadingMessage = true;
            antibioticsLowWarned = true;
        }

        if (antibioticsLeft <= 0)
        {
            antibioticsDepleted = true;
            isReadingMessage = true;
            OnAntibioticsDepleted?.Invoke();
        }        
    }

    private void MessageConfirmed()
    {

        isReadingMessage = false;
    }
}
