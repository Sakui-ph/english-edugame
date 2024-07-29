using COMMANDS;
using System;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using DIALOGUE;

public class CMD_DatabaseExtension_BranchingCommands : CMD_DatabaseExtension
{
    private static string[] BUTTON_NAME = new string[] { "-btn", "-buttonName" };
    private static string[] BRANCH_NAME = new string[] { "-branch", "-b" };
    private static string[] INCONSISTENTCY = new string[] { "-i", "-inconsistency" };
    private static string[] IS_CLASS_TRIAL = new string[] { "-ct", "-classTrial" };
    new public static void Extend(CommandDatabase database)
    {
        // usage PrepareNormalButton(buttonName, branchName)
        database.AddCommand("preparenormalbutton", new Action<string[]>(PrepareNormalButton));
        // usage PrepareClassTrialButton(buttonName, branchName, isInconsistency?)
        database.AddCommand("prepareclasstrialbutton", new Action<string[]>(PrepareClassTrialButton));
        database.AddCommand("returnbranch", new Action<string>(ReturnBranch));
        database.AddCommand("returndirectory", new Action<string>(ReturnDirectory));
        database.AddCommand("showbuttons", new Func<IEnumerator>(ShowButtons));
    }

    private static (string, string, bool) PrepareButton(string[] data)
    {
        var parameters = ConvertDataToParameters(data);
        string buttonName = "";
        string branchName = "";
        bool isInconsistency = false;

        parameters.TryGetValue(BUTTON_NAME, out buttonName, defaultValue: "");
        if (buttonName == "")
            Debug.LogError("No branching button text given!");
        parameters.TryGetValue(BRANCH_NAME, out branchName, defaultValue: "");
        if (branchName == "")
            Debug.LogError("No branch name given!");
        parameters.TryGetValue(INCONSISTENTCY, out isInconsistency, defaultValue: false);

        return (buttonName, branchName, isInconsistency);
    }

    private static void PrepareNormalButton(string[] data)
    {
        string buttonName = "";
        string branchName = "";
        bool isInconsistency = false;
        (buttonName, branchName, isInconsistency) = PrepareButton(data);
        DialogueSystem.instance.playerInputManager.PrepareButton(buttonName, branchName, false, false);
    }

    private static void PrepareClassTrialButton(string[] data)
    {
        string buttonName = "";
        string branchName = "";
        bool isInconsistency = false;
        (buttonName, branchName, isInconsistency) = PrepareButton(data);
        DialogueSystem.instance.playerInputManager.PrepareButton(buttonName, branchName, isInconsistency, true);
    }


    private static void ReturnBranch(string fileName)
    {
        DialogueSystem.instance.chapterManager.QueuePreviousFile(fileName);
    }

    private static void ReturnDirectory(string fileName)
    {
        DialogueSystem.instance.chapterManager.QueuePreviousDirectory(fileName);
    }

    private static IEnumerator ShowButtons()
    {
        yield return DialogueSystem.instance.playerInputManager.ShowButtons();
    }

}

