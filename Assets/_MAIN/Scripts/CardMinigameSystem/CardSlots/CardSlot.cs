namespace CARD_GAME
{
    public class CardSlot
    {
        public CardType expectedOccupantType;
        public bool isOccupied = false;
        
        public CardSlot(CardType expectedOccupantType)
        {
            this.expectedOccupantType = expectedOccupantType;
        }
    }
}
