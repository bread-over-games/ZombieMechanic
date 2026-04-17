using UnityEngine;
using System;

public class UIPauseMenu : MonoBehaviour
{
    private enum PauseMenuButton
        {
            ResumeGame,
            RestartGame,
            ExitGame
        }

    public GameObject pauseMenuWindow;
    private PauseMenuButton pauseMenuButtonSelected;

    public static Action OnExitGameButtonSelected;
    public static Action OnResumeGameButtonSelected;
    public static Action OnRestartGameButtonSelected;

    private void OnEnable()
    {
        PauseController.OnGamePaused += OpenPauseMenu;
        PauseController.OnGameResumed += ClosePauseMenu;
    }

    private void OnDisable()
    {
        PauseController.OnGamePaused -= OpenPauseMenu;
        PauseController.OnGameResumed -= ClosePauseMenu;
    }

    private void OpenPauseMenu()
    {
        PlayerInteraction.OnPrimaryInteractionInterceptor = ConfirmButtonSelection;
        UIFocusStack.Push(pauseMenuWindow);
    }

    private void ClosePauseMenu()
    {
        PlayerInteraction.OnPrimaryInteractionInterceptor = null;
        UIFocusStack.Pop();
    }

    public void SelectResumeGame()
    {
        pauseMenuButtonSelected = PauseMenuButton.ResumeGame;
    }

    public void SelectRestartGame()
    {
        pauseMenuButtonSelected = PauseMenuButton.RestartGame;
    }

    public void SelectExitGame()
    {
        pauseMenuButtonSelected = PauseMenuButton.ExitGame;
    }

    private void ConfirmButtonSelection()
    {
        switch (pauseMenuButtonSelected)
        {
            case PauseMenuButton.ResumeGame:
                OnResumeGameButtonSelected?.Invoke();
                ClosePauseMenu();
                break;
            case PauseMenuButton.RestartGame:
                ClosePauseMenu();
                OnRestartGameButtonSelected?.Invoke();
                break;
            case PauseMenuButton.ExitGame:
                OnExitGameButtonSelected?.Invoke();
                break;
        }
    }
}
