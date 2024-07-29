using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CARD_GAME
{
    public class CardManager : MonoBehaviour
    {
        public static CardManager instance;

        [Header("Decks")]
        [SerializeField] List<Deck> decks; // TODO (optional) make this cleaner by making it so that only one deck of one type can exist in this list

        [Header("Card Game Objects")]
        [SerializeField] private GameObject groundCardPrefab;
        [SerializeField] private GameObject warrantCardPrefab;
        [SerializeField] private GameObject cardRoot;

        [Header("Card Game Color Settings")] // TODO move to a config if ever this gets too much
        public Color warrantColor = new();
        public Color groundColor = new();

        private List<CardInteraction> cardUIs = new();

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        // spawns a set of cards, the deck it belongs to is not yet taken to account
        public void SpawnCard(HashSet<Card> cards)
        {
            foreach (Card card in cards)
            {
                SpawnCard(card);
            }
        }

        public void SpawnCard(Card card)
        {
            Transform parent;
            GameObject prefab = card.cardType == CardType.ground? groundCardPrefab : warrantCardPrefab;
            if (card.cardParent == null)
                parent = card.deckParent != null? card.deckParent : cardRoot.transform;
            else {
                parent = card.cardParent;
            }
                
            if (parent != null && parent.gameObject != null)
            {
                GameObject newCard = Instantiate(prefab, parent);
                newCard.gameObject.name = $"{card.cardType} Card";

                

                CardInteraction cardUI = newCard.GetComponent<CardInteraction>();
                cardUI.SetCard(card);
                

                TextMeshProUGUI[] TMPs = newCard.GetComponentsInChildren<TextMeshProUGUI>();
                TMPs[0].text = card.cardText;

                cardUIs.Add(cardUI);
            }
            
        }

        public void SortCardsByType(HashSet<Card> cards)
        {
            foreach (Card card in cards)
            {
                foreach (Deck deck in decks)
                {
                    if (deck.cardType == card.cardType)
                    {
                        card.deckParent = deck.transform;
                        continue;
                    }
                }
            } 
        }

        public void ShuffleDecks()
        {
            foreach (Deck deck in decks)
                deck.ShuffleCards();
        }

        

        public void DestroyAllCards()
        {
            foreach (CardInteraction card in cardUIs)
            {
                DestroyImmediate(card.gameObject);
            }
            cardUIs = new();
        }
    }
}

