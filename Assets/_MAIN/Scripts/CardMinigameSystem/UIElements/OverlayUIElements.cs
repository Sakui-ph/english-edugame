using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CARD_GAME
{
    [System.Serializable]
    public class OverlayUIElements
    {
        public CanvasGroup overlayImage;

        public LTDescr FadeToBlack()
        {
            return LeanTween.alphaCanvas(overlayImage, 1, 2);
        }

        public LTDescr FadeToWhite()
        {
            return LeanTween.alphaCanvas(overlayImage, 0, 2);
        }
    }
}
