using System;
using System.Collections;
using AUDIO_SYSTEM;
using UnityEngine;
using UnityEngine.UI;

namespace CARD_GAME
{
    [RequireComponent(typeof(Button))]
    public class ChainButton : MonoBehaviour
    {
        public Claim claimParent;
        public Button chainButton;
        public string claimKey = null;
        public ChainButtonEffects animationHandler;
        public event Action incorrectCombo;
        public event Action correctCombo;
        public static event Action<string> TutorialHelper;
        

        void Awake()
        {
            chainButton = GetComponent<Button>();
            chainButton.onClick.AddListener(CheckAnswers);
            animationHandler = gameObject.GetComponent<ChainButtonEffects>();
        }

        private void CheckAnswers()
        {
            AudioManager.instance.PlaySoundEffect("Switch");

            if (claimKey == null)
            {
                Debug.LogWarning("Chain has no claim key!");
                incorrectCombo?.Invoke();
                return;
            }

            CardSlotInteraction[] cardSlots = transform.parent.GetComponentsInChildren<CardSlotInteraction>();
            CardInteraction cardUIOne = cardSlots[0].GetOccupyingCardUI();
            CardInteraction cardUITwo = cardSlots[1].GetOccupyingCardUI();

            if (!cardUIOne || !cardUITwo)
            {
                return;
            }

            Card cardOne = cardUIOne.card;
            Card cardTwo = cardUITwo.card;

            if (cardOne == null || cardTwo == null)
            {
                return;
            }

            if (cardOne.CheckPair(cardTwo, claimKey, claimParent.claimText, claimParent.currentSortedType.ToString()))
            {
                TutorialHelper?.Invoke("CORRECT");

                AudioManager.instance.PlaySoundEffect("CorrectSound");
                animationHandler.CorrectAnimation(() => 
                {
                    claimParent.CheckComplete();
                });
                LockCardSlots();
                correctCombo?.Invoke();
                return;
            }

            TutorialHelper?.Invoke("INCORRECT");
            incorrectCombo?.Invoke();     
            return;  
        }

        private void LockCardSlots()
        {
            CardSlotInteraction[] cardSlots = transform.parent.GetComponentsInChildren<CardSlotInteraction>();
            cardSlots[0].Lock();
            cardSlots[1].Lock();
        }
    }

    
}
