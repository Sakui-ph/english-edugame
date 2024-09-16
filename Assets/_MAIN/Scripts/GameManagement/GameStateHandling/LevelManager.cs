using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public Dictionary<uint, LEVEL_DATA> levels {get; private set;}

    void Start()
    {
        // Read all levels
        levels = new();
        ReadAllLevelData();
    }

    public void LaunchLevel(uint levelId)
    {
        string levelPath = levels[levelId].levelPath;
        string level = levelPath.Split(new char[] {Path.DirectorySeparatorChar}).Last();
        GameSystem.instance.LoadVisualNovel(level);
    }

    private void ReadAllLevelData()
    {
        string[] levelDirectories;
        string folderPath = FilePaths.better_stream_assets_levels;
        levelDirectories = BetterStreamingAssets.GetFiles(folderPath, "levelData.json", SearchOption.AllDirectories);

        foreach (string value in levelDirectories)
        {
            string path = Path.Combine(Application.streamingAssetsPath, value);
            string levelPath = Path.GetFullPath(Path.Combine(path, ".."));
            string json_data = File.ReadAllText(path);
            uint levelId = (uint)levels.Count;
            LEVEL_DATA data = new(JsonConvert.DeserializeObject<LevelData>(json_data), levelId, levelPath);

            StoreLevelData(data);
        }
    }

    private void StoreLevelData(LEVEL_DATA levelData)
    {
        if (levels.ContainsValue(levelData))
            {
                Debug.LogWarning($"A copy of {levelData.levelName} with  exists or one with the same name exists. The other copy is ignored");
                return;
            }
        levels.Add(levelData.levelId, levelData);
    }
}
