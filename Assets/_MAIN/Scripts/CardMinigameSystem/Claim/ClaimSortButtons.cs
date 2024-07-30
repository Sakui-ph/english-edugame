using System;
using AUDIO_SYSTEM;
using UnityEngine;
using UnityEngine.UI;

namespace CARD_GAME 
{
    [Serializable]
    public class ClaimSortButtons
    {
        private ClaimManager manager;
        private Claim currentClaim => manager.currentClaim;
        public MultipleChoiceButton forButton;
        public MultipleChoiceButton irrelevantButton;
        public MultipleChoiceButton againstButton;
        public MultipleChoiceGroup mcg => forButton.multipleChoiceGroup;
        public static event Action<string> TutorialHelper;
        public Button checker;
        private bool hidden = false;
        private GameObject checkerObject => checker.gameObject;

        public ClaimSortButtons(ClaimManager manager)
        {
            this.manager = manager;
        }

        public void InitializeButtons()
        {
            forButton.OnClickAction += SetClaimFor;
            irrelevantButton.OnClickAction += SetClaimIrrelevant;
            againstButton.OnClickAction += SetClaimAgainst;
        }

        private void SetClaimFor()
        {
            currentClaim.currentSortedType = ClaimType.FOR;
            TutorialHelper?.Invoke(ClaimType.FOR.ToString());
        }

        private void SetClaimAgainst()
        {
            currentClaim.currentSortedType = ClaimType.AGAINST;
            TutorialHelper?.Invoke(ClaimType.AGAINST.ToString());
        }

        private void SetClaimIrrelevant()
        {
            currentClaim.currentSortedType = ClaimType.IRRELEVANT;
            TutorialHelper?.Invoke(ClaimType.IRRELEVANT.ToString());
        }

        public void UpdateClaimType(ClaimType claimType)
        {
            switch (claimType)
            {
                case ClaimType.IRRELEVANT:
                    mcg.OnClick(irrelevantButton);
                    SetClaimIrrelevant();
                    break;
                case ClaimType.FOR:
                    mcg.OnClick(forButton);
                    SetClaimFor();
                    break;
                case ClaimType.AGAINST:
                    mcg.OnClick(againstButton);
                    SetClaimAgainst();
                    break;
                default:
                    mcg.SetAllIdle();
                    return;
            }
        }

        public void HideChecker()
        {
            if (!hidden)
            {
                mcg.Lock();
                checker.enabled = false; 
                hidden = true;
                LeanTween.rotateAround(checkerObject, Vector3.right, 360, 1);
                LeanTween.scale(checkerObject, Vector2.zero, 1f);
            }
        }

        public void ShowChecker()
        {
            if (hidden)
            {
                mcg.Unlock();
                checker.enabled = true; 
                hidden = false;
                LeanTween.scale(checkerObject, Vector2.one, 1f);
            }
        }


    }
}