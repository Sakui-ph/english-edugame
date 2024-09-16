using System;
using AUDIO_SYSTEM;
using UnityEngine;
using UnityEngine.UI;

namespace CARD_GAME
{
    public class CardGameHealthDisplay : MonoBehaviour
    {
        private CardMinigameSystem system => CardMinigameSystem.instance;
        [SerializeField] GameObject healthSprite;
        private int spriteCount => transform.childCount;


        void Start()
        {
            if (CardMinigameSystem.instance.config.healthSprite != null)
                healthSprite.GetComponent<Image>().sprite = system.config.healthSprite;
        }

        public void OnHealthChange(int amount)
        {
            UpdateHealth(amount);
        }

        private void UpdateHealth(int health)
        {
            for (int i = 0; i <= Math.Abs(spriteCount - health); i++)
            {
                if (spriteCount > health)
                    DestroyHealthSprite();
                else if (spriteCount < health)
                    InstantiateHealthSprite();
            }
        }

        private void InstantiateHealthSprite()
        {   
            GameSystemSL.services.audioManager.PlaySoundEffect(system.correctSound);
            Instantiate(healthSprite, transform);
        }

        private void DestroyHealthSprite()
        {
            GameSystemSL.services.audioManager.PlaySoundEffect(system.incorrectSound);
            if (spriteCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }
}