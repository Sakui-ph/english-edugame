using System;
using AUDIO_SYSTEM;
using TUTORIAL_MANAGER;
using UnityEngine;
using UnityEngine.UI;

namespace CARD_GAME 
{
    [Serializable]
    public class ClaimSortButtons : ITutorialHelper
    {
        private ClaimManager manager;
        private Claim currentClaim => manager.currentClaim;
        public MultipleChoiceButton forButton;
        public MultipleChoiceButton irrelevantButton;
        public MultipleChoiceButton againstButton;
        public MultipleChoiceGroup multipleChoiceGroup => forButton.multipleChoiceGroup;
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
            forButton.OnClickAction += () => SetClaimType(ClaimType.FOR);
            irrelevantButton.OnClickAction += () => SetClaimType(ClaimType.IRRELEVANT);
            againstButton.OnClickAction += () => SetClaimType(ClaimType.AGAINST);
            checker.onClick.AddListener(CheckClaim);
        }

        private void SetClaimType(ClaimType claimType)
        {
            currentClaim.currentSortedType = claimType;
            TutorialHelper?.Invoke(claimType.ToString());
        }

        public void UpdateClaimType(ClaimType claimType)
        {
            
            switch (claimType)
            {
                case ClaimType.IRRELEVANT:
                    multipleChoiceGroup.OnClick(irrelevantButton);
                    break;
                case ClaimType.FOR:
                    multipleChoiceGroup.OnClick(forButton);
                    break;
                case ClaimType.AGAINST:
                    multipleChoiceGroup.OnClick(againstButton);
                    break;
                default:
                    multipleChoiceGroup.SetAllIdle();
                    return;
            }
        }

        private void CheckClaim()
        {
            currentClaim.ListenForCorrectAnswer += HideChecker;
            currentClaim.CheckClaim();
            currentClaim.ListenForCorrectAnswer -= HideChecker;
        }

        public void HideChecker()
        {
            if (!hidden)
            {
                multipleChoiceGroup.Lock();
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
                multipleChoiceGroup.Unlock();
                checker.enabled = true; 
                hidden = false;
                LeanTween.scale(checkerObject, Vector2.one, 1f);
            }
        }


    }
}