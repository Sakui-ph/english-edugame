using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PLAYER_DATA 
{
    public string playerName;
    public int playerGender;
    public float volumePreference;
    public Dictionary<string, float> playerScore = new();
    public bool hasSeenHOTutorial = false;

    public PLAYER_DATA(Player player)
    {
        playerName = player.playerName;
        playerGender = (int)player.playerGender;
        volumePreference = player.volumePreference;
        playerScore = player.playerScore;
        hasSeenHOTutorial = player.hasSeenHOTutorial;
    }

    public Player ConvertToPlayer()
    {
        Player player = new Player
        {
            playerName = playerName,
            playerGender = (PlayerGender)playerGender,
            volumePreference = volumePreference,
            playerScore = playerScore,
            hasSeenHOTutorial = hasSeenHOTutorial
        };

        return player;
    }
}
