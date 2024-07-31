using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using TMPro;
using UnityEngine;

namespace CHARACTERS
{
    public abstract class Character
    {
        public const bool DEFAULT_ORIENTATION_IS_FACING_LEFT = true;
        public const string ANIM_REFRESH_TRIGGER = "Refresh";

        public string name = "";
        public string displayName = "";
        public bool overriddenColor = false;
        public RectTransform root = null;
        public Animator animator = null;
        public Color color {get; protected set;} = Color.white;
        public CharacterConfigData config;

        protected CharacterManager characterManager => CharacterManager.instance;
        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        protected bool facingLeft = DEFAULT_ORIENTATION_IS_FACING_LEFT;

        // Coroutines
        protected Coroutine thread_revealing, thread_hiding, thread_color_changing;
        protected Coroutine thread_moving;
        protected Coroutine thread_flipping;

        public bool isChangingColor => thread_color_changing != null;
        public bool isMoving => thread_moving != null;
        public bool isRevealing => thread_revealing != null;
        public bool isHiding => thread_hiding != null;
        public bool isFlipping => thread_flipping != null;
        public virtual bool isVisible {get; set;}

        public bool isFacingLeft => facingLeft;
        public bool isFacingRight => !facingLeft;

        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            this.displayName = name;
            this.config = config;

