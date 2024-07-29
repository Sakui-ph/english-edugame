using UnityEngine;
using TMPro;
using CHARACTERS;

namespace DIALOGUE 
{
    [System.Serializable]
    public class DialogueContainer
    {
        public GameObject root;
        public NameContainer nameContainer = new NameContainer();
        public TextMeshProUGUI dialogueText;

        public void SetDialogueColor(Color color) => dialogueText.color = color;
        public void SetDialogueFont(TMP_FontAsset font) => dialogueText.font = font;
        public void SetDialogueFontSize(int size) => dialogueText.fontSize = size;
        public void Hide() => root.SetActive(false);
        public void Show() => root.SetActive(true);
        public void Clear() => dialogueText.text = string.Empty;
    }
}