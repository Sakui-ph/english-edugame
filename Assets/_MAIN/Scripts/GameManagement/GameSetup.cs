using UnityEditor;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadMain()
    {
        Debug.Log(PlayerSettings.Android.targetSdkVersion);
        GameObject main = Instantiate(Resources.Load("Prefabs/MainGameAssets")) as GameObject;
        DontDestroyOnLoad(main);
    }
}