            if (prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, characterManager.CharacterPanel);
                ob.name = characterManager.FormatCharacterPath(characterManager.characterPrefabName, name);
                ob.SetActive(true);
                root = ob.GetComponent<RectTransform>();
                animator = root.GetComponentInChildren<Animator>();
            }
        }

        public Coroutine Say(string dialogue) => Say(new List<string> {{$"\"{dialogue}\""}});

        public Coroutine Say(List<string> conversation)
        {
            dialogueSystem.dialogueContainer.Clear();
            dialogueSystem.dialogueContainer.nameContainer.Clear();
            dialogueSystem.ShowSpeakerName(displayName);
            dialogueSystem.ApplySpeakerDataToDialogueContainer(config);
            return dialogueSystem.Say(conversation);
        }

        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;
        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void SetDialogueColor(Color color) => config.dialogueColor = color;

        public void UpdateTextCustomizationsOnView(CharacterConfigData config) => dialogueSystem.ApplySpeakerDataToDialogueContainer(config);
        public void ResetConfigurationData() => config = CharacterManager.instance.GetCharacterConfig(name);

        public virtual Coroutine Show()
        {
            if (isRevealing)
                return thread_revealing;
            
            if (isHiding)
                characterManager.StopCoroutine(thread_hiding);

            thread_revealing = characterManager.StartCoroutine(ShowingOrHiding(true));
            return thread_revealing;
        }

        public virtual Coroutine Hide()
        {
            if (isHiding)
                return thread_hiding;
            if (isRevealing)
                characterManager.StopCoroutine(thread_revealing);

            thread_hiding = characterManager.StartCoroutine(ShowingOrHiding(false));
            return thread_hiding;
        }

        // Different for each character type
        public virtual IEnumerator ShowingOrHiding(bool show)
        {
            Debug.LogError("ShowingOrHiding() not implemented for base character type.");
            yield return null;
        }

        public virtual void SetColor(float red, float green, float blue, float alpha)
        {
            SetColor(new Color(red, green, blue, alpha));
        }

        public virtual void SetColor(Color color)
        {
            this.color = color;
        }

        public Coroutine TransitionColor(float red, float green, float blue, float alpha, float speed = 1f)
        {
            thread_color_changing = TransitionColor(new Color(red, green, blue, alpha), speed);
            return thread_color_changing;
        }

        public Coroutine TransitionColor(Color color, float speed = 1f)
        {
            this.color = color;
            if (isChangingColor)
            {
                characterManager.StopCoroutine(thread_color_changing);
            }

            thread_color_changing = characterManager.StartCoroutine(ChangingColor(color, speed));
            return thread_color_changing;
        }

        public virtual Coroutine ChangeCharacterSprite(string expressionName, int layer, float speed = 1f)
        {
            Debug.LogError("ChangeCharacterSprite() not implemented for base character type");
            return null;
        }

        protected virtual IEnumerator ChangingColor(Color color, float duration)
        {
            Debug.LogError("ChangingColor() not implemented for base character type.");
            yield return null;
        }

        public Coroutine Flip(bool immediate = false)
        {
            if (isFacingLeft)
                return FaceRight(immediate: immediate);
            else
                return FaceLeft(immediate: immediate);
        }

        public Coroutine FaceLeft(float speed = 1f, bool immediate = false)
        {
            if (isFlipping) 
            {
                characterManager.StopCoroutine(thread_flipping);
            }

            facingLeft = true;
            thread_flipping = characterManager.StartCoroutine(FaceDirection(facingLeft, speed, immediate));
            return thread_flipping;
        }

        public Coroutine FaceRight(float speed = 1f, bool immediate = false)
        {
            if (isFlipping) 
            {
                characterManager.StopCoroutine(thread_flipping);
            }
            facingLeft = false;
            thread_flipping = characterManager.StartCoroutine(FaceDirection(facingLeft, speed, immediate));
            return thread_flipping;
        }
        
        public virtual IEnumerator FaceDirection(bool faceLeft, float speedMultiplier, bool immediate)
        {
            Debug.LogError("FacingLeft() not implemented for base character type.");
            yield return null;
        }


        // trigger
        public void Animate(string animation) 
        {
            animator.SetTrigger(animation);
        }

        // state
        public void Animate(string animation, bool state) 
        {
            animator.SetBool(animation, state);
            animator.SetTrigger(ANIM_REFRESH_TRIGGER);
        }

        public virtual void SetPosition(Vector2 position)
        {
            // Operates off of the anchor positions
            // 0,0 is bottom left corner, 1,1 is top right corner

            // Convert the Vector2 position into coordinates that we can use and apply relative to the character position
            if (root == null)
                return;

            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);

            // sets our position on screen
            root.anchorMin = minAnchorTarget;
            root.anchorMax = maxAnchorTarget;
        }

        public virtual Coroutine MoveToPosition(Vector2 position, float speed = 2f)
        {
            if (root == null)
                return null;
            if (isMoving)
                characterManager.StopCoroutine(thread_moving);

            thread_moving = characterManager.StartCoroutine(MovingToPosition(position, speed));
            return thread_moving;
        }

        public virtual Coroutine SmoothMoveToPosition(Vector2 position, float duration = 1f)
        {
            if (root == null)
                return null;
            if (isMoving)
                characterManager.StopCoroutine(thread_moving);

            thread_moving = characterManager.StartCoroutine(SmoothMovingToPosition(position, duration));
            return thread_moving;
        }

        private IEnumerator MovingToPosition(Vector2 position, float speed)
        {
            // The position we need to move to
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);
            Vector2 padding = root.anchorMax - root.anchorMin;

            while (root.anchorMin != minAnchorTarget || root.anchorMax != maxAnchorTarget)
            {
                root.anchorMin = Vector2.MoveTowards(root.anchorMin, minAnchorTarget, Time.deltaTime * speed * 0.35f);

                root.anchorMax = root.anchorMin + padding;
                yield return null;
            }

            thread_moving = null;
        }

        // FIX THIS TODO
        private IEnumerator SmoothMovingToPosition(Vector2 position, float duration)
        {
            float startTime = Time.time;
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);
            Vector2 padding = root.anchorMax - root.anchorMin;

            while (root.anchorMin != minAnchorTarget || root.anchorMax != maxAnchorTarget)
            {
                Vector2 velocity = Vector2.one;
                float t = (Time.time - startTime) / duration;
                root.anchorMin = Vector2.Lerp(root.anchorMin, minAnchorTarget, Mathf.SmoothStep(0.0f, 1.0f, t));

                root.anchorMax = root.anchorMin + padding;
                yield return null;
            }

            thread_moving = null;
        }

        protected (Vector2, Vector2) ConvertUITargetPositionToRelativeCharacterAnchorTargets(Vector2 position)
        {
            // How thick and tall is the character?
            Vector2 padding = root.anchorMax - root.anchorMin;

            // What is the maximum X and Y that we can go to? Use 1f because we're working on a normalized range
            float maxX = 1f - padding.x;
            float maxY = 1f - padding.y;

            // Minimum anchor target takes the 
            Vector2 minAnchorTarget = new Vector2(maxX * position.x, maxY * position.y);
            Vector2 maxAnchorTarget = minAnchorTarget + padding;

            return (minAnchorTarget, maxAnchorTarget);
        }

        public enum CharacterType 
        {
            Text,
            Sprite,
            SpriteSheet,
            Live2D,
            Model3D
        }
    }

    
}
