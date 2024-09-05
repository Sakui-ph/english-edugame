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
        Continue();
        pauseButton.onClick.AddListener(Pause);
        continueButton.onClick.AddListener(Continue);
        resetButton.onClick.AddListener(Reset);
        quitButton.onClick.AddListener(Quit);
    }

    public void Pause()
    {
        CanvasGroupControl.ShowCanvasGroup(pauseMenu);
    }

    public void Continue()
    {
        CanvasGroupControl.HideCanvasGroup(pauseMenu);
    }


    public void Reset()
    {
        // todo: delegate these to a score handler of some sort
        HigherOrderErrorHandler.Reset();
        LowerOrderScoreHandler.Reset();
        GameSystem.instance.ResetLevel();

        Continue();
    }

    public void Quit()
    {
        HigherOrderErrorHandler.Reset();
        LowerOrderScoreHandler.Reset();
        GameSystem.instance.LoadMainMenu();
    }

}
