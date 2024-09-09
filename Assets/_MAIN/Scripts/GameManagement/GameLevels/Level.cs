using System;
using System.Collections;
using System.Collections.Generic;
using CARD_GAME;
using UnityEngine;

public class Level
{
    public string levelName;
    public string levelReference;
    public string postLevelReference = "";
    public LevelType levelType;
    public Level(string levelName, string levelReference, LevelType levelType, string postLevelReference = "")
    {
        this.levelName = levelName;
        this.levelReference = levelReference;
        this.levelType = levelType;
        this.postLevelReference = postLevelReference;
    }

    public void EndLevel()
    {
        GameSystem.instance.currentLevel = null;

        SaveLevelData(this);
        if (postLevelReference != "")
                GameSystem.instance.LoadVisualNovel(postLevelReference, () => GameSystem.instance.LoadMainMenu());
            else
                GameSystem.instance.LoadMainMenu();
    }


    private void SaveLevelData(Level level)
    {
        Player player = GameSystem.instance.GetLoadedPlayer();
        if (level.levelType == LevelType.ClassTrial)
        {
            player.SaveLOScore(level.levelReference, LowerOrderScoreHandler.GetPercentage());
        }
        if (level.levelType == LevelType.CardGame)
        {
            player.SaveHOScore(level.levelReference);
        }
    }
}

public enum LevelType
{
    ClassTrial, CardGame
}
