using CARD_GAME;
using DIALOGUE;
using UnityEngine;
using UnityEngine.Audio;


// SCENES
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
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
        pauseMenu.SetActive(true);
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
    }


    public void Reset()
    {
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
