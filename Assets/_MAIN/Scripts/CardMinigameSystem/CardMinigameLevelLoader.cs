


using System;
using System.Collections.Generic;
using UnityEngine;


namespace CARD_GAME
{   
    public static class CardMinigameLevelLoader
    {
        private static CardMinigameLevel level;
        public static List<Claim> claims = new();
        public static HashSet<Card> cards = new();
        public static string subject = "";
        public static event Action OnCardGameEnd;
        
        public static CardMinigameLevel LoadLevel()
        {
            if (claims.Count == 0 || cards.Count == 0)
            {
                Debug.LogError("There are no claims or cards");
                return null;
            }

            if (subject == "")
            {
                Debug.LogError("No subject supplied");
                return null;
            }

            level = new(claims, cards, subject);
            return level;
        }

        public static void Reset()
        {
            level = null;
            claims = new();
            cards = new();
            subject = "";
        }

        public static void AddCard(string cardText, string claimKey, CardType cardType, string connectionKey)
        {
            cards.Add(new Card(cardText, cardType, claimKey, connectionKey));
        }

        public static void AddCard(string cardText, CardType cardType)
        {
            cards.Add(new Card(cardText, cardType));
        }

        public static void AddClaim(string claimText, string claimKey, ClaimType claimType, int numChains)
        {
            claims.Add(new Claim(claimKey, numChains, claimText, claimType));
        }

        public static void EndGame()
        {
            OnCardGameEnd?.Invoke();
            OnCardGameEnd = null;
        }
    }
}
