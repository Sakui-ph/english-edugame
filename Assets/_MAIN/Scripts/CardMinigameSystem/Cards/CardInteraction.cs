using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CARD_GAME
{
    public class CardInteraction : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private const int DRAG_OUT_THRESHOLD = 100;
        private Canvas canvas;
        private Canvas dragCanvas;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        public Transform parentAfterDrag;
        public bool locked = false;
        public Card card {get; private set; }

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponentInParent<Canvas>();
        }

        public void SetCard(Card card)
        {
            this.card = card;
            
            
            if (card.cardType == CardType.warrant)
                SetCardColor(CardManager.instance.warrantColor);
            else
                SetCardColor(CardManager.instance.groundColor);
        }

        public void SetCardColor(Color color)
        {
            Image cardImage = GetComponentInChildren<Image>();
            cardImage.color = color;
        }

        public void OnPointerDown(PointerEventData eventData) 
        {
            // do nothing...
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
            dragCanvas = gameObject.AddComponent<Canvas>();
            dragCanvas.overrideSorting = true;
            dragCanvas.sortingOrder = 1;

            parentAfterDrag = transform.parent;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            SetParent(parentAfterDrag);
            SnapBackToParent();

            Destroy(dragCanvas);
            
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        private void SnapBackToParent()
        {
            if (card.inSlot)
            {
                RectTransform rt = (RectTransform)transform;
                if (Math.Abs(rt.anchoredPosition.x) >= DRAG_OUT_THRESHOLD || Math.Abs(rt.anchoredPosition.y) >= DRAG_OUT_THRESHOLD) 
                {
                    ResetParent();
                }
                FitToParent();
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)card.deckParent);
        }

        public void FitToParent()
        {

            RectTransform cardRectTrans = (RectTransform)transform;
            cardRectTrans.anchorMax = new Vector2(1,1);
            cardRectTrans.anchorMin = new Vector2(0,0);
            cardRectTrans.localScale = new Vector3(1, 1, 1);
            cardRectTrans.offsetMin = new Vector2(0,0);
            cardRectTrans.offsetMax = new Vector2(0,0);
            if (!card.inSlot)
            {
                LayoutElement le = gameObject.GetComponent<LayoutElement>();
                cardRectTrans.sizeDelta = new Vector2(le.preferredWidth, le.preferredHeight);
            }          
            
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
            if (card.deckParent != parent)
            {
                card.cardParent = parent;
                card.inSlot = true;
            }
            
        }

        public void LockCard()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            locked = true;
        }

        public void ResetParent()
        {   
            if (transform.parent.GetComponentInChildren<CardSlotInteraction>())
            {
                transform.parent.GetComponentInChildren<CardSlotInteraction>().UnoccupySlot();
            }

            transform.SetParent(card.deckParent, false);
            card.cardParent = null;
            card.inSlot = false;
            
        }
    }
}
