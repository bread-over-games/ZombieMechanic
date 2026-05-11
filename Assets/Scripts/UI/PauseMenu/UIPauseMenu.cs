using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class UIPauseMenu : MonoBehaviour
{
    private enum PauseMenuButton
        {
            ResumeGame,
            RestartGame,
            ExitGame,
            WishlistGame
        }

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

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

    public void SelectWishlist()
    {
        pauseMenuButtonSelected = PauseMenuButton.WishlistGame;
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
            case PauseMenuButton.WishlistGame:
                Debug.Log("Wishlist game");
                Application.OpenURL("https://store.steampowered.com/app/4692870/ZArmory/");
                ShowWindow(GetActiveWindow(), 2); // 2 = SW_MINIMIZE
                break;
        }
    }
}
