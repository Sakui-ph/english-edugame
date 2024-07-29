using System.Collections.Generic;
using UnityEngine;
namespace CARD_GAME
{
    public class Claim
    {
        public List<Chain> chains = new();
        public string claimText;
        public ClaimType claimType {get; private set;}
        public ClaimType currentSortedType = ClaimType.NONE;
        private bool _isComplete = false;
        private bool _isCorrect = false;

        public bool isComplete {
            get 
            {
                return _isComplete;
            } 
            set {
                _isComplete = value;
                if (_isComplete == true)
                {
                    CardMinigameSystem.instance.CheckFinished();
                }
            }
        }

        public bool isCorrect {
            get 
            {
                return _isCorrect;
            } 
            set {
                _isCorrect = value;
                if (_isCorrect == true)
                {
                    CardMinigameSystem.instance.CheckFinished();
                }
            }
        }

        public Claim(string claimKey, int numChains, string claimText, ClaimType claimType)
        {
            for (int i = 0; i < numChains; i++)
            {
                Chain chain = new(this, claimKey);
                chains.Add(chain);
            }
            this.claimType = claimType;
            this.claimText = claimText;
        }

        public void CheckComplete()
        {
            foreach (Chain chain in chains)
            {
                if (!chain.complete)
                {
                    isComplete = false;
                    return;
                }
            }
            isComplete = true;
        }

        public bool CheckSortedType()
        {
            bool correct = currentSortedType == claimType;

            if (correct)
            {
                isCorrect = true;
                CardGamePlayerDataManager.instance.IncreaseHP();
                return true;
            }
            else
            {
                isCorrect = false;
                CardGamePlayerDataManager.instance.ReduceHP();
                HigherOrderErrorHandler.AddError(HOErrorType.CLAIM_ERROR, CardMinigameSystem.instance.subjectText.text, claimText, currentSortedType.ToString());
                return false;
            }
                
        }
    }

    public enum ClaimType {NONE, IRRELEVANT, FOR, AGAINST};
}
