using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "DeveloperConfigSO", menuName = "DeveloperConfigSO", order = 0)]
public class DeveloperConfigSO : ScriptableObject {
    [SerializeField]
    private bool DEBUG_MODE = false;
    private static DeveloperConfigSO instance;

    public static bool DebugMode {
        get {
            if (instance == null) {
                instance = Resources.Load<DeveloperConfigSO>("DeveloperConfigSO");
            }
            return instance != null && instance.DEBUG_MODE;
        }
    }
}
