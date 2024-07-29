using System.Collections.Generic;
using CHARACTERS;
using TMPro;
using UnityEngine;

namespace DIALOGUE
{
    [CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "Dialogue System/Dialogue Configuration Asset")]
    public class DialogueSystemConfigurationSO : ScriptableObject
    {
        public CharacterConfigSO characterConfigurationAsset;
        public Color characterDimColor = Color.white;

        public Color defaultDialogueTextColor = Color.white;
        public Color defaultNameTextColor = Color.white;
        public TMP_FontAsset defaultFont;
        public TMP_FontAsset defaultNameFont;
        public int defaultFontSize;
        public int defaultNameFontSize;
        public List<AudioClip> voice = new();
    }
}