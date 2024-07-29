using UnityEngine;
using UnityEngine.UI;

namespace MAIN_MENU
{
    [System.Serializable]
    public class MainMenuButtons 
    {
        public Button playButton;
        public Button openOptionsButton;
        public Button openCreditsButton;
        public Button exitCreditsButton;
        public Button exitOptionsButton;
        public Button quitButton;
        public Button exitLevelSelection;
        public GameObject levelSelection;
        public GameObject optionsMenuCanvas;
        public GameObject mainMenuPageCanvas;
        public GameObject creditsCanvas;
        private delegate void PlayGameEvent();
        private void OpenOptions() {
            CloseMainMenuPage();
            CanvasGroupControl.ShowCanvasGroup(optionsMenuCanvas);
        }
        private void CloseOptions() {
            CanvasGroupControl.HideCanvasGroup(optionsMenuCanvas);
            OpenMainMenuPage();
        }

        private void OpenCredits()
        {
            CloseMainMenuPage();
            creditsCanvas.SetActive(true);
        }

        private void CloseCredits()
        {
            OpenMainMenuPage();
            creditsCanvas.SetActive(false);
        }



        private void OpenMainMenuPage() => mainMenuPageCanvas.SetActive(true);
        private void CloseMainMenuPage() => mainMenuPageCanvas.SetActive(false);
        private void OpenLevelSelect() => CanvasGroupControl.ShowCanvasGroup(levelSelection);
        private void CloseLevelSelect() => CanvasGroupControl.HideCanvasGroup(levelSelection);
        public void InitializeButtons() 
        {
            playButton.onClick.AddListener(OpenLevelSelect);
            exitLevelSelection.onClick.AddListener(CloseLevelSelect);
            openOptionsButton.onClick.AddListener(OpenOptions);
            exitOptionsButton.onClick.AddListener(CloseOptions);
            openCreditsButton.onClick.AddListener(OpenCredits);
            exitCreditsButton.onClick.AddListener(CloseCredits);
            quitButton.onClick.AddListener(() => 
            {
                #if UNITY_STANDALONE
                    Application.Quit();
                #endif
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #endif
            });
        }
    }
}
