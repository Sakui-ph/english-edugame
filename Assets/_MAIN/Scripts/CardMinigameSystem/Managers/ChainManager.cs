
using System.Collections.Generic;
using UnityEngine;

namespace CARD_GAME
{
    public class ChainManager : MonoBehaviour
    {
        // Supposed to know which slots are a pair, and handle their data updates as well

        // The point is the chains manager only loads the chains for the page thats set on screen, so the chains list only contains at a max 3-5 chains;

        public const string WARRANT_SLOT_KEY = "WarrantSlot";
        public const string GROUND_SLOT_KEY = "GroundSlot";
        [SerializeField] private GameObject chainPrefab;
        public GameObject chainGroup;
        [SerializeField] private GameObject chainRootPrefab;

        public void SpawnChains(List<Chain> chains, string gameObjectName = "Chain Root")
        {
            GameObject chainRoot = Instantiate(chainRootPrefab, chainGroup.transform);
            foreach(var chain in chains)
            {
                GameObject chainObject = Instantiate(chainPrefab, chainRoot.transform);
                chainRoot.name = gameObjectName;

                // bind the monobehaviours
                CardSlotInteraction[] cardSlots = chainObject.GetComponentsInChildren<CardSlotInteraction>();
                CardSlotInteraction warrantSlotUI = cardSlots[0];
                CardSlotInteraction groundSlotUI = cardSlots[1];

                
                // bind the data
                ChainButton chainButton = chainObject.GetComponentInChildren<ChainButton>();
                chain.SetChainButton(chainButton);

                warrantSlotUI.cardSlot = chain.warrantSlot;
                groundSlotUI.cardSlot = chain.groundSlot;
            }
        }

        public void DestroyChains()
        {
            foreach(Transform child in chainRootPrefab.transform)
            {
                Destroy(child.gameObject);
            }

        }
    }
}