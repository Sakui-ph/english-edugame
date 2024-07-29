using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    private List<TabGroupButton> tabButtons;
    public List<GameObject> objectsToSwap;
    public TabGroupButton firstSelectedTab;
    public bool useSprites = false;
    [Header("Color Options")]
    public Color tabIdleColor;
    public Color tabHoverColor;
    public Color tabActiveColor;

    [Header("Sprite Options")]
    public Sprite tabIdleSprite;
    public Sprite tabHoverSprite;
    public Sprite tabActiveSprite;

    [Header("Additional Options")]
    public ScrollRect scrollRect = null;
    private TabGroupButton selectedTab;

    public event Action<int> OnSwitch;
    public static event Action<string> TutorialHelper;

    public void Subscribe(TabGroupButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabGroupButton>();
            SelectTab(button);
        }

        if (button == firstSelectedTab)
        {
            SelectTab(button);
        }

        tabButtons.Add(button);
        ResetActiveChildren();
    }
    
    public void OnTabEnter(TabGroupButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
            SetButtonHover(button);
    }

    public void OnTabExit(TabGroupButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabGroupButton button)
    {
        SelectTab(button);
        ResetActiveChildren();
        TutorialHelper?.Invoke(button.GetComponentInChildren<TextMeshProUGUI>().text);
    }   

    private void SelectTab(TabGroupButton button)
    {
        selectedTab = button;
        OnSwitch?.Invoke(button.index);
    }

    private void ResetActiveChildren()
    {
        ResetTabs();
        SetButtonActive(selectedTab);

        int index = selectedTab.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
                if (scrollRect != null)
                {
                    scrollRect.content = (RectTransform)objectsToSwap[i].transform;
                }
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(TabGroupButton button in tabButtons)
        {
            if(selectedTab != null && button == selectedTab) continue;

            SetButtonIdle(button);
        }
    }

    private void SetButtonIdle(TabGroupButton button)
    {
        if (useSprites) {
            button.background.sprite = tabIdleSprite;
            return;
        }
            
        button.background.color = tabIdleColor;
    }

    private void SetButtonHover(TabGroupButton button)
    {
        if (useSprites) {
            button.background.sprite = tabHoverSprite;
            return;
        }
            
        button.background.color = tabHoverColor;
    }

    private void SetButtonActive(TabGroupButton button)
    {
        if (useSprites) {
            button.background.sprite = tabActiveSprite;
            return;
        }
            
        button.background.color = tabActiveColor;
    }
}
