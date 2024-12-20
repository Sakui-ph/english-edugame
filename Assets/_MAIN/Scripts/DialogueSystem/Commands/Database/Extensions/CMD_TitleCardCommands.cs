using COMMANDS;
using DIALOGUE;
using System;
using System.Collections;
using UnityEngine;
public class CMD_TitleCardCommands : CMD_DatabaseExtension
{   
    new public static void Extend(CommandDatabase database)
    {
        // usage PlayTitle(HeaderText TitleText Delay)
        database.AddCommand("playtitle", new Func<string[], IEnumerator>(PlayTitle));

        database.AddCommand("setweekday", new Action<string>(SetWeekday));
    }

    public static void SetWeekday(string day)
    {
        DaysOfTheWeek value = DaysOfTheWeek.Monday;
        Enum.TryParse(day, out value);
        VisualNovelSL.services.viewController.SetWeekday(value);
    }

    public static IEnumerator PlayTitle(string[] data)
    {
        var parameters = ConvertDataToParameters(data);
        string headerText = "";
        string titleText = "";
        float delay = 10f;

        parameters.TryGetValue("-h", out headerText, "");
        parameters.TryGetValue("-t", out titleText, "");
        parameters.TryGetValue("-d", out delay, 10f);
        
        VisualNovelSL.services.titleCardController.SetHeaderText(headerText);
        VisualNovelSL.services.titleCardController.SetTitleText(titleText);
        VisualNovelSL.services.titleCardController.ShowCard();
        yield return new WaitForSeconds(delay);
        VisualNovelSL.services.titleCardController.HideCard();

        yield return new WaitForSeconds(1.5f);
    }
}


