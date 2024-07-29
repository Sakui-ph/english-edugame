using UnityEngine;

namespace TESTING
{
    public class SaveTesting : MonoBehaviour
    {
        void Awake()
        {
            Player player = new()
            {
                playerName = "Minecraft Steve",
                playerGender = PlayerGender.Male
            };


            SaveSystem.SavePlayer(player);
        }
    }
}