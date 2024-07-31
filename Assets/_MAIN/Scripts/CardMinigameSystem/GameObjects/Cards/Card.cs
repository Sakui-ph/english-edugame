using System;
using TMPro;
using UnityEngine;

namespace CARD_GAME
{
    public class Card : DraggableItem
    {
        public CardData cardData;

        public override void Start()
        {
            TextMeshProUGUI[] TMPs = GetComponentsInChildren<TextMeshProUGUI>();
            TMPs[0].text = cardData.cardText;
            base.Start();
        }
    }
}
