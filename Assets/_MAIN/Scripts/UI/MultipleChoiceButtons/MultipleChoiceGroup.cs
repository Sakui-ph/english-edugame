using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceGroup : MonoBehaviour
{
    private List<MultipleChoiceButton> multipleChoiceButtons;
    public bool useSprites = false;
    [Header("Color Options")]
    public Color tabIdleColor;
    public Color tabHoverColor;
    public Color tabActiveColor;

    [Header("Sprite Options")]
    public Sprite tabIdleSprite;
    public Sprite tabHoverSprite;
    public Sprite tabActiveSprite;
    public MultipleChoiceButton activeButton;
    private bool isLocked = false;


    public void Subscribe(MultipleChoiceButton button)
    {
        if (multipleChoiceButtons == null)
        {
            multipleChoiceButtons = new();
        }

        multipleChoiceButtons.Add(button);
    }

    public void Unsubscribe(MultipleChoiceButton button)
    {
        multipleChoiceButtons.Remove(button);
    }

    public void SetAllIdle()
    {
        activeButton = null;
        foreach (MultipleChoiceButton button in multipleChoiceButtons)
            OnExit(button);
    }

    public void Lock()
    {
        if (!isLocked)
        {   
            isLocked = true;
            DisableOtherButtons();
        }
        
    }

    public void Unlock()
    {
        if (isLocked)
        {
            isLocked = false;
            ResetButtons();
        }
        
    }

    private void ResetButtons()
    {
        foreach (var button in multipleChoiceButtons)
        {
            button.EnableButton();
        }
    }


    private void DisableOtherButtons(MultipleChoiceButton selectedButton)
    {
        foreach (var button in multipleChoiceButtons)
        {
            if (button != selectedButton)
                button.DisableButton();
        }
    }

    private void DisableOtherButtons()
    {
        foreach (var button in multipleChoiceButtons)
        {
            button.DisableButton();
        }
    }

    public void OnEnter(MultipleChoiceButton button)
    {
        if (button != activeButton)
            SetButtonHover(button);
    }

    public void OnExit(MultipleChoiceButton button)
    {
        if (button != activeButton)
            SetButtonIdle(button);
        else if (button == activeButton)
            SetButtonActive(activeButton);
    }

    public void OnClick(MultipleChoiceButton button)
    {
        if (activeButton != null)
            SetButtonIdle(activeButton);
        activeButton = button;
        SetButtonActive(button);
    }


    public List<MultipleChoiceButton> GetButtons()
    {
        return multipleChoiceButtons;
    }

    private void SetButtonIdle(MultipleChoiceButton button)
    {
        if (useSprites) {
            button.background.sprite = tabIdleSprite;
            return;
        }
            
        button.background.color = tabIdleColor;
    }

    private void SetButtonHover(MultipleChoiceButton button)
    {
        if (useSprites) {
            button.background.sprite = tabHoverSprite;
            return;
        }
            
        button.background.color = tabHoverColor;
    }

    private void SetButtonActive(MultipleChoiceButton button)
    {
        if (useSprites) {
            button.background.sprite = tabActiveSprite;
            return;
        }
            
        button.background.color = tabActiveColor;
    }
}
