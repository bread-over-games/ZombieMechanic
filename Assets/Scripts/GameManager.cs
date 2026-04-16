using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isGameOver = false;

    public static Action OnGameOver;
    public static Action OnGameStart;

    private void OnEnable()
    {
        Infection.OnInfectionReachedMaxLevel += DeclareGameOver;
        EndStatesUI.OnRestartConfirmed += RestartGame;
        UIPauseMenu.OnRestartGameButtonSelected += RestartGame;
        UIPauseMenu.OnExitGameButtonSelected += ExitGame;   
    }

    private void OnDisable()
    {
        Infection.OnInfectionReachedMaxLevel -= DeclareGameOver;
        EndStatesUI.OnRestartConfirmed -= RestartGame;
        UIPauseMenu.OnRestartGameButtonSelected -= RestartGame;
        UIPauseMenu.OnExitGameButtonSelected -= ExitGame;
    }

    private void Awake()
    {
        Instance = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        OnGameStart?.Invoke();
    }

    private void DeclareGameOver()
    {
        isGameOver = true;
        OnGameOver?.Invoke();
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Garage");
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
