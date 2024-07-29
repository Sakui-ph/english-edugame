using UnityEngine;

namespace CARD_GAME
{
    public class Deck : MonoBehaviour
    {
        public CardType cardType;

        public void ShuffleCards()
        {
            foreach (Transform child in transform)
            {
                child.SetSiblingIndex(Random.Range(0, transform.childCount - 1));
            }
        }
    }

}
