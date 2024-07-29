
public class PlayerHandler
{
    public Player loadedPlayer = new();

    public void LoadPLayer(string playerName)
    {
        loadedPlayer = SaveSystem.LoadPlayer(playerName);
    }

    public void SaveCurrentPlayer()
    {
        SaveSystem.SavePlayer(loadedPlayer);
    }

}