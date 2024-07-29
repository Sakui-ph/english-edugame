using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CARD_GAME
{
    public class ClaimManager : MonoBehaviour
    {
        [Header("Claim Tab Group")]
        [SerializeField] private TextMeshProUGUI claimTextTMP;
        [SerializeField] public TabGroup claimTabGroup;
        [SerializeField] public GameObject claimTabPrefab;
        
        [Header("Claim Sorting Buttons")]
        [SerializeField] private MultipleChoiceButton forButton;
        [SerializeField] private MultipleChoiceButton irrelevantButton;
        [SerializeField] private MultipleChoiceButton againstButton;
        [SerializeField] private Button checker;
        private ClaimSortButtons claimSortButtons;
        private List<Claim> claims;
        public Claim currentClaim;

        void Awake()
        {
            claimTabGroup.OnSwitch += UpdateClaimText;
            claimSortButtons = new(this)
            {
                forButton = forButton,
                irrelevantButton = irrelevantButton,
                againstButton = againstButton,
                checker = checker
            };
            claimSortButtons.InitializeButtons();
        }


        public void SetClaimData(List<Claim> claims)
        {
            this.claims = claims;
        }

        public void SetupClaimTabs(int numClaimTabs)
        {            
            for (int i = 0; i < numClaimTabs; i++)
            {
                GameObject claimTab = Instantiate(claimTabPrefab, claimTabGroup.transform);
                claimTab.name = $"Tab - Claim Tab {i+1}";

                TextMeshProUGUI claimTabTMP = claimTab.GetComponentInChildren<TextMeshProUGUI>();
                claimTabTMP.text = $"{i+1}";

                TabGroupButton tabButton = claimTab.GetComponentInChildren<TabGroupButton>();
                tabButton.tabGroup = claimTabGroup;
            }
        }

        public void MapChainToTabGroup(GameObject chainGroup)
        {
            claimTabGroup.objectsToSwap = new List<GameObject>();
            foreach (Transform child in chainGroup.transform)
            {
                claimTabGroup.objectsToSwap.Add(child.gameObject);
            }
        }

        private void UpdateClaimText(int index)
        {
            claimTextTMP.text = claims[index].claimText;
            currentClaim = claims[index];

            ClaimType claimType = currentClaim.currentSortedType;
            claimSortButtons.UpdateClaimType(claimType);

            if (currentClaim.isCorrect)
            {
                claimSortButtons.HideChecker();
            }
            else
                claimSortButtons.ShowChecker();
        }

        public bool CheckFinished()
        {
            foreach (Claim claim in claims)
            {
                if (!claim.isCorrect || !claim.isComplete)
                    return false;
            }
            return true;
        }

    }
}