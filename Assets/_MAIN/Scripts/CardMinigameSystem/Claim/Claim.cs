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
                ChainData chain = new(this, claimKey);
                chainDataList.Add(chain);
            }
            this.claimType = claimType;
            this.claimText = claimText;
        }
    }

    public enum ClaimType {NONE, IRRELEVANT, FOR, AGAINST};
}
