using AUDIO_SYSTEM;
using UnityEngine;
using UnityEngine.Video;

namespace MAIN_MENU
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private MainMenuButtons mainMenuButtons = new();
        [SerializeField] private MainMenuAudio mainMenuAudio = new();
        [SerializeField] private MainMenuCG mainMenuCG;
        public Canvas canvas;
        private bool isFirstStart => GameSystemSL.services.gameSystem.isFirstStart;
        
        private void Start() 
        {     
            mainMenuButtons.InitializeButtons();

            GameSystemSL.services.audioManager.StopAllSoundEffects();

            mainMenuAudio.Play();
            if (!isFirstStart) {
                enabled = false;
            }

        }

        void Update()
        {
            mainMenuCG.Animate();
            enabled = false;
        }

        void OnDestroy()
        {
            mainMenuAudio.Stop();
        }
    }
}
