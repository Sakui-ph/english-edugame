using System;
using UnityEngine;
using UnityEngine.UI;

namespace CARD_GAME
{
    public class CardGameHealthDisplay : MonoBehaviour
    {
        CardGamePlayerDataManager cardGamePlayerData => CardGamePlayerDataManager.instance;
        [SerializeField] GameObject healthSprite;
        private int spriteCount => transform.childCount;


        void Start()
        {
            cardGamePlayerData.HealthUpdate += HealthUpdate;

            cardGamePlayerData.initialHP = CardMinigameSystem.instance.config.startingHP;

            if (CardMinigameSystem.instance.config.healthSprite != null)
                healthSprite.GetComponent<Image>().sprite = CardMinigameSystem.instance.config.healthSprite;

            SetupInitialHP(cardGamePlayerData.initialHP);
        }

        private void HealthUpdate(int playerHealth)
        {
            if (spriteCount < playerHealth)
                IncreaseHealthSprite();
            if (spriteCount > playerHealth)
                DecreaseHealthSprite();
        }

        private void SetupInitialHP(int currentHP)
        {
            while (spriteCount < currentHP)
            {
                IncreaseHealthSprite();
            }
        }

        private void DecreaseHealthSprite()
        {
            if (spriteCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }

        private void IncreaseHealthSprite()
        {
            if (spriteCount != cardGamePlayerData.maxHP)
                Instantiate(healthSprite, transform);
        }
    }
}