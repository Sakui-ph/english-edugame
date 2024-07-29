using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

public static class SaveSystem 
{
    private const string PLAYER_SAVE_EXTENSION = ".evng";
    private const string SAVE_DIRECTORY = "/saves";
    private const string HO_SCORES_DIRECTORY = "/scores/HO";
    public static void SavePlayer(Player player)
    {
        string directory = Application.persistentDataPath + SAVE_DIRECTORY + $"/{player.playerName}";
        string path = directory + $"/{player.playerName}" + PLAYER_SAVE_EXTENSION;

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            Directory.CreateDirectory(directory + "/scores");
            Directory.CreateDirectory(directory + HO_SCORES_DIRECTORY);
        } 
        

        PLAYER_DATA data = new(player);
        data.ConvertToPlayer();

        File.WriteAllText(path, JsonConvert.SerializeObject(data));
    }

    public static Player LoadPlayer(string player)
    {
        string directory = Application.persistentDataPath + SAVE_DIRECTORY + $"/{player}";
        string path = directory + $"/{player}" + PLAYER_SAVE_EXTENSION;

        if (!PlayerExists(player))
        {
            Debug.LogWarning($"Player with name {player} does not exist");
            return null;
        }

        if (File.Exists(path))
        {
            PLAYER_DATA data = new(JsonConvert.DeserializeObject<Player>(File.ReadAllText(path)));
            
            if (data == null)
            {
                Debug.LogError("Failed to load player!");
                return null;
            }
            return data.ConvertToPlayer();
        } 

        Debug.LogError("Save file not found");
        return null;
    }

    public static bool PlayerExists(string player)
    {
        // this only checks the directory
        string directory = Application.persistentDataPath + SAVE_DIRECTORY + $"/{player}";
        if (!Directory.Exists(directory))
        {
            return false;
        }
        return true;
    }

    public static string[] GetPlayerList()
    {
        string path = Application.persistentDataPath + SAVE_DIRECTORY;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // TODO (optional): check if the save file inside matches and is valid
        string[] directory_paths = Directory.GetDirectories(path);
        string[] save_directories = Directory.GetDirectories(path).Select(Path.GetFileName).ToArray();

        return save_directories;
    }

    public static void SaveHOErrors(string levelName)
    {
        string directory = 
        Application.persistentDataPath 
        + SAVE_DIRECTORY 
        + $"\\{GameSystem.instance.GetLoadedPlayer().playerName}" 
        + HO_SCORES_DIRECTORY;

        List<string> text = new();
        foreach (HighOrderError error in HigherOrderErrorHandler.highOrderErrors)
        {
            string val = error.ToString();
            text.Add(val);
        }

        levelName += " " + System.DateTime.UtcNow.ToString("yyyyMMddHHmmss");

        File.WriteAllLines(directory + $"\\{levelName}.txt", text);
    }

}
