using System.Collections;
using UnityEngine;
using TMPro;
using System;
using AUDIO_SYSTEM;
using System.Collections.Generic;

public class TextArchitect
{
    private TextMeshProUGUI tmpro_ui;
    private TextMeshPro tmpro_world;

    private const float baseSpeed = 1;
    private float speedMultiplier = 1;
    private int characterMultiplier = 1;
    private Coroutine buildProcess = null;
    public bool isBuilding => buildProcess != null;
    public bool hurryUp = false;

    public TMP_Text tmpro => tmpro_ui != null ? tmpro_ui : tmpro_world;

    public string currentText => tmpro.text;
    public string targetText {get; private set;} = "";
    public string preText {get; private set;} = "";
    public string fullTargetText => preText + targetText;
    public List<AudioClip> voiceBank = null;

    public enum BuildMethod {instant, typewriter, fade}


    public BuildMethod buildMethod = BuildMethod.typewriter;
    public Color textColor {get {return tmpro.color;} set {tmpro.color = value;}}
    public float speed {get {return baseSpeed * speedMultiplier; } set {speedMultiplier = value;}}
    public int charactersPerCycle {get { return speed <= 2f ? characterMultiplier 
                                              : speed <= 2.5 ? characterMultiplier * 2 
                                              : characterMultiplier * 3;}}


    public TextArchitect(TextMeshProUGUI tmpro_ui) {
        this.tmpro_ui = tmpro_ui;
    }

    public TextArchitect(TextMeshPro tmpro_world) {
        this.tmpro_world = tmpro_world;
    }

    // Starts building the text one character at a time
    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    // Appends text to what the text architect is already building
    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    public void Stop() 
    {
        if (!isBuilding) return;
        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;
    }

    IEnumerator Building() 
    {
        Prepare();
        switch(buildMethod) 
        {
            case BuildMethod.typewriter:
                yield return Build_Typewriter();
                break;
            case BuildMethod.fade:
                yield return Build_Fade();
                break;
        }
        OnComplete();
    }

    private IEnumerator Build_Typewriter() 
    {
        StartVoices();
        while(tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount) 
        {
            tmpro.maxVisibleCharacters += hurryUp ? charactersPerCycle * 5 : charactersPerCycle;

            yield return new WaitForSeconds(0.015f / speed);
        }
        AudioManager.instance.StopVoiceBank();
    }

    private void StartVoices()
    {
        if (voiceBank == null)
            return;
        if (voiceBank.Count != 0)
            AudioManager.instance.RunVoiceBank(voiceBank);
    }

    private IEnumerator Build_Fade()
    {
        yield return null;
    }

    private void OnComplete()
    {
        buildProcess = null;
        hurryUp = false;
    }

    public void ForceComplete()
    {
        switch(buildMethod) 
        {
            case BuildMethod.typewriter:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;
            case BuildMethod.fade:
                break;
        }
    }

    // Prepares text for whatever our build process is
    private void Prepare()
    {
        switch(buildMethod) 
        {
            case BuildMethod.instant:
                Prepare_Instant();
                break;
            case BuildMethod.typewriter:
                Prepare_Typewriter();
                break;
            case BuildMethod.fade:
                Prepare_Fade();
                break;
        }
    }

    private void Prepare_Instant()
    {
        // Forces it to reinitialize itself, the colors that are changing 
        // in Fade is not the one in the text object itself buts its the one in the vertices
        tmpro.color = tmpro.color;
        tmpro.text = fullTargetText;
        tmpro.ForceMeshUpdate();
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
    }

    private void Prepare_Typewriter()
    {   
        // Reset the color in case we were fading
        tmpro.maxVisibleCharacters = 0;

        // Set it to preText first to check if its empty
        tmpro.text = preText;
    
        if (preText != "")
        { // If there is preText, we force the update to make sure the text is still there and we also update
          // maxCharCount to the number of characters in the pretext
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }
        // Tack on the target text and we're good
        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();

    }

    private void Prepare_Fade()
    {
        // decided I didn't need this
    }

    public void EmptyBox()
    {
        tmpro.text = "";
    }


}
