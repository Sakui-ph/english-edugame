using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

namespace CHARACTERS
{
    public class CharacterSpriteLayer
    {
        private CharacterManager characterManager => CharacterManager.instance; 

        private const float DEFAULT_TRANSITION_SPEED = 3f;
        private float transitionSpeedMultiplier = 1;
        public int layer { get; private set;} = 0;
        public Image renderer { get; private set; } = null;
        public CanvasGroup rendererCG => renderer.GetComponent<CanvasGroup>();

        private List<CanvasGroup> oldRenderers = new List<CanvasGroup>();

        private Coroutine thread_transitioning = null;
        private Coroutine thread_levelingAlpha = null;
        private Coroutine co_flipping = null;
        private Coroutine thread_color_changing;
        public bool isChangingColor => thread_color_changing != null;
        public bool isTransitioningLayer => thread_transitioning != null;
        public bool isLevelingAlpha => thread_levelingAlpha != null;
        public bool isFlipping => co_flipping != null;
        private bool isFacingLeft = Character.DEFAULT_ORIENTATION_IS_FACING_LEFT;

        public CharacterSpriteLayer(Image defaultRenderer, int layer = 0)
        {
            this.layer = layer;
            renderer = defaultRenderer;
        }

        public void SetSprite(Sprite sprite)
        {
            renderer.sprite = sprite;
        }

        public Coroutine TransitionSprite(Sprite sprite, float speed = 1)
        {
            if (sprite == renderer.sprite)
                return null;

            if (isTransitioningLayer)
            {
                characterManager.StopCoroutine(thread_transitioning);
            }

            thread_transitioning = characterManager.StartCoroutine(TransitioningSprite(sprite, speed));
            return thread_transitioning;
        }

        private IEnumerator TransitioningSprite(Sprite sprite, float speedMultiplier = 1)
        {
            transitionSpeedMultiplier = speedMultiplier;
            Image newRenderer = CreateRenderer(renderer.transform.parent);
            newRenderer.sprite = sprite;

            yield return TryStartLevelingALphas();

            thread_transitioning = null;
        }

        private Image CreateRenderer(Transform parent)
        {
            Image newRenderer = Object.Instantiate(renderer, parent);
            oldRenderers.Add(rendererCG);

            newRenderer.name = renderer.name;
            renderer = newRenderer;
            renderer.gameObject.SetActive(true);
            rendererCG.alpha = 0;

            return newRenderer;
        }

        private Coroutine TryStartLevelingALphas()
        {
            if (isLevelingAlpha)
                return thread_levelingAlpha;

            thread_levelingAlpha = characterManager.StartCoroutine(RunAlphaLeveling());
            return thread_levelingAlpha;
        }

        private IEnumerator RunAlphaLeveling()
        {
            while (rendererCG.alpha < 1 || oldRenderers.Any(oldCG => oldCG.alpha > 0))
            {
                float speed = DEFAULT_TRANSITION_SPEED * transitionSpeedMultiplier * Time.deltaTime;
                rendererCG.alpha = Mathf.MoveTowards(rendererCG.alpha, 1, speed);

                for (int i = oldRenderers.Count - 1; i >= 0; i--)
                {
                    CanvasGroup oldCG = oldRenderers[i];
                    oldCG.alpha = Mathf.MoveTowards(oldCG.alpha, 0, speed);
                    if (oldCG.alpha == 0)
                    {
                        oldRenderers.RemoveAt(i);
                        Object.Destroy(oldCG.gameObject);
                    }
                }

                yield return null;
            }

            thread_levelingAlpha = null;
        }

        public Coroutine FaceLeft(float speed = 1f, bool immediate = false)
        {
            if (isFlipping)
                characterManager.StopCoroutine(co_flipping);

            isFacingLeft = true;
            co_flipping = characterManager.StartCoroutine(FaceDirection(isFacingLeft, speed, immediate));   
            return co_flipping;
        }

        public Coroutine FaceRight(float speed = 1f, bool immediate = false)
        {
            if (isFlipping)
                characterManager.StopCoroutine(co_flipping);

            isFacingLeft = false;
            co_flipping = characterManager.StartCoroutine(FaceDirection(isFacingLeft, speed, immediate));   
            return co_flipping;
        }


        private IEnumerator FaceDirection(bool faceLeft, float speedMultiplier, bool immediate)
        {
            float xScale = faceLeft ? 1 : -1;
            Vector3 newScale = new Vector3(xScale, 1, 1);

            if (!immediate)
            {
                Image newRenderer = CreateRenderer(renderer.transform.parent);

                newRenderer.transform.localScale = newScale;
                transitionSpeedMultiplier = speedMultiplier;
                TryStartLevelingALphas();

                while(isLevelingAlpha)
                    yield return null;
            }
            else
            {
                renderer.transform.localScale = newScale;
            }

            co_flipping = null;
        }

        public void SetColor(Color color)
        {
            renderer.color = color;

            foreach(CanvasGroup oldCG in oldRenderers)
            {
                oldCG.GetComponent<Image>().color = color;
            }
        }

        public Coroutine TransitionColor(Color color, float speed = 1f)
        {
            if (isChangingColor)
                characterManager.StopCoroutine(thread_color_changing);

            thread_color_changing = characterManager.StartCoroutine(ChangingColor(color, speed));
            return thread_color_changing;
        }

        private IEnumerator ChangingColor(Color color, float speedMultiplier)
        {
            Color oldColor = renderer.color;
            List<Image> oldImages = new();

            foreach (var oldCG in oldRenderers)
            {
                oldImages.Add(oldCG.GetComponent<Image>());
            }

            float colorPercent = 0;
            while (colorPercent < 1)
            {
                colorPercent += DEFAULT_TRANSITION_SPEED * speedMultiplier * Time.deltaTime;

                renderer.color = Color.Lerp(oldColor, color, colorPercent);

                foreach(Image oldImage in oldImages)
                {
                    oldImage.color = renderer.color;
                }

                yield return null;
            }
            thread_color_changing = null;
        }
            
    }
}
