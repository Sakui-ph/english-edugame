using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

namespace CARD_GAME
{
    public class CardManager : MonoBehaviour
    {
        public static CardManager instance;

        [Header("Decks")]
        [SerializeField] Deck warrantDeck;
        [SerializeField] Deck groundDeck;

        [Header("Card Game Objects")]
        [SerializeField] private GameObject groundCardPrefab;
        [SerializeField] private GameObject warrantCardPrefab;
        [SerializeField] private GameObject cardRoot;

        [Header("Card Game Color Settings")] // TODO move to a config if ever this gets too much
        public Color warrantColor = new();
        public Color groundColor = new();

        private List<Card> cards = new();

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
        public void SpawnCard(HashSet<CardData> cardDataSet)
        {
            foreach (CardData card in cardDataSet)
            {
                SpawnCard(card);
            }
            ShuffleDecks();
        }

        private void SpawnCard(CardData cardData)
        {
            Deck deck = SortCardDeck(cardData.cardType);

            if (deck == null)
            {
                Debug.LogError("No deck suitable for spawning card");
                return;
            }

            Transform deckTransform = deck.transform;
            GameObject prefab = cardData.cardType == CardType.ground? groundCardPrefab : warrantCardPrefab;
        
            GameObject cardObject = Instantiate(prefab, deckTransform);
            cardObject.name = $"{cardData.cardType} Card";

            Card card = cardObject.GetComponent<Card>();
            card.cardData = cardData;
            cards.Add(card);
        }

        public void ShuffleDecks()
        {
            warrantDeck.ShuffleCards();
            groundDeck.ShuffleCards();
        }

        private Deck SortCardDeck(CardType cardType)
        {
            switch(cardType)
            {
                case CardType.warrant:
                    return warrantDeck;
                case CardType.ground:
                    return groundDeck;
                default:
                    Debug.LogError($"This card type {cardType.ToString()} does not exist");
                    break;
            }
            return null;
        }

        public void DestroyAllCards()
        {
            foreach(Card card in cards)
            {   
                Destroy(card.gameObject);
            }
            cards = new();
        }
    }
}

