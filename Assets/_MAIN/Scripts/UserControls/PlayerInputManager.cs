using UnityEngine;
using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerInputManager : MonoBehaviour
{
    private DialogueSystem ds => DialogueSystem.instance;
    private bool buttonMode = false;

    // Where the buttons will show up
    public GameObject buttonRoot;
    public Transform buttonContainer;
    public GameObject buttonObjectPrefab;
    public GameObject nameInputGameObject;
    public GameObject genderInputGameObject;
    private bool waitForPlayerPrompt;
    private Coroutine process = null;

    // Data Containers
    private Player loadingPlayer = new();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            PromptAdvance();
    }

    #region Player Information Input
    public Coroutine RunGetPlayerName()
    {
        if (process != null)
            ds.StopCoroutine(process);
        process = ds.StartCoroutine(GetPlayerName());
        return process;
    }

    public IEnumerator GetPlayerName()
    {
        nameInputGameObject.SetActive(true);

        TMP_InputField nameInputField = nameInputGameObject.GetComponentInChildren<TMP_InputField>();
        Button nameInputButton = nameInputGameObject.GetComponentInChildren<Button>();

        if (nameInputButton == null)
        {
            Debug.LogWarning("Name input button not found!");
        }

        if (nameInputField == null)
        {
            Debug.LogWarning("Name input field not found!");
        }

        nameInputButton.onClick.AddListener(() => {
            if (nameInputField.text.Length != 0)
            {
                loadingPlayer.playerName = nameInputField.text;
                waitForPlayerPrompt = false;
                nameInputGameObject.SetActive(false);
            }
        });

        yield return WaitForPlayerResponse();
    }

    public Coroutine GetPlayerGender()
    {
        if (process != null)
            ds.StopCoroutine(process);
        process = ds.StartCoroutine(RunGetPlayerGender());
        return process;
    }

    private IEnumerator RunGetPlayerGender()
    {
        genderInputGameObject.SetActive(true);

        Button[] buttons = genderInputGameObject.GetComponentsInChildren<Button>();

        if (buttons.Length != 2)
        {
            Debug.LogError("Buttons are null");
            yield return null;
        }
            

        Button boy = buttons[0];
        Button girl = buttons[1];

        boy.onClick.AddListener(() => {
            loadingPlayer.playerGender = PlayerGender.Male;
            waitForPlayerPrompt = false;
        });

        girl.onClick.AddListener(() => {
            loadingPlayer.playerGender = PlayerGender.Female;
            waitForPlayerPrompt = false;
        });

        yield return WaitForPlayerResponse();
        genderInputGameObject.SetActive(false);
        UpdatePlayerInfo();
        yield return null;
    }

    public void PrepareButton(string buttonText, string branchName, bool isInconsistency, bool isClassTrial)
    {
        buttonRoot.SetActive(false);
        GameObject branchingButtonObject = Instantiate(buttonObjectPrefab, buttonContainer);
        
        Debug.Log($"Creating a button name {buttonText}");
        BranchingButton branchingButton = branchingButtonObject.GetComponent<BranchingButton>();
        branchingButton.SetText(buttonText);
        branchingButton.branchName = branchName;
        branchingButton.isInconsistency = isInconsistency;
        branchingButton.isClassTrial = isClassTrial;
    }
    #endregion

    public Coroutine ShowButtons()
    {
        buttonMode = true;
        buttonRoot.SetActive(true);
        if (process != null)
            ds.StopCoroutine(process);
        
        process = ds.StartCoroutine(WaitForPlayerResponse());
        return process;
    }

    public IEnumerator WaitForPlayerResponse()
    {
        waitForPlayerPrompt = true;
        while(waitForPlayerPrompt)
            yield return null;
        waitForPlayerPrompt = false;
    }
    

    public void ClearButtons()
    {
        waitForPlayerPrompt = false;
        buttonMode = false;
        
        MultipleChoiceGroup group = buttonContainer.gameObject.GetComponent<MultipleChoiceGroup>();

        foreach (var button in group.GetButtons())
        {
            if (button is BranchingButton)
            {
                BranchingButton branchingButton = button as BranchingButton;
            }
        }

        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
        buttonRoot.SetActive(false);
    }
        

    public void PromptAdvance()
    {
        if (!buttonMode)
            ds.OnUserPrompt_Next();
    }

    private void UpdatePlayerInfo()
    {
        SaveSystem.SavePlayer(loadingPlayer);
        GameSystem.instance.LoadPlayer(loadingPlayer.playerName);
    }
}

