using COMMANDS;
using CHARACTERS;
using System;
using UnityEngine;
using System.Collections;

public class CMD_DatabaseExtension_Animations : CMD_DatabaseExtension
{   
    private static string[] PARAM_ANIMATION_NAME = new string[] { "-a", "-animation" };
    private static string[] PARAM_CHARACTER_NAME = new string[] { "-c", "-character" }; 
    new public static void Extend(CommandDatabase database)
    {
        // USAGE: triggeranimation -c [characterName] -a [animationName]
        database.AddCommand("triggeranimation", new Func<string[], IEnumerator>(TriggerAnimation));
        // USAGE: toggleanimation -c [characterName] -a [animationName]
        database.AddCommand("toggleanimation", new Action<string[]>(ToggleAnimation));
    }

    private static IEnumerator TriggerAnimation(string[] data)
    {
        var parameters = ConvertDataToParameters(data);
        string animationName = "";
        string characterName = "";

        parameters.TryGetValue(PARAM_CHARACTER_NAME, out characterName);
        parameters.TryGetValue(PARAM_ANIMATION_NAME, out animationName);

        if (string.IsNullOrEmpty(animationName))
        {
            Debug.LogError("Animation name is not provided, cannot play animation");
            yield return null;
        }

        if (string.IsNullOrEmpty(characterName))
        {
            Debug.LogError("Character name is not provided, cannot play animation");
            yield return null;
        }

        Character character = CharacterManager.instance.GetCharacter(characterName);
        if (character == null)
        {
            Debug.LogError($"Character with name {characterName} does not exist, cannot play animation");
            yield return null;
        }

        Animator anim = character.animator;
        // If the character is already playing the animation, wait for it to complete
        while (anim.GetCurrentAnimatorStateInfo(0).IsName(animationName) &&
        anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
            yield return null;
        }
        character.Animate(animationName);
    }

    private static void ToggleAnimation(string[] data)
    {
        var parameters = ConvertDataToParameters(data);
        string animationName = "";
        string characterName = "";

        parameters.TryGetValue(PARAM_CHARACTER_NAME, out characterName);
        parameters.TryGetValue(PARAM_ANIMATION_NAME, out animationName);

        if (string.IsNullOrEmpty(animationName))
        {
            Debug.LogError("Animation name is not provided, cannot play animation");
            return;
        }

        if (string.IsNullOrEmpty(characterName))
        {
            Debug.LogError("Character name is not provided, cannot play animation");
            return;
        }

        Character character = CharacterManager.instance.GetCharacter(characterName);
        if (character == null)
        {
            Debug.LogError($"Character with name {characterName} does not exist, cannot play animation");
            return;
        }

        bool currentState = character.animator.GetBool(animationName);
        character.Animate(animationName, !currentState);
    }
}
