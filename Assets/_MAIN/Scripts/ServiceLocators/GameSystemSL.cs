using System.Collections;
using System.Collections.Generic;
using AUDIO_SYSTEM;
using UnityEngine;

public class GameSystemSL : MonoBehaviour
{
    public static GameSystemSL services            {get; private set;}

    public GameSystem gameSystem                   {get; private set;}
    public AudioManager audioManager               {get; private set;}
    public SceneHandler sceneHandler               {get; private set;}
    public LevelManager levelManager               {get; private set;}




    void Awake()
    {
        if (services != null)
        {
            Destroy(this.gameObject);
            return;
        }
        services = this;
        Initialize();
        DontDestroyOnLoad(gameObject);
    }

    private void Initialize()
    {
        gameSystem =   GetComponentInChildren<GameSystem>();
        audioManager = GetComponentInChildren<AudioManager>();
        sceneHandler = GetComponentInChildren<SceneHandler>();
        levelManager = GetComponentInChildren<LevelManager>();
    }
}
