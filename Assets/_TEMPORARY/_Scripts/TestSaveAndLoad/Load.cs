using UnityEngine;

namespace TESTING
{
    public class LoadTesting : MonoBehaviour
    {
        void Awake()
        {
            Player player = SaveSystem.LoadPlayer("Minecraft Steve");

            Debug.Log(player.playerName);
            Debug.Log(player.playerGender);
        }
    }
}