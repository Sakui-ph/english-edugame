using System;
using System.Collections;
using System.Collections.Generic;
using CARD_GAME;
using UnityEngine;

public class Level
{
    public string levelName;
    public string levelChapterReference;
    public string postLevelChapterReference = "";
    public LevelType levelType;
    public Level(string levelName, string levelChapterReference, LevelType levelType, string postLevelChapterReference = "")
    {
        this.levelName = levelName;
        this.levelChapterReference = levelChapterReference;
        this.levelType = levelType;
        this.postLevelChapterReference = postLevelChapterReference;
    }

    public void EndLevel()
    {
        SaveLevelData(this);
        GameSystem.instance.LoadMainMenu();
    }


    private void SaveLevelData(Level level)
    {
        Player player = GameSystem.instance.GetLoadedPlayer();
        if (level.levelType == LevelType.ClassTrial)
        {
            player.SaveLOScore(level.levelChapterReference, LowerOrderScoreHandler.GetPercentage());
        }
        if (level.levelType == LevelType.CardGame)
        {
            player.SaveHOScore(level.levelChapterReference);
        }
    }
}

public enum LevelType
{
    ClassTrial, CardGame
}
