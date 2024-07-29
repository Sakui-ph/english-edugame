using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CARD_GAME
{
    public class CardSlotInteraction : MonoBehaviour, IDropHandler
    {
        public CardSlot cardSlot = null;
        public static event Action<string> TutorialHelper;
        public bool locked = false;

        public void OnDrop(PointerEventData eventData)
        {
            if (locked)
                return;

            if (cardSlot == null)
            {
                Debug.LogWarning("cardSlot has not been set");
                return;
            }

            if (eventData.pointerDrag == null)
            {
                Debug.LogWarning("Dropped object on CardSlot is null");
                return;
            }

            GameObject cardObject = eventData.pointerDrag;

            if (!cardObject.GetComponent<CardInteraction>())
            {
                Debug.LogWarning("Object dropped lacks a CardInteraction object");
                return;
            }

            CardInteraction cardDropped = cardObject.GetComponent<CardInteraction>();


            if (cardDropped.card.cardType == cardSlot.expectedOccupantType)
            {
                // Check if its in a previous slot
                CardSlotInteraction previousSlot = null;

                if (cardDropped.card.inSlot)
                    previousSlot = cardDropped.GetComponentInParent<CardSlotInteraction>();

                // If transferring to an occupied slot
                if (cardSlot.isOccupied)
                {
                    MoveToOccupiedSlot(previousSlot);
                }

                // If transferring to a non occupied slot
                if (!cardSlot.isOccupied)
                {
                    MoveToUnoccupiedSlot(previousSlot);
                }
                TutorialHelper?.Invoke(cardDropped.card.cardText);
                cardDropped.parentAfterDrag = transform;
                cardDropped.FitToParent();
            }

            
        }

        private void MoveToOccupiedSlot(CardSlotInteraction previousSlot = null)
        {
            CardInteraction currentCard = GetComponentInChildren<CardInteraction>();

            // If the parent is a CardSlot, replace
            if (previousSlot != null)
            {
                currentCard.SetParent(previousSlot.transform);
            } 
            // If the parent isn't a CardSlot, reset to deck
            else 
            {
                currentCard.ResetParent();
            }
            currentCard.FitToParent();
        }

        private void MoveToUnoccupiedSlot(CardSlotInteraction previousSlot = null)
        {
            // If it was in a previous slot, deoccupy it
            if (previousSlot != null)
            {
                previousSlot.UnoccupySlot();
            }
            cardSlot.isOccupied = true;
        }

        public void UnoccupySlot()
        {
            cardSlot.isOccupied = false;
        }

        public void Lock()
        {

            locked = true;
            GetOccupyingCardUI().LockCard();
        }

        public CardInteraction GetOccupyingCardUI()
        {
            // GameObject testObject = Instantiate(gameObject, transform);
            // testObject.name = "problemChild";
            // foreach(GameObject child in transform)
            // {

            //     Debug.Log(child.name);
            // }
            if (cardSlot.isOccupied)
                return GetComponentInChildren<CardInteraction>();
            else return null;
        }
    }
}
