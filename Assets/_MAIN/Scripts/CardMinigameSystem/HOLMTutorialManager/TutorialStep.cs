using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CARD_GAME;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TUTORIAL_MANAGER
{
    public class TutorialStep : MonoBehaviour
    {
        public List<string> lines = new();
        private SpeechBubble speechBubble => GetComponentInChildren<SpeechBubble>();
        private Coroutine process;
        public bool isRunning => process != null;
        public bool complete = false;
        private bool waitForUser = false;
        private bool userError = false;
        public event Action hideNext;
        public event Action showNext; 
        public event Action blockInputs;
        public event Action allowInputs;
        public TutorialCondition tutorialCondition;
        public string ErrorMessage = "Put something here if you wont to listen for something";
        public string ExpectedString = "Put expected result here";

        // Start is called before the first frame update

        public Coroutine SayLines()
        {
            if (isRunning)
                StopCoroutine(process);
            process = StartCoroutine(RunSayLines());
            return process;
        }

        public void UserPrompt()
        {
            waitForUser = false;
        }

        public void UserPrompt(string value)
        {
            if (value == ExpectedString) {
                waitForUser = false;
            } else {
                userError = true;
            }
        }

        // could also just make an enum with a bunch of switches
        private IEnumerator RunSayLines()
        {
            foreach (string line in lines)
            {
                hideNext?.Invoke();
                blockInputs?.Invoke();
                yield return speechBubble.Say(line);

                yield return PerformSelectedCondition();
                
            }
            complete = true;
        }

        private IEnumerator PerformSelectedCondition()
        {
            switch(tutorialCondition)
            {
                case TutorialCondition.TABGROUP:
                    yield return WaitForTabChange();
                    break;
                case TutorialCondition.CARDSLOT:
                    yield return WaitForCardDrop();
                    break;
                case TutorialCondition.CLAIMCHECK:
                    yield return WaitForClaimCheck();    
                    break;
                case TutorialCondition.CLAIMSORTBUTTON:
                    yield return WaitForClaimSort();
                    break;
                case TutorialCondition.CHAINBUTTON:
                    yield return WaitForChainCheck();
                    break;
                case TutorialCondition.NONE:
                    showNext?.Invoke();
                    yield return WaitForUser();
                    break;
            }
        }
        private IEnumerator WaitForTabChange() => WaitForAction<TabGroup>();
        private IEnumerator WaitForCardDrop() => WaitForAction<CardSlot>();
        private IEnumerator WaitForClaimCheck() => WaitForAction<Claim>();
        private IEnumerator WaitForClaimSort() => WaitForAction<ClaimSortButtons>();
        private IEnumerator WaitForChainCheck() => WaitForAction<ChainButton>();

        private IEnumerator WaitForAction<T>() 
            where T : ITutorialHelper
        {
            allowInputs?.Invoke();
            EventInfo eventInfo = typeof(T).GetEvent("TutorialHelper", BindingFlags.Public | BindingFlags.Static);
            Action<string> handler = UserPrompt;

            eventInfo.AddEventHandler(null, handler);
            yield return WaitForUser();
            eventInfo.RemoveEventHandler(null, handler);
        }

        private IEnumerator WaitForUser()
        {
            waitForUser = true;
            while (waitForUser) {
                if (userError)
                {
                    yield return speechBubble.Say(ErrorMessage);
                    userError = false;
                }
                yield return null;
            }
                
            waitForUser = false;
        }

        public void RemoveListeners()
        {
            hideNext = null;
            showNext = null;
            blockInputs = null;
            allowInputs = null;
        }
    }


}

public enum TutorialCondition
{
    NONE, CLAIMCHECK, CARDSLOT, CLAIMSORTBUTTON, TABGROUP, CHAINBUTTON
}
