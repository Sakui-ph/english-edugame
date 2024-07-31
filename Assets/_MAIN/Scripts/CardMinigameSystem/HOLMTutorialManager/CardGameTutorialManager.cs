using System.Collections;
using System.Collections.Generic;
using CARD_GAME;
using UnityEngine;
using UnityEngine.UI;

namespace TUTORIAL_MANAGER
{
    public class CardGameTutorialManager : MonoBehaviour
    {
        public List<TutorialStep> panels = new();
        private Coroutine process = null;
        public bool isRunning => process != null;
        public Button next;
        public GameObject inputBlocker;

        void Start()
        {
            process = StartCoroutine(RunTutorial());
        }

        private IEnumerator RunTutorial()
        {
            foreach (TutorialStep step in panels)
            {
                step.gameObject.SetActive(true);

                next.onClick.AddListener(() => {step.UserPrompt();});
                step.hideNext += HideNext;
                step.showNext += ShowNext;
                step.blockInputs += BlockInputs;
                step.allowInputs += AllowInputs;

                yield return step.SayLines();

                step.RemoveListeners();
                step.gameObject.SetActive(false);
                next.onClick.RemoveAllListeners();
            } 

            HideNext();

            GameSystem.instance.GetLoadedPlayer().hasSeenHOTutorial = true;
            GameSystem.instance.SaveLoadedPlayer();
            SceneHandler.instance.UnloadScene(SceneName.HOLMTutorial);
        }

        public void BlockInputs()
        {
            inputBlocker.SetActive(true);
        }

        public void AllowInputs()
        {
            inputBlocker.SetActive(false);
        }

        public void HideNext()
        {
            CanvasGroup cg = next.GetComponent<CanvasGroup>();
            CanvasGroupControl.HideCanvasGroup(cg);
        }

        public void ShowNext()
        {
            CanvasGroup cg = next.GetComponent<CanvasGroup>();
            CanvasGroupControl.ShowCanvasGroup(cg);
        }
    }
}
