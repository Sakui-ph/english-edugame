using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public static class EditorHelpers
{
    public static void SortChildrenByName(Transform parent)
    {
        for (int i = 1; i < parent.childCount; i++)
        {
            SwapChildren(parent.GetChild(i-1), parent.GetChild(i));
        }
    }

    private static void SwapChildren(Transform t1, Transform t2)
    {
        if (t1.name.CompareTo(t2.name) == -1)
            t1.SetSiblingIndex(t2.GetSiblingIndex());
    }
}