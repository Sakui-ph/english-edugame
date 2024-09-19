using System.IO;
using System.Linq;
using UnityEngine;

public class LevelMenuSetup : MonoBehaviour
{
    private LevelManager levelManager => GameSystemSL.services.levelManager;
    public GameObject levelMenuOptionPrefab;
    
    
    void Start()
    {
        InitializeLevels();
    }

    public void InitializeLevels()
    {
        levelManager.FindLevels();

        for (int key = 0; key < levelManager.levels.Count; key++)
        {
            InitializeLevel("imagePath", levelManager.levels[key]);
        }
        SortLevels();
    }

    private void InitializeLevel(string imagePath, LEVEL_DATA levelData)
    {
        GameObject newLevelOption = Instantiate(levelMenuOptionPrefab, transform);

        int levelNumber = levelData.levelNumber;
        if (levelNumber < 1)
        {
            levelNumber = levelData.levelId;
        }

        newLevelOption.name = levelNumber + "-" + levelData.levelPath.Split(new char[] {Path.DirectorySeparatorChar}).Last();
        LevelMenuButton levelButton = newLevelOption.GetComponent<LevelMenuButton>();

        // todo: add background image support
        levelButton.levelTitle.text = levelData.levelName;
        levelButton.levelScore.text = levelData.playerScore.ToString();
        levelButton.Subscribe(() => GameSystemSL.services.levelManager.LaunchLevel(levelData.levelId));
    }

    private void SortLevels()
    {
        EditorHelpers.SortChildrenByName(transform);
    }
}