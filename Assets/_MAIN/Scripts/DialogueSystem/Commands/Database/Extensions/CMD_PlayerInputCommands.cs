using COMMANDS;
using DIALOGUE;
using System;
using System.Collections;
public class CMD_PlayerInputCommands : CMD_DatabaseExtension
{   
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

        VisualNovelSL.services.viewController.SetFocusMode(setting);
    }

    public static IEnumerator GetPlayerName()
    {
        yield return VisualNovelSL.services.playerInputManager.RunGetPlayerName();
    }

    public static IEnumerator GetPlayerGender()
    {
        yield return VisualNovelSL.services.playerInputManager.GetPlayerGender();
    }
}


