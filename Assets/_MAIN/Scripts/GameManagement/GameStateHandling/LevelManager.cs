using System.IO;
using Newtonsoft.Json;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    void Start()
    {
        // Read all levels
        ReadAllLevelData();

        // Read all relevant json files
    }

    private void ReadAllLevelData()
    {
        string[] levelDirectories;
        string folderPath = FilePaths.better_stream_assets_levels;
        levelDirectories = BetterStreamingAssets.GetFiles(folderPath, "levelData.json", SearchOption.AllDirectories);

        foreach (string value in levelDirectories)
        {
            string path = Path.Combine(Application.streamingAssetsPath, value);
            string json_data = File.ReadAllText(path);
            LEVEL_DATA data = new(JsonConvert.DeserializeObject<LevelData>(json_data), path);
            Debug.Log(data);
        }
    }
}
