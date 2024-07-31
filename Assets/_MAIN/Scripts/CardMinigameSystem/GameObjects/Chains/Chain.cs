using System;
using UnityEngine;

namespace CARD_GAME
{
    public class Chain : MonoBehaviour
    {   
        public ChainData chainData;
        public ChainButton chainButton;
        [SerializeField] private CardSlot warrantSlot;
        [SerializeField] private CardSlot groundSlot;
        private CardData warrantCardData => warrantSlot.GetComponentInChildren<Card>() 
                                            ? warrantSlot.GetComponentInChildren<Card>().cardData
                                            : null;

        private CardData groundCardData => groundSlot.GetComponentInChildren<Card>() 
                                            ? groundSlot.GetComponentInChildren<Card>().cardData
                                            : null;

        void Awake()
        {
            chainButton.onClick.AddListener(() => CheckChain());
        }

        private AnswerState ValidateAnswer()
        {
            // EITHER SLOT EMPTY
            if (warrantCardData == null || groundCardData == null)
                return AnswerState.NONE;
            
            // EITHER SLOT RED HERRING
            if (!warrantCardData.hasConnectionKey || !groundCardData.hasConnectionKey)
            {
                return AnswerState.INCORRECT;
            }
                

            // IF EITHER CLAIM KEY DOES NOT LINE UP
            if (chainData.claimKey != warrantCardData.claimKey 
                || chainData.claimKey != groundCardData.claimKey)
            {
                return AnswerState.INCORRECT;
            }
                
                
            if (warrantCardData.connectionKey == groundCardData.connectionKey)
                return AnswerState.CORRECT;
            
            
            return AnswerState.INCORRECT;
        }

        private void CheckChain()
        {
            switch (ValidateAnswer())
            {
                case AnswerState.NONE:
                    break;
                case AnswerState.CORRECT:
                // change to lock
                    OnCorrectAnswer();
                    break;
                case AnswerState.INCORRECT:
                    OnIncorrectAnswer();
                    break;
            }
        }

        private void OnCorrectAnswer()
        {
            groundSlot.Lock();
            warrantSlot.Lock();
            chainData.IsComplete = true;
            chainButton.animationHandler.CorrectAnimation(() => 
            {
                // todo: add the final check for if the game is done
            });
        }

        private void OnIncorrectAnswer()
        {
            CardMinigameSystem.instance.cardGamePlayer.ChangeHealth(-1);
        }
    }   

    public enum AnswerState
    {
        CORRECT, INCORRECT, NONE
    }
}

