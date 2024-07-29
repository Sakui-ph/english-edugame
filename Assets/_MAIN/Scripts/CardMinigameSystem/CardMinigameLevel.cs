using System.Collections.Generic;

namespace CARD_GAME
{
    public class CardMinigameLevel
    {
        // handles level progress and level storage
        
        public int totalChains => CalculateTotalChains();
        public List<Claim> claims {get; private set;}
        public HashSet<Card> cards {get; private set;}
        public string subject;

        public CardMinigameLevel(List<Claim> claims, HashSet<Card> cards, string subject)
        {
            this.claims = claims;
            this.cards = cards;
            this.subject = subject;
        }

        private int CalculateTotalChains()
        {
            int totalChains = 0;
            foreach(Claim claim in claims)
            {
                totalChains += claim.chains.Count;
            }
            return totalChains;
        }
    }

}
