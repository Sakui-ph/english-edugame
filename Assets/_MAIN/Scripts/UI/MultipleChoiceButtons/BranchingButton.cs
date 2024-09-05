using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DIALOGUE;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BranchingButton : MultipleChoiceButton
{
    [SerializeField] private TextMeshProUGUI textTMP;
    public string branchName = "";

    // If is inconsistent and branch type lines up, then they get a point
    // If branching type is none, that means there is no inconsistency to be found
    public bool isInconsistency = false;
    public bool isClassTrial = false;

    void Awake()
    {
        if (!GetComponentInParent<MultipleChoiceGroup>())
            Destroy(this);

        multipleChoiceGroup = GetComponentInParent<MultipleChoiceGroup>();
    }

    public void SetText(string text)
    {
        textTMP.text = text;
    }

    public override void HandleClick()
    {
        base.HandleClick();

        // I wanted to make this an event that happens in PlayerInputManager, but it wouldn't subscribe :(
        if (isClassTrial)
        {
            VisualNovelSL.services.dialogueSystem.CheckClassTrialAnswer(isInconsistency);
        }

        VisualNovelSL.services.dialogueSystem.branchManager.QueueBranch(branchName);
        VisualNovelSL.services.playerInputManager.ClearButtons();
    }
}