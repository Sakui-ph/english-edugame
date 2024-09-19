using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelMenuButton : MonoBehaviour
{
    public Button button;
    public Image backgroundImage;
    public TextMeshProUGUI levelTitle;
    public TextMeshProUGUI levelScore;

    public void Subscribe(Action action)
    {
        button.onClick.AddListener(() => action.Invoke());
    }
}
