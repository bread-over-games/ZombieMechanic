using UnityEngine;

public class EndStatesUI : MonoBehaviour
{
    [SerializeField] private GameObject zombieEndState;
    [SerializeField] private GameObject exterminationEndState;

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
        exterminationEndState.SetActive(true);
        Invoke("ExterminationEndStateHide", 13f);
    }

    private void ExterminationEndStateHide()
    {
        exterminationEndState.SetActive(false);
    }
}
