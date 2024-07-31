using UnityEngine;

namespace CARD_GAME
{
    public class CardSlot : DroppableSlot
    {
        public CardType expectedOccupantType;

        public override bool IsAllowedType(GameObject droppedObject)
        {
            CardData cardData = droppedObject.GetComponent<Card>().cardData;

            if (cardData.cardType == expectedOccupantType)
                return true;
            return false;
        }
    }
}
