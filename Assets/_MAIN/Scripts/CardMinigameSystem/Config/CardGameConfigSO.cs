using UnityEngine;

namespace CARD_GAME
{
    [CreateAssetMenu(fileName = "Card Minigame Configuration", menuName = "Card Minigame/Card Minigame Configuration")]
    public class CardGameConfigSO : ScriptableObject
    {
        [Header("Health Settings")]
        public int startingHP = 3;
        public Sprite healthSprite;
    }

}
