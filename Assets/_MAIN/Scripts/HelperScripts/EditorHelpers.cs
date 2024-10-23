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
            Transform t1 = parent.GetChild(i-1);
            Transform t2 = parent.GetChild(i);
            if (t1.name.CompareTo(t2.name) == -1)
                SwapChildren(t1, t2);
        }
    }

    public static void SortChildrenByNumber(Transform parent)
    {
        for (int i = 1; i < parent.childCount; i++)
        {
            Transform t1 = parent.GetChild(i-1);
            Transform t2 = parent.GetChild(i);

            int n1 = int.Parse(t1.name.Split('[', ']')[1]);
            int n2 = int.Parse(t2.name.Split('[', ']')[1]);
            if (n1 > n2)
            SwapChildren(t1, t2);
        }
    }

    private static void SwapChildren(Transform t1, Transform t2)
    {
        t1.SetSiblingIndex(t2.GetSiblingIndex());
    }

}