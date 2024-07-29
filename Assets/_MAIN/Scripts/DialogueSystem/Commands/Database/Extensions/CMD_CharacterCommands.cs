using System;
using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using COMMANDS;
using Unity.VisualScripting;
using UnityEngine;

public class CMD_CharacterCommands : CMD_DatabaseExtension
{
    private static CharacterManager manager = CharacterManager.instance;

    new public static void Extend(CommandDatabase database)
    {
        // Usage: MoveCharacter("Name" "X:Y" "Speed / Duration" -s (bool)smooth)
        database.AddCommand("movecharacter", new Func<string[], IEnumerator>(MoveCharacter));
        // Usage: Show("Name1" "Name2" -i (bool)immediate
        database.AddCommand("showcharacter", new Func<string[], IEnumerator>(ShowAll));
        // Usage: Hide("Name1" "Name2" -i (bool)immediate)
        database.AddCommand("hidecharacter", new Func<string[], IEnumerator>(HideAll));
        // Usage: CreateCharacter("Name" -e (bool)enableOnSpawn -i (bool)immediate)
        database.AddCommand("createcharacter", new Action<string[]>(CreateCharacter));
        // Usage: SetPosition("Name" "X:Y")  X = 0-1 Y=0-1
        database.AddCommand("setposition", new Action<string[]>(SetPosition));
        // Usage: FlipCharacter("Name" -i (bool)immediate)
        database.AddCommand("flipcharacter", new Func<string[], IEnumerator>(FlipCharacter));
        database.AddCommand("setcharactercolor", new Action<string[]>(SetCharacterColor));
        database.AddCommand("transitioncharactercolor", new Func<string[], IEnumerator>(ChangeCharacterColor));
        database.AddCommand("freecolor", new Action<string>(FreeCharacterColor));

        // usage Express(Anjelo sad layerNum)
        database.AddCommand("express", new Func<string[], IEnumerator>(Express));
    }

    private static IEnumerator Express(string[] data)
    {
        var parameters = ConvertDataToParameters(data);

        string characterName = "";
        string expressionName = "";
        int layerNum = 0;
        float speed = 1f;


        parameters.TryGetValue("-c", out characterName, "");
        parameters.TryGetValue("-e", out expressionName, "");
        parameters.TryGetValue("-l", out layerNum, 0);
        parameters.TryGetValue("-spd", out speed, 1f);


        if (!CharacterManager.instance.CharacterExists(characterName))
        {
            Debug.LogError("Character doesn't exist");
            yield return null;
        }

        Character character = CharacterManager.instance.GetCharacter(characterName);
        yield return character.ChangeCharacterSprite(expressionName, layerNum, speed);
    }

    public static void SetCharacterColor(string[] data)
    {
        if (data.Length < 5)
        {
            Debug.LogError("SetCharacterColor command requires at least 5 parameters: Name R G B A");
            return;
        }

        string characterName = data[0];
        float r = 255f;
        float g = 255f;
        float b = 255f;
        float a = 255f;

        float.TryParse(data[1], out r);
        float.TryParse(data[2], out g);
        float.TryParse(data[3], out b);
        float.TryParse(data[4], out a);



        Character character = manager.GetCharacter(characterName, createIfDoesNotExist: false);

        if (character == null)
        {
            Debug.LogError("Character not found");
            return;
        }
        character.overriddenColor = true;
        character.SetColor(r,g,b,a);
    }

    public static IEnumerator ChangeCharacterColor(string[] data)
    {
        if (data.Length < 6)
        {
            Debug.LogError("ChangeCharacterColor command requires at least 5 parameters: Name R G B A spd");
            yield return null;
        }

        string characterName = data[0];
        float r = 255f;
        float g = 255f;
        float b = 255f;
        float a = 255f;
        float spd = 1f;

        float.TryParse(data[1], out r);
        float.TryParse(data[2], out g);
        float.TryParse(data[3], out b);
        float.TryParse(data[4], out a);
        float.TryParse(data[5], out spd);
        
        Character character = manager.GetCharacter(characterName, createIfDoesNotExist: false);
        if (character == null)
        {
            Debug.LogError("Character not found");
            yield return null;
        }

        character.overriddenColor = true;
        yield return character.TransitionColor(r, g, b, a, spd);
    }

    public static void FreeCharacterColor(string data)
    {
        Character character = manager.GetCharacter(data, createIfDoesNotExist: false);
        character.overriddenColor = false;
    }

    public static void CreateCharacter(string[] data)
    {
        string characterName = data[0];
        bool enable = false;
        bool immediate = false;

        Character character = manager.CreateCharacter(characterName);

        var parameters = ConvertDataToParameters(data);
        parameters.TryGetValue("-e", out enable, defaultValue: false);
        parameters.TryGetValue("-i", out immediate, defaultValue: false);

        if (character == null)
        {
            return;
        }
        if (!enable)
            return;
        if (immediate)
            character.isVisible = true;
        else
            character.Show();
    }

