using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(CanvasGroup))]
public class LevelLocker : MonoBehaviour
{
    public string levelReferenceRequired = "";
    public bool unlocked = false;
    private Player player => GameSystem.instance.GetLoadedPlayer();
    private CanvasGroup cg;
    private Image image;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
        unlocked = CheckUnlock();
    }

    private bool CheckUnlock()
    {
        if (player.playerScore.ContainsKey(levelReferenceRequired) || levelReferenceRequired == "")
        {
            CanvasGroupControl.HideCanvasGroup(cg);
            return true;
        }
        CanvasGroupControl.ShowCanvasGroup(cg);
        return false;
    }   
}
