using System;
using UnityEngine;

namespace CARD_GAME
{
    public class Chain
    {   
        public Claim parent;
        private ChainButton chainButton;
        public CardSlot warrantSlot;
        public CardSlot groundSlot;
        public string claimKey {get; private set;}
        public bool isFinished;
        public event Action correctCombo;
        public event Action incorrectCombo;
        public bool complete = false;

        public Chain(Claim parent, string claimKey)
        {
            this.parent = parent;
            // warrantSlot = new CardSlot(CardType.warrant);
            // groundSlot = new CardSlot(CardType.ground);
            this.claimKey = claimKey;
        }

        public void SetChainButton(ChainButton chainButton)
        {
            this.chainButton = chainButton;
            chainButton.claimParent = this.parent;
            chainButton.claimKey = claimKey;
            chainButton.incorrectCombo += IncorrectCombo;
            chainButton.correctCombo += CorrectCombo;
        }

        private void IncorrectCombo()
        {
            CardGamePlayerDataManager.instance.ReduceHP();
            incorrectCombo?.Invoke();
        }

        private void CorrectCombo()
        {
            complete = true;
            correctCombo?.Invoke();
        }
    }   
}

