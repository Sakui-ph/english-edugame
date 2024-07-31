using System.Collections.Generic;
using UnityEngine;
namespace CARD_GAME
{
    public class Claim
    {
        public List<ChainData> chainDataList = new();
        public string claimText;
        public ClaimType claimType {get; private set;}
        public ClaimType currentSortedType = ClaimType.NONE;
        private bool _isChainsCompleted = false;
        private bool _isAnsweredCorrect = false;

        public bool isChainsCompleted {
            get 
            {
                return _isChainsCompleted;
            } 
            set {
                _isChainsCompleted = value;
                if (_isChainsCompleted == true)
                {
                    CardMinigameSystem.instance.TryEndMinigame();
                }
            }
        }

        public bool isAnsweredCorrect {
            get 
            {
                return _isAnsweredCorrect;
            } 
            set {
                _isAnsweredCorrect = value;
                if (_isAnsweredCorrect == true)
                {
                    CardMinigameSystem.instance.TryEndMinigame();
                }
            }
        }

        public delegate void ClaimCheckEvent();
        public event ClaimCheckEvent ListenForCorrectAnswer;
        public event ClaimCheckEvent ListenForIncorrectAnswer; 

        public Claim(string claimKey, int numChains, string claimText, ClaimType claimType)
        {
            for (int i = 0; i < numChains; i++)
            {
                ChainData chain = new(this, claimKey);
                chainDataList.Add(chain);
            }
            this.claimType = claimType;
            this.claimText = claimText;
        }

        public void CheckChains()
        {
            foreach (var chain in chainDataList)
            {
                if (!chain.IsComplete)
                {
                    return;
                }   
            }
            isChainsCompleted = true;
        }

        public void CheckClaim()
        {
            switch(ValidateAnswer())
            {
                case AnswerState.CORRECT:
                    OnCorrectAnswer();
                    break;
                case AnswerState.INCORRECT:
                    OnIncorrectAnswer();
                    break;
            }
        }

        private AnswerState ValidateAnswer()
        {
            Debug.Log($"Current sorted type = {currentSortedType} and Current claim type = {claimType}");
            if (currentSortedType == claimType)
                return AnswerState.CORRECT;
            return AnswerState.INCORRECT;
        }

        private void OnCorrectAnswer()
        {
            isAnsweredCorrect = true;

            CardMinigameSystem.instance.cardGamePlayer.ChangeHealth(1);
            ListenForCorrectAnswer?.Invoke();
        }

        private void OnIncorrectAnswer()
        {
            CardMinigameSystem.instance.cardGamePlayer.ChangeHealth(-1);
            ListenForIncorrectAnswer?.Invoke();
        }
    }

    public enum ClaimType {NONE, IRRELEVANT, FOR, AGAINST};
}
