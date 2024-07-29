using TMPro;
using UnityEngine;

namespace DIALOGUE 
{
    [System.Serializable]
    public class NameContainer
    {
        [SerializeField] GameObject root;
        [SerializeField] private TextMeshProUGUI nameText;

        public void SetNameColor(Color color) => nameText.color = color;
        public void SetNameFont(TMP_FontAsset font) => nameText.font = font;
        public void SetNameFontSize(int size) => nameText.fontSize = size;
        public void Clear() => nameText.text = string.Empty;

        public void Show(string speakerName = "") 
        {
            root.SetActive(true);
            if(speakerName != string.Empty)
                nameText.text = speakerName;
        }

        public void Hide() 
        {
            root.SetActive(false);
        }


    }   
}

