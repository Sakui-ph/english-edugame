using UnityEditor;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadMain()
    {
        GameObject main = Instantiate(Resources.Load("Prefabs/MainGameAssets")) as GameObject;
        DontDestroyOnLoad(main);
    }
}
