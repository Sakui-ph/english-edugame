using System.Collections.Generic;
using DIALOGUE;
using TMPro;
using UnityEngine;

namespace CHARACTERS
{
    [System.Serializable]
    public class CharacterConfigData
    {
        public string name;
        public string alias;
        public Character.CharacterType characterType;

        public Color nameColor;
        public Color dialogueColor;

        public TMP_FontAsset nameFont;
        public TMP_FontAsset dialogueFont;
        public int fontSize;
        public int nameFontSize;
        public List<AudioClip> voice;

        public CharacterConfigData Clone()
        {
            CharacterConfigData result = new CharacterConfigData();

            result.name = name;
            result.alias = alias;
            result.characterType = characterType;

            result.nameColor = new Color(nameColor.r, nameColor.g, nameColor.b, nameColor.a);
            result.dialogueColor = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, dialogueColor.a);

            result.nameFont = nameFont;
            result.nameFontSize = nameFontSize;
            result.dialogueFont = dialogueFont;
            result.fontSize = fontSize;
            result.voice = voice;

            return result;
        }

        private static Color defaultDialogueTextColor => VisualNovelSL.services.dialogueSystem.config.defaultDialogueTextColor;
        private static Color defaultNameTextColor => VisualNovelSL.services.dialogueSystem.config.defaultNameTextColor;
        private static TMP_FontAsset defaultNameFont => VisualNovelSL.services.dialogueSystem.config.defaultNameFont;
        private static TMP_FontAsset defaultFont => VisualNovelSL.services.dialogueSystem.config.defaultFont;
        private static int defaultFontSize => VisualNovelSL.services.dialogueSystem.config.defaultFontSize;
        private static int defaultNameFontSize => VisualNovelSL.services.dialogueSystem.config.defaultNameFontSize;


        public static CharacterConfigData Default
        {
            get
            {
                CharacterConfigData result = new CharacterConfigData();

                result.name = "";
                result.alias = "";
                result.characterType = Character.CharacterType.Text;

                result.nameColor = new Color(defaultNameTextColor.r, defaultNameTextColor.g, defaultNameTextColor.b, defaultNameTextColor.a);
                result.dialogueColor = new Color(defaultDialogueTextColor.r, defaultDialogueTextColor.g, defaultDialogueTextColor.b, defaultDialogueTextColor.a);

                result.nameFont = defaultNameFont;
                result.dialogueFont = defaultFont;
                result.fontSize = defaultFontSize;
                result.nameFontSize = defaultNameFontSize;
                result.voice = new();

                return result;
            }
        }
    }
}