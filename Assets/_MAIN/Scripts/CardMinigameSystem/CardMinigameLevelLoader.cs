


using System;
using System.Collections.Generic;
using UnityEngine;


namespace CARD_GAME
{   
    public static class CardMinigameLevelLoader
    {
        private static CardMinigameLevel level;
        public static List<Claim> claims = new();
        public static HashSet<CardData> cardDataSet = new();
        public static string subject = "";
        public static string postLevelChapterReference = "";
        
        public static CardMinigameLevel LoadLevel()
        {
            if (claims.Count == 0 || cardDataSet.Count == 0)
            {
                Debug.LogError("There are no claims or cards");
                return null;
            }

            if (subject == "")
            {
                Debug.LogError("No subject supplied");
                return null;
            }

            level = new(claims, cardDataSet, subject, postLevelChapterReference);
            return level;
        }

        public static void Reset()
        {
            level = null;
            claims = new();
            cardDataSet = new();
            subject = "";
        }

        public static void AddCard(string cardText, string claimKey, CardType cardType, string connectionKey)
        {
            cardDataSet.Add(new CardData(cardText, cardType, claimKey, connectionKey));
        }

        public static void AddCard(string cardText, CardType cardType)
        {
            cardDataSet.Add(new CardData(cardText, cardType));
        }

        public static void AddClaim(string claimText, string claimKey, ClaimType claimType, int numChains)
        {
            claims.Add(new Claim(claimKey, numChains, claimText, claimType));
        }
    }
}
