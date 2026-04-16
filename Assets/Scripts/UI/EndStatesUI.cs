using UnityEngine;
using System;

public class EndStatesUI : MonoBehaviour
{
    [SerializeField] private GameObject zombieEndState;
    [SerializeField] private GameObject exterminationEndState;

    private bool victoryStateShown = false;

    public static Action OnRestartConfirmed;

    private void OnEnable()
    {
        Infection.OnInfectionReachedMaxLevel += ZombieEndStateShow;
        ZombiesController.OnAllZombiesKilled += ExterminationEdnStateShow;
    }

    private void OnDisable()
    {
        Infection.OnInfectionReachedMaxLevel -= ZombieEndStateShow;
        ZombiesController.OnAllZombiesKilled -= ExterminationEdnStateShow;
    }

    private void ZombieEndStateShow()
    {
        zombieEndState.SetActive(true);
        PlayerInteraction.OnSecondaryInteractionInterceptor = ZombieEndStateHide;
    }

    private void ZombieEndStateHide()
    {
        PlayerInteraction.OnSecondaryInteractionInterceptor = null;
        OnRestartConfirmed?.Invoke();
    }

    private void ExterminationEdnStateShow()
    {
        if (!victoryStateShown)
        {
            exterminationEndState.SetActive(true);
            Invoke("ExterminationEndStateHide", 13f);
            victoryStateShown = true;
        }        
    }

    private void ExterminationEndStateHide()
    {
        exterminationEndState.SetActive(false);
    }
}
