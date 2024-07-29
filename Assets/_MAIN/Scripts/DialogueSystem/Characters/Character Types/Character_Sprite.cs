using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CHARACTERS
{
    public class Character_Sprite : Character
    {
        private const string SPRITE_RENDERED_PARENT_NAME = "Renderer";
        private const string SPRITESHEET_DEFAULT_SHEETNAME = "Default";
        private const char SPRITESHEET_DELIMITER = '-';

        private CanvasGroup rootCG => root.GetComponent<CanvasGroup>();
        private Image spriteImage => root.GetComponentInChildren<Image>();

        public List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();

        private RectTransform imageTransform => spriteImage.gameObject.GetComponent<RectTransform>();
        public override bool isVisible { get => rootCG.alpha > 0; set => rootCG.alpha = value ? 1 : 0; }

        private string artAssetsDirectory = "";

        public Character_Sprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
        {
            rootCG.alpha = 0;
            artAssetsDirectory = rootAssetsFolder + "/Images";

            GetLayers();

            Debug.Log($"Created Sprite Character: {name}");
        }

        private void GetLayers()
        {
            Transform rendererRoot = animator.transform.Find(SPRITE_RENDERED_PARENT_NAME);

            if (rendererRoot == null)
                return;

            for (int i = 0; i < rendererRoot.transform.childCount; i++)
            {
                Transform child = rendererRoot.transform.GetChild(i);
                Image rendererImage = child.GetComponent<Image>();
                
                if (rendererImage != null)
                {
                    CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage, i);
                    layers.Add(layer);
                    child.name = $"Layer: {i}";
                }
            }
        }

        public void SetSprite(Sprite sprite, int layer = 0)
        {
            if (layers.Count == 0)
                return;

            layers[layer].SetSprite(sprite);
        }

        public Sprite GetSprite(string spriteName)
        {
            if (config.characterType == CharacterType.SpriteSheet)
            {
                string[] data = spriteName.Split(SPRITESHEET_DELIMITER);
                Sprite[] spriteArray;
                if (data.Length == 2)
                {
                    string textureName = data[0];
                    spriteName = data[1];
                    spriteArray = Resources.LoadAll<Sprite>($"{artAssetsDirectory}/{textureName}");
                }
                else
                {
                    // for default char sprite sheets
                    spriteArray = Resources.LoadAll<Sprite>($"{artAssetsDirectory}/{SPRITESHEET_DEFAULT_SHEETNAME}");
                }

                if (spriteArray.Length == 0)
                {
                    Debug.LogError($"Character {name} does not have a default art asset called {SPRITESHEET_DEFAULT_SHEETNAME}");
                }

                

                return Array.Find(spriteArray, sprite => sprite.name == spriteName);
            }
            else
            {
                Sprite sprite = Resources.Load<Sprite>($"{artAssetsDirectory}/{spriteName}");
                
                if (sprite == null)
                    Debug.LogError($"Character {name} does not have a sprite called {spriteName}");

                return sprite;
            }
        }

        public override void SetColor(float red, float green, float blue, float alpha)
        {
            base.SetColor(new Color(red, green, blue, alpha));
            SetColor(new Color(red, green, blue, alpha));
        }

        public override void SetColor(Color color)
        {
            base.SetColor(color);
            foreach(CharacterSpriteLayer layer in layers)
            {
                layer.SetColor(color);
            }
        }

        protected override IEnumerator ChangingColor(Color color, float speed = 1f)
        {
            foreach (CharacterSpriteLayer layer in layers)
            {
                layer.TransitionColor(color, speed);
            }

            yield return null;

            while(layers.Any(l => l.isChangingColor))
            {
                yield return null;
            }

            thread_color_changing = null;
        }

        public override Coroutine ChangeCharacterSprite(string expressionName, int layer, float speed = 1)
        {
            Sprite sprite = GetSprite(expressionName);
            return TransitionSprite(sprite, layer, speed);
        }

        public Coroutine TransitionSprite(Sprite sprite, int layer = 0, float speed = 1)
        {
            CharacterSpriteLayer targetLayer = layers[layer];
            return targetLayer.TransitionSprite(sprite, speed);
        }

        public override IEnumerator ShowingOrHiding(bool show)
        {
            float targetAlpha = show ? 1f : 0;
            CanvasGroup self = rootCG;
            
            while (self.alpha != targetAlpha)
            {
                self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, Time.deltaTime * 2f);
                yield return null;
            }

            // clear our the coroutines
            thread_revealing = null;
            thread_hiding = null;
        }

        public override IEnumerator FaceDirection(bool faceLeft, float speedMultiplier, bool immediate)
        {
            foreach (CharacterSpriteLayer layer in layers)
            {
                if (faceLeft)
                    layer.FaceLeft(speedMultiplier, immediate);
                else
                    layer.FaceRight(speedMultiplier, immediate);
            }

            Debug.Log("Try flipping");

            yield return null;

            while(layers.Any(l => l.isFlipping))
                yield return null;

            thread_flipping = null;
        }


    }
}