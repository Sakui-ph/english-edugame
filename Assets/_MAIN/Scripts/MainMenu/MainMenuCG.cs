using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Video;


namespace MAIN_MENU
{   
    [System.Serializable]
    public class MainMenuCG : MonoBehaviour
    {
        private Vector2 vec2One = Vector2.one;
        public CanvasGroup bg;  
        public RectTransform flowerRoot;
        public GameObject flowersPrefab;
        public RectTransform logo;
        public RectTransform play;
        public RectTransform settings;
        public RectTransform credits;
        public RectTransform quit;
        public RectTransform production;
        public RectTransform characters;
        private bool isFirstStart => GameSystem.instance.isFirstStart;
        void Start()
        {
            if (isFirstStart)
            {
                bg.alpha = 0;
                logo.localScale = new Vector2(0,0);
                play.localScale = new Vector2(0,0);
                settings.localScale = new Vector2(0,0);
                credits.localScale = new Vector2(0,0);
                quit.localScale = new Vector2(0,0);
                production.localScale = new Vector2(0,0);
                characters.localScale = new Vector2(0,0);
            } 
            else
                Instantiate(flowersPrefab, flowerRoot);

        }

        public void Animate()
        {
            CanvasGroupControl.ShowCanvasGroup(bg);
            ScaleRect(logo, 1f);
            ScaleRect(play, 2f);
            ScaleRect(settings, 2f);
            ScaleRect(credits, 2.2f);
            ScaleRect(quit, 2.3f);
            LeanTween.delayedCall(1.9f, () => {
                Instantiate(flowersPrefab, flowerRoot);
                GameSystem.instance.isFirstStart = false;
            });
            ScaleRect(characters, 2.3f);
            ScaleRect(production, 2.4f);
            
        }

        private void ScaleRect(RectTransform rect, float time = 1f, Action callback = null)
        {
            if (callback == null)
            {
                LeanTween.scale(rect, vec2One, time).setEaseOutBounce();
                return;
            }
            LeanTween.scale(rect, vec2One, time).setEaseOutBounce().setOnComplete(callback);
        }
    }
}
