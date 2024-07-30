using System;
using System.Collections;
using System.Collections.Generic;
using CARD_GAME;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialStep : MonoBehaviour
{
    public List<string> lines = new();
    private SpeechBubble speechBubble => GetComponentInChildren<SpeechBubble>();
    private Coroutine process;
    public bool isRunning => process != null;
    public bool complete = false;
    private bool waitForUser = false;
    private bool userError = false;
    public event Action hideNext;
    public event Action showNext; 
    public event Action blockInputs;
    public event Action allowInputs;

    // custom listeners
    public bool listenForCardDrop = false;
    public bool listenForChainCheck = false;
    public bool listenForTabChange = false;
    public bool listenForClaimSort = false;

    public string ErrorMessage = "Put something here if you wont to listen for something";
    public string ExpectedString = "Put expected result here";

    // Start is called before the first frame update

    public Coroutine SayLines()
    {
        if (isRunning)
            StopCoroutine(process);
        process = StartCoroutine(RunSayLines());
        return process;
    }

    public void UserPrompt()
    {
        waitForUser = false;
    }

    public void UserPrompt(string value)
    {
        if (value == ExpectedString) {
            waitForUser = false;
        } else {
            userError = true;
        }
    }

    private IEnumerator RunSayLines()
    {
        foreach (string line in lines)
        {
            hideNext?.Invoke();
            blockInputs?.Invoke();
            yield return speechBubble.Say(line);

            if (listenForCardDrop){
                yield return WaitForCardDrop();
            }
            else if (listenForChainCheck){
                yield return WaitForChainCheck();
            }
            else if (listenForTabChange){
                yield return WaitForTabChange();
            }
            else if (listenForClaimSort){
                yield return WaitForClaimSort();
            }
            else {
                showNext?.Invoke();
                yield return WaitForUser();
            } 
        }
        complete = true;
    }

    private IEnumerator WaitForChainCheck()
    {
        allowInputs?.Invoke();
        ChainButton.TutorialHelper += UserPrompt;
        yield return WaitForUser();
        ChainButton.TutorialHelper -= UserPrompt;
    }

    private IEnumerator WaitForTabChange()
    {
        allowInputs?.Invoke();
        TabGroup.TutorialHelper += UserPrompt;
        yield return WaitForUser();
        TabGroup.TutorialHelper -= UserPrompt;
    }

    private IEnumerator WaitForClaimSort()
    {
        allowInputs?.Invoke();
        ClaimSortButtons.TutorialHelper += UserPrompt;
        yield return WaitForUser();
        ClaimSortButtons.TutorialHelper -= UserPrompt;
    }

    private IEnumerator WaitForCardDrop()
    {
        allowInputs?.Invoke();
        // CardSlotInteraction.TutorialHelper += UserPrompt;
        yield return WaitForUser();
        // CardSlotInteraction.TutorialHelper -= UserPrompt;
    }

    private IEnumerator WaitForUser()
    {
        waitForUser = true;
        while (waitForUser) {
            if (userError)
            {
                yield return speechBubble.Say(ErrorMessage);
                userError = false;
            }
            yield return null;
        }
            
        waitForUser = false;
    }

    public void RemoveListeners()
    {
        hideNext = null;
        showNext = null;
        blockInputs = null;
        allowInputs = null;
    }
}
