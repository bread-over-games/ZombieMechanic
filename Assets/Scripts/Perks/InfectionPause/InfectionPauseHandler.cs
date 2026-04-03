using UnityEngine;
using System;
using System.Collections;

public class InfectionPauseHandler : MonoBehaviour
{
    public InfectionPause infectionPauseSO;
    public static Action OnInfectionPause;
    public static Action OnInfectionResume;

    private void OnEnable()
    {
        InfectionPause.OnInfectioanPauseActivated += InfectionPauseStart;
    }

    private void OnDisable()
    {
        InfectionPause.OnInfectioanPauseActivated -= InfectionPauseStart;
    }

    private void InfectionPauseStart()
    {
        OnInfectionPause?.Invoke();
        StartCoroutine(InfectionPauseTimer());
    }

    private void InfectionPauseStop()
    {
        OnInfectionResume?.Invoke();
    }

    private IEnumerator InfectionPauseTimer()
    {       
        yield return new WaitForSeconds(infectionPauseSO.infectionPauseDuration);
        InfectionPauseStop();        
    }
}
