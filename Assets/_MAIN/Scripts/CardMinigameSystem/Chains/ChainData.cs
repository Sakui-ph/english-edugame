using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CARD_GAME
{
    public class ChainData
    {
        public Claim parent;
        public string claimKey {get; private set;}
        public bool complete = false;

        public ChainData(Claim parent, string claimKey)
        {
            this.parent = parent;
            this.claimKey = claimKey;
        }
    }
}
