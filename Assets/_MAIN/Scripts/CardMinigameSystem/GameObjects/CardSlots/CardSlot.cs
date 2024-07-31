using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TUTORIAL_MANAGER;

namespace CARD_GAME
{
    public class CardSlot : DroppableSlot, ITutorialHelper
    {
        public CardType expectedOccupantType;
        public static event Action<string> TutorialHelper;

        public override bool IsAllowedType(GameObject droppedObject)
        {
            CardData cardData = droppedObject.GetComponent<Card>().cardData;

            if (cardData.cardType == expectedOccupantType)
                return true;
            return false;
        }

        public override void OnDrop(PointerEventData eventData)
        {
            base.OnDrop(eventData);
            TutorialHelper?.Invoke(eventData.pointerDrag.GetComponent<Card>().cardData.cardText);
        }
    }
}
