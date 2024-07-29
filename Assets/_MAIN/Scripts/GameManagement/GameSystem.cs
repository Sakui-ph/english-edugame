using System;
using CARD_GAME;
using DIALOGUE;
using Unity.Profiling;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public MenuConfigSO menuConfigSO;
    private const string INTRO_CHAPTER = "nameselectionchapter"; 
    private SceneHandler sh => SceneHandler.instance;
    private PlayerHandler playerHandler = new();
    private string[] playerList;
    public static GameSystem instance;
    public bool isFirstStart = true;
    public string cachedChapter;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeRight;
        sh.LoadScene(SceneName.OpeningScene);
        

        // convert URP to SRP
        foreach (var material in Resources.FindObjectsOfTypeAll<Material>())
        {
            if (material.shader.name.StartsWith("Universal Render Pipeline"))
            {
                material.shader = Shader.Find("Standard");
            }
        }   

        #if UNITY_ANDROID
        BetterStreamingAssets.Initialize();
        #endif
    }

    public void CheckForPlayerList()
    {
        // Load the animations necessary at the beginning
        // TODO: Add the video here

        playerList = SaveSystem.GetPlayerList();
        // Check if any player saves exist, if there are none, move to main menu, if there are any, move straight to username select and then chapter 1
        if (playerList.Length == 0)
        {
            Debug.Log("There are no player saves");
            sh.SetModeSingle();
            CreateNewCharacter();
        }
        else
        {
            sh.SetModeSingle();
            LoadAccountSelect();
            return;
        }
    }

    public void LoadVisualNovel(string levelReference, Action OnChapterEnd = null)
    {
        sh.LoadSceneWithCallback(SceneName.VisualNovel, () => {
            DialogueSystem.instance.LoadChapter(levelReference, OnChapterEnd);
        });
    }

    public void ResetLevel()
    {
        sh.LoadSceneWithCallback(SceneName.VisualNovel, () => {
            DialogueSystem.instance.LoadChapter(cachedChapter);
        });
    }

    public void LoadCardGame(bool tutorialMode = false)
    {
        sh.LoadSceneWithCallback(SceneName.CardMinigame, () => {
            CardMinigameSystem.instance.StartGame(CardMinigameLevelLoader.LoadLevel());

            if (tutorialMode && !GetLoadedPlayer().hasSeenHOTutorial)
            {
                sh.SetModeAdditive();
                sh.LoadScene(SceneName.HOLMTutorial);
                sh.SetModeSingle();
            }
            
        });
    }

    public void LoadMainMenu()
    {
        sh.LoadScene(SceneName.MainMenu);
    }

    public void LoadAccountSelect() => sh.LoadSceneWithCallback(SceneName.AccountSelect, LoadAccountSelection);
    

    public void CreateNewCharacter() => sh.LoadSceneWithCallback(SceneName.VisualNovel, RunGameIntro);
    
    private void LoadAccountSelection()
    {
        AccountSelectHandler ash = GameObject.Find("AccountSelect").GetComponent<AccountSelectHandler>();
        ash.SetupButtons(playerList);
    }
    
    private void RunGameIntro()
    {
        DialogueSystem.instance.LoadChapter(INTRO_CHAPTER);
        ChapterManager.OnChapterEnd += () =>
        {
            LoadMainMenu();
        };
    }

    public void LoadPlayer(string playerName) => playerHandler.LoadPLayer(playerName);
    public Player GetLoadedPlayer() => playerHandler.loadedPlayer;
    public void SaveLoadedPlayer() => playerHandler.SaveCurrentPlayer();
}