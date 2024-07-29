using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public RectTransform tail;
    public RectTransform head;
    private bool isBuilding;
    private bool built = false;

    private TextArchitect architect;
    // Start is called before the first frame update
    void OnEnable()
    {
        architect = new(tmp);
        tmp.text = "";

        LeanTween.scaleY(head.gameObject, 0f, 0f);
        LeanTween.scaleX(tail.gameObject, 0f, 0f);
    }

    public void BuildSpeechBubble()
    {
        isBuilding = true;
        LeanTween.scaleY(head.gameObject, 1f, 0.3f).setEaseInBounce().setOnComplete(() => {
            isBuilding = false;
        }) ;
        LeanTween.scaleX(tail.gameObject, 1f, 0.2f).setEaseInCubic();
        built = true;
    }   

    public IEnumerator Say(string text)
    {
        if (!built)
        {
            BuildSpeechBubble();
            while(isBuilding)
                yield return null;
        }
        yield return architect.Build(text);
    }
}
