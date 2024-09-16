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
        GameSystemSL.services.gameSystem.currentLevel = null;

        SaveLevelData(this);
        if (postLevelReference != "")
                GameSystemSL.services.gameSystem.LoadVisualNovel(postLevelReference, () => GameSystemSL.services.gameSystem.LoadMainMenu());
            else
                GameSystemSL.services.gameSystem.LoadMainMenu();
    }


    private void SaveLevelData(Level level)
    {
        Player player = GameSystemSL.services.gameSystem.GetLoadedPlayer();
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
