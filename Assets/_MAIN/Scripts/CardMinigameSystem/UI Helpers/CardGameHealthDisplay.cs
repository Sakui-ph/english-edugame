using System;
using UnityEngine;
using UnityEngine.UI;

namespace CARD_GAME
{
    public class CardGameHealthDisplay : MonoBehaviour
    {
        [SerializeField] GameObject healthSprite;
        private int spriteCount => transform.childCount;


        void Start()
        {
            if (CardMinigameSystem.instance.config.healthSprite != null)
                healthSprite.GetComponent<Image>().sprite = CardMinigameSystem.instance.config.healthSprite;
        }

        public void OnHealthChange(int amount)
        {
            Debug.Log("Health change detected");
            UpdateHealth(amount);
        }

        private void UpdateHealth(int health)
        {
            Debug.Log( Math.Abs(spriteCount - health));
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
            Instantiate(healthSprite, transform);
        }

        private void DestroyHealthSprite()
        {
            if (spriteCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }
}