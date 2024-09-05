using CARD_GAME;
using DIALOGUE;
using UnityEngine;
using UnityEngine.Audio;


// SCENES
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] CanvasGroup pauseMenu;
    public Button pauseButton;
    public Button continueButton;
    public Button resetButton;
    public Button quitButton;

    void Awake()
    {
        ClosePauseMenu();
        pauseButton.onClick.AddListener(OpenPauseMenu);
        continueButton.onClick.AddListener(ClosePauseMenu);
        resetButton.onClick.AddListener(ResetButton);
        quitButton.onClick.AddListener(QuitButton);
    }

    public void OpenPauseMenu()
    {
        CanvasGroupControl.ShowCanvasGroup(pauseMenu);
    }

    public void ClosePauseMenu()
    {
        CanvasGroupControl.HideCanvasGroup(pauseMenu);
    }


    public void ResetButton()
    {
        // todo: delegate these to a score handler of some sort
        HigherOrderErrorHandler.Reset();
        LowerOrderScoreHandler.Reset();
        GameSystem.instance.ResetLevel();

        ClosePauseMenu();
    }

    public void QuitButton()
    {
        HigherOrderErrorHandler.Reset();
        LowerOrderScoreHandler.Reset();
        GameSystem.instance.LoadMainMenu();
    }

}
