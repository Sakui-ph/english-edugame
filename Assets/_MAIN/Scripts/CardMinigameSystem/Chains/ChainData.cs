using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CARD_GAME
{
    public class ChainData
    {
        public Claim parent;
        public string claimKey {get; private set;}
        private bool _isComplete = false;

        public bool isComplete {
            get 
            {
                return _isComplete;
            } 
            set {
                _isComplete = value;
                if (_isComplete == true)
                {
                    parent.CheckChains();
                }
            }
        }

        public ChainData(Claim parent, string claimKey)
        {
            this.parent = parent;
            this.claimKey = claimKey;
        }
    }
}