    public static void SetPosition(string[] data)
    {
        string characterName = data[0];
        string positionString = data[1];
        float x;
        float y;
        if (data.Length < 2)
        {
            Debug.LogError("SetPosition command requires at least 2 parameters: Name and Position");
            return;
        }

        Character character = manager.GetCharacter(characterName, createIfDoesNotExist: false);
        if (character == null)
        {
            Debug.LogError("Character not found");
            return;
        }
        

        string[] positionSplit = positionString.Split(':');

        if (positionSplit.Length < 2)
        {
            Debug.LogError("Position string is not in the correct format. It should be in the format X:Y");
            return;
        }

        bool tryX = float.TryParse(positionSplit[0], out x);
        bool tryY = float.TryParse(positionSplit[1], out y);

        if (!tryX || !tryY)
        {
            Debug.LogError("Position string is not in the correct format. X or Y should be a floating point value");
            return;
        }

        character.SetPosition(new Vector2(x, y));

    }

    public static IEnumerator HideAll(string[] data)
    {
        List<Character> characters = GetCharacters(data);
        bool immediate = false;

        if (characters.Count == 0)
        {
            Debug.LogWarning("No characters found to hide");
            yield return null;
        }
        // Convert data array to parameter container
        var parameters = ConvertDataToParameters(data);

        // Check if the -i flag is present
        parameters.TryGetValue("-i", out immediate, defaultValue: false);

        foreach (Character c in characters)
        {
            if (immediate)
                c.isVisible = false;
            else
                c.Hide();
        }
        yield return null;
    }

    // Usage: Show("Name1" "Name2" -i)
    // -i true will show the characters immediately
    public static IEnumerator ShowAll(string[] data)
    {
        List<Character> characters = GetCharacters(data);
        bool immediate = false;

        if (characters.Count == 0)
        {
            Debug.LogWarning("No characters found to show");
            yield return null;
        }

        // Convert data array to parameter container
        var parameters = ConvertDataToParameters(data);

        // Check if the -i flag is present
        parameters.TryGetValue("-i", out immediate, defaultValue: false);

        foreach (Character c in characters)
        {
            if (immediate)
                c.isVisible = true;
            else
                yield return c.Show();
        }
    }

    private static List<Character> GetCharacters(string[] data)
    {
        List<Character> characters = new List<Character>();

        foreach (string s in data)
        {
            if (s.StartsWith("-"))
                break;

            Character c = manager.GetCharacter(s, false);
            if (c != null)
                characters.Add(c);
        }

        return characters;
    }

    // Format for use:
    // MoveCharacter("Name" "X:Y" "Speed / Duration" -s)
    // -s is optional, if it is present, the movement will be smooth
    public static IEnumerator MoveCharacter(string[] position)
    {
        float x = 0;
        float y = 0;

        if (position.Length < 3)
        {
            Debug.LogError("MoveCharacter command requires at least 3 parameters: Name, Position, and Speed/Duration");
            yield return null;
        }

        string characterName = position[0];
        string positionString = position[1];

        if (positionString.Contains(":") == false)
        {
            Debug.LogError("Position string is not in the correct format. It should be in the format X:Y");
            yield return null;
        }

        string[] positionSplit = positionString.Split(':');

        if (positionSplit.Length < 2)
        {
            Debug.LogError("Position string is not in the correct format. It should be in the format X:Y");
            yield return null;
        }

        bool tryX = float.TryParse(positionSplit[0], out x);
        bool tryY = float.TryParse(positionSplit[1], out y);

        if (!tryX || !tryY)
        {
            Debug.LogError("Position string is not in the correct format. X or Y should be a floating point value");
            yield return null;
        }

        if (!float.TryParse(position[2], out float modifier))
        {
            Debug.LogError("Speed/Duration is not a valid floating point value");
            yield return null;
        }
        var parameters = ConvertDataToParameters(position);
        parameters.TryGetValue("-s", out bool smooth, defaultValue: false);
        if (smooth)
        {   
            yield return manager.GetCharacter(characterName, false).SmoothMoveToPosition(new Vector2(x, y), modifier);
        }
        yield return manager.GetCharacter(characterName, false).MoveToPosition(new Vector2(x, y), modifier);

        yield return null;
    }


    public static IEnumerator FlipCharacter(string[] data)
    {
        Character character = manager.GetCharacter(data[0], false);
        if (character == null)
        {
            Debug.LogError("Character not found");
            yield return null;
        }

        var paramters = ConvertDataToParameters(data);
        paramters.TryGetValue("-i", out bool immediate, defaultValue: false);

        yield return character.Flip(immediate);
        yield return null;
    }
}
