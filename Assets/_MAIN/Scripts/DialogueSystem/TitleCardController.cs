using AUDIO_SYSTEM;
using TMPro;
using UnityEngine;

public class TitleCardController : MonoBehaviour
{
    private const string SOUND = "PaperFlip";
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI header_TMP;
    public TextMeshProUGUI title_TMP;

    public void SetHeaderText(string text) => header_TMP.text = text;
    public void SetTitleText(string text) => title_TMP.text = text;

    void Awake()
    {
        canvasGroup.alpha = 0f;
    }

    public void ShowCard()
    {
        canvasGroup.alpha = 0f;

        float duration =0.3f;

        AudioManager.instance.PlaySoundEffect(SOUND);

        LeanTween.alphaCanvas(canvasGroup, 1.0f, duration).setDelay(0f);
    }

    public void HideCard()
    {
        canvasGroup.alpha = 1f;

        float duration = 1f;
        AudioManager.instance.PlaySoundEffect(SOUND);
        LeanTween.alphaCanvas(canvasGroup, 0.0f, duration).setDelay(1f);
    }
}