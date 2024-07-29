using UnityEngine;

namespace CARD_GAME
{
    public class Card
    {
        // TODO: In the future, add a button thingie that leads to some kind of connection with the VN for displaying hints
        public string cardText {get; private set;} // the text displayed in the card
        public CardType cardType {get; private set;} // the type of card we have
        public Transform cardParent = null;
        public bool inSlot = false;
        public Transform deckParent = null;
        private string claimKey = null;
        private string connectionKey = null;
        public bool hasConnectionKey => connectionKey == null;

        public Card(string cardText, CardType cardType, string claimKey, string connectionKey)
        {
            this.cardText = cardText;
            this.cardType = cardType;
            this.claimKey = claimKey;
            this.connectionKey = connectionKey;
        }

        public Card(string cardText, CardType cardType)
        {
            this.cardText = cardText;
            this.cardType = cardType;
        }

        public bool CheckPair(Card otherCard, string claimKey, string claimText, string claimSort)
        {
            if (cardType == CardType.none) return false;
            bool ground_correct = this.claimKey == claimKey;
            bool warrant_correct = otherCard.connectionKey == connectionKey;
            
            if (ground_correct && !warrant_correct) // if warrant error
            {
                // TODO to clean up this code you need to grab the data from outside this function and pass it directly to the error handler, but we don't have time for that
                HigherOrderErrorHandler.AddError(HOErrorType.WARRANT_ERROR, CardMinigameSystem.instance.subjectText.text, claimText, claimSort, cardText, otherCard.cardText);
                return false;
            }
            else if (!ground_correct && warrant_correct) // if ground error
            {
                HigherOrderErrorHandler.AddError(HOErrorType.GROUND_ERROR, CardMinigameSystem.instance.subjectText.text, claimText, claimSort, cardText, otherCard.cardText);
                return false;
            }
            else if (!ground_correct && !warrant_correct) // both are errors
            {
                HigherOrderErrorHandler.AddError(HOErrorType.TOTAL_ERROR, CardMinigameSystem.instance.subjectText.text, claimText, claimSort, cardText, otherCard.cardText);
                return false;
            }

            return true;
        }
    }

    public enum CardType { none, ground, warrant } // since we technically won't be making claim cards, we don't need a type for it
}
