using System;
using CARD_GAME;
using DIALOGUE;
using Unity.Profiling;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public MenuConfigSO menuConfigSO;
    private const string INTRO_LEVEL = "nameselectionchapter"; 
    private SceneHandler sh => GameSystemSL.services.sceneHandler;
    private PlayerHandler playerHandler = new();
    private string[] playerList;
    public bool isFirstStart = true;
    public string cachedLevel;
    public Level currentLevel = null;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeRight;

        Debug.Log(DeveloperConfigSO.DebugMode);

        if (!DeveloperConfigSO.DebugMode)
            sh.LoadScene(SceneName.OpeningScene);  

        // convert URP to SRP
        foreach (var material in Resources.FindObjectsOfTypeAll<Material>())
        {
            if (material.shader.name.StartsWith("Universal Render Pipeline"))
            {
                material.shader = Shader.Find("Standard");
            }
        }   

        BetterStreamingAssets.Initialize();
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

    public void LoadVisualNovel(string levelPath, Action OnLevelEnd = null)
    {
        sh.LoadSceneWithCallback(SceneName.VisualNovel, () => {
            Debug.Log($"Loading Visual Novel under the level path = {levelPath}");
            VisualNovelSL.services.dialogueSystem.LoadLevel(levelPath, OnLevelEnd);
        });
    }

    public void ResetLevel()
    {
        sh.LoadSceneWithCallback(SceneName.VisualNovel, () => {
            VisualNovelSL.services.dialogueSystem.LoadLevel(cachedLevel);
        });
    }

    public void LoadCardGame(bool tutorialMode = false)
    {
        sh.LoadSceneWithCallback(SceneName.CardMinigame, () => {
            CardMinigameSystem.instance.StartGame();

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
        Debug.Log("main menu loaded");
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
        VisualNovelSL.services.dialogueSystem.LoadLevel(INTRO_LEVEL);
        BranchManager.OnBranchEnd += () =>
        {
            LoadMainMenu();
        };
    }

    public void LoadPlayer(string playerName) => playerHandler.LoadPLayer(playerName);
    public Player GetLoadedPlayer() => playerHandler.loadedPlayer;
    public void SaveLoadedPlayer() => playerHandler.SaveCurrentPlayer();
}
