using COMMANDS;
using DIALOGUE;
using System;
using System.Collections;
public class CMD_PlayerInputCommands : CMD_DatabaseExtension
{   
    private static DialogueSystem ds => DialogueSystem.instance;

    new public static void Extend(CommandDatabase database)
    {
        database.AddCommand("getplayername", new Func<IEnumerator>(GetPlayerName));
        database.AddCommand("getplayergender", new Func<IEnumerator>(GetPlayerGender));
        database.AddCommand("setfocusmode", new Action<string>(SetFocusMode));
    }

    public static void SetFocusMode(string data)
    {
        bool setting = true;
        bool.TryParse(data, out setting);

        ds.viewController.SetFocusMode(setting);
    }

    public static IEnumerator GetPlayerName()
    {
        yield return ds.playerInputManager.RunGetPlayerName();
    }

    public static IEnumerator GetPlayerGender()
    {
        yield return ds.playerInputManager.GetPlayerGender();
    }
}


