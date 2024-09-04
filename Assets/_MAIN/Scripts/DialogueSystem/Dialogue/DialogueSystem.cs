// NOTE: MOST OF THE CODE FROM THE DIALOGUE SYSTEM IS FROM STELLAR STUDIO'S YOUTUBE CHANNEL TUTORIAL
// CHECK THEM OUT HERE: https://www.youtube.com/@stellarstudio5495

using UnityEngine;
using CHARACTERS;
using System.Collections.Generic;
using System;
namespace DIALOGUE 
{
    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField] private DialogueSystemConfigurationSO _config;
        public DialogueSystemConfigurationSO config => _config;
        public DialogueContainer dialogueContainer = new DialogueContainer();
        public TextArchitect architect;
        private ConversationManager conversationManager;
        public bool isRunningConversation => conversationManager.isRunning;
        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPrompt_Next;
        private CharacterManager characterManager => CharacterManager.instance;
        public ChapterManager chapterManager;

        bool _initialized = false;


        void Awake()
        {
            Initialize();
        }

        virtual public void Initialize()
        {   
            if (_initialized)
                return;
            
            _initialized = true;
            HideDialogueBox();
            VisualNovelSL.services.viewController.Hide();
            architect = new TextArchitect(dialogueContainer.dialogueText);
            conversationManager = new ConversationManager(architect);
        }

        public void OnUserPrompt_Next()
        {
            onUserPrompt_Next?.Invoke();
        }

        public void SetSpeakerData(DL_SPEAKER_DATA speaker, DialogueContainer dialogueContainer = null)
        {
            string speakerName = speaker.name;
            string speakerCast = speaker.castName;

            if (!characterManager.CharacterExists(speakerName)) {
                characterManager.CreateCharacter(speakerName);
            }

            // dim everyone else except this guy
            if (characterManager.CharacterExists(speakerName))
            {
                Character speakerCharacter = characterManager.GetCharacter(speakerName);

                if (speakerCharacter is Character_Sprite)
                {
                    characterManager.DimCharacters(config.characterDimColor);
                    if (!speakerCharacter.overriddenColor)
                        speakerCharacter.SetColor(Color.white);
                }
            }
            

            List<(int Layer, string expression)> expressions = null;

            // If we provide a position, only then can we update it
            Vector2 position = new Vector2(speaker.castPosition.X, speaker.castPosition.Y);

            if (speaker.hasExpressionCast) {
                expressions = speaker.CastExpressions;
                ApplyCharacterSpriteExpression(speakerName, expressions);
            }
                

            if (speaker.hasPositionCast) {
                ApplyCharacterViewData(speakerName, position);
            }
            
            ApplySpeakerDataToDialogueContainer(speakerName, dialogueContainer);
        }

        // Take a character and apply new config to the dialogue container
        public void ApplySpeakerDataToDialogueContainer(string speakerName, DialogueContainer dialogueContainer = null)
        {
            Character character = characterManager.GetCharacter(speakerName);
            CharacterConfigData config = character != null ? character.config : characterManager.GetCharacterConfig(speakerName);
            
        
            ApplySpeakerDataToDialogueContainer(config, dialogueContainer);
        }

        public void ApplySpeakerDataToDialogueContainer(CharacterConfigData config, DialogueContainer dialogueContainer = null)
        {
            DialogueContainer speakerContainer = dialogueContainer == null? this.dialogueContainer : dialogueContainer;
            speakerContainer.Clear();
            speakerContainer.nameContainer.Clear();
            speakerContainer.SetDialogueFont(config.dialogueFont);
            speakerContainer.SetDialogueColor(config.dialogueColor);
            speakerContainer.SetDialogueFontSize(config.fontSize);
            speakerContainer.nameContainer.SetNameFont(config.nameFont);
            speakerContainer.nameContainer.SetNameColor(config.nameColor);
            speakerContainer.nameContainer.SetNameFontSize(config.nameFontSize);

            ApplySpeakerVoice(config.voice);
        }

        public void ApplySpeakerVoice(List<AudioClip> voice)
        {
            architect.voiceBank = voice;
        }

        // note = expressions have not been implemented yet
        public void ApplyCharacterViewData(string characterName, Vector2 position)
        {
            Character character = characterManager.GetCharacter(characterName);
            character.SetPosition(position);
        }

        private void ApplyCharacterSpriteExpression(string speakerName, List<(int Layer, string expression)> expressions = null)
        {
            Character character = characterManager.GetCharacter(speakerName);
            if (expressions != null 
            && character.config.characterType == Character.CharacterType.Sprite 
            || character.config.characterType == Character.CharacterType.SpriteSheet)
            {
                Character_Sprite characterSprite = character as Character_Sprite;
                characterSprite.ChangeCharacterSprite(expressions[0].expression, expressions[0].Layer, 3f);
            }
        }

        public void ShowSpeakerName(string speakerName = "", DialogueContainer dialogueContainer = null) 
        {
            DialogueContainer speakerContainer = dialogueContainer == null? this.dialogueContainer : dialogueContainer;
            if (speakerName.ToLower() != "narrator")
                speakerContainer.nameContainer.Show(speakerName);
            else
                HideSpeakerName(speakerContainer);
        }

        public void HideSpeakerName(DialogueContainer dialogueContainer = null){
            DialogueContainer speakerContainer = dialogueContainer == null? this.dialogueContainer : dialogueContainer;
            speakerContainer.nameContainer.Hide();
        }

        public void HideDialogueBox() => dialogueContainer.Hide();
        public void ShowDialogueBox() => dialogueContainer.Show();

        // Playing the chapter
        public virtual void LoadChapter(string chapterName, Action callback = null)
        {
            characterManager.ClearCharacters();
            LowerOrderScoreHandler.Reset();
            HistoryData.Reset();
            
            conversationManager.NullifyObservers();
            chapterManager = new(chapterName);
            GameSystem.instance.cachedChapter = chapterName;
            conversationManager.finishConversation += chapterManager.PlayQueuedChapter;

            if (callback != null)
            {
                ChapterManager.OnChapterEnd += callback;
            }

            chapterManager.PlayChapter();       
        }

        public Coroutine Say(string speaker, string dialogue) 
        {
            List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            return Say(conversation);
        }

        public virtual Coroutine Say(List<string> conversation)
        {
            VisualNovelSL.services.viewController.Show();
            return conversationManager.StartConversation(conversation);
        }
        public void CheckClassTrialAnswer(bool answer)
        {
            LowerOrderScoreHandler.CheckAnswer(answer, conversationManager.isInconsistent);

            // reset the inconsistency checker
            conversationManager.isInconsistent = false;
        }

        public string GetDialogue() => dialogueContainer.dialogueText.text;
        public string GetCurrentSentence() => conversationManager.currentDialogueSentence;
        public void RestartLevel() => conversationManager.ResetConversation();
    }
}