using UnityEngine;
using UnityEngine.UI;

public class CanvasGroupControl
{
    public static void HideCanvasGroup(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }

    public static void HideCanvasGroup(GameObject go)
    {
        CanvasGroup cg = go.GetComponent<CanvasGroup>();
        HideCanvasGroup(cg);
    }

    public static void ShowCanvasGroup(CanvasGroup cg, bool interactable = true)
    {
        cg.alpha = 1;
        cg.blocksRaycasts = interactable;
        cg.interactable = interactable;
    }

    public static void ShowCanvasGroup(GameObject go)
    {
        CanvasGroup cg = go.GetComponent<CanvasGroup>();
        ShowCanvasGroup(cg);
    }
}
