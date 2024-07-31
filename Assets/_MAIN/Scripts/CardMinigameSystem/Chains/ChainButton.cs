using System;
using System.Collections;
using AUDIO_SYSTEM;
using UnityEngine;
using UnityEngine.UI;

namespace CARD_GAME
{
    public class ChainButton : MenuButton
    {
        public ChainButtonEffects animationHandler;
        public static event Action<string> TutorialHelper;
        

        protected override void Awake()
        {
            animationHandler = gameObject.GetComponent<ChainButtonEffects>();
            base.Awake();
        }
    }

    
}
