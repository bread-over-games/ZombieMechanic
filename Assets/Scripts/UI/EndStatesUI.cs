using UnityEngine;

public class EndStatesUI : MonoBehaviour
{
    [SerializeField] private GameObject zombieEndState;
    [SerializeField] private GameObject exterminationEndState;

    private bool victoryStateShown = false;

    private void OnEnable()
    {
        Infection.OnInfectionReachedMaxLevel += ZombieEndState;
        ZombiesController.OnAllZombiesKilled += ExterminationEdnStateShow;
    }

    private void OnDisable()
    {
        Infection.OnInfectionReachedMaxLevel -= ZombieEndState;
        ZombiesController.OnAllZombiesKilled -= ExterminationEdnStateShow;
    }

    private void ZombieEndState()
    {
        zombieEndState.SetActive(true);
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
