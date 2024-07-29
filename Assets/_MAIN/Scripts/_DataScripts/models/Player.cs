

using System.Collections.Generic;

public class Player
{
    public string playerName = "John";

    public PlayerGender playerGender = PlayerGender.Male;

    public float volumePreference = 1;

    public Dictionary<string, float> playerScore = new();

    public bool hasSeenHOTutorial = false;

    public void SaveLOScore(string levelName, float score)
    {
        if (playerScore.ContainsKey(levelName))
        {
            playerScore[levelName] = score;
        }
        else
            playerScore.Add(levelName, score);

        SaveSystem.SavePlayer(this);
    }

    public void SaveHOScore(string levelName)
    {
        if (playerScore.ContainsKey(levelName))
        {
            playerScore[levelName] = 1f;
        }
        else
            playerScore.Add(levelName, 1f);
        SaveSystem.SavePlayer(this);
    }
}


public enum PlayerGender
{
    Male, Female
}