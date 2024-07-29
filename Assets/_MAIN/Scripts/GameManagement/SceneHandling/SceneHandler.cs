using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;
    private LoadSceneMode loadingMode = LoadSceneMode.Single;
    private Coroutine process = null;

    public event Action OnLoadScene;
    public event Action OnUnloadScene;

    void Awake()   
    {
        if (instance == null)
        {
            instance = this;

            SceneManager.sceneLoaded += LoadSceneCallback;
            SceneManager.sceneUnloaded += UnloadSceneCallback;
        }
        else {
            Destroy(gameObject);
        }
            
    }
    
    public void LoadScene(SceneName sceneName)
    {
        process = StartCoroutine(RunLoadScene(sceneName));
    }

    public void UnloadScene(SceneName sceneName)
    {
        process = StartCoroutine(RunUnloadScene(sceneName));
    }

    private IEnumerator RunLoadScene(SceneName sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName.ToString(), loadingMode);
        while (!scene.isDone){
            yield return null;
        }
        process = null;
    }

    private void LoadSceneCallback(Scene scene, LoadSceneMode loadSceneMode)
    {
        OnLoadScene?.Invoke();
        OnLoadScene = null;
    }

    public IEnumerator RunUnloadScene(SceneName sceneName)
    {
        var scene = SceneManager.UnloadSceneAsync(sceneName.ToString());
        while (!scene.isDone){
            yield return null;
        }
        OnUnloadScene?.Invoke();
    }

    private void UnloadSceneCallback(Scene scene)
    {
        OnUnloadScene?.Invoke();
        OnUnloadScene = null;
    }

    public void LoadSceneWithCallback(SceneName sceneName, Action callback, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        if (!SceneIsLoaded(sceneName.ToString()))
            {
                OnLoadScene += callback;
                loadingMode = loadSceneMode;
                LoadScene(sceneName);
            } 
        else callback();
    }

    public bool SceneIsLoaded(string sceneName) => SceneManager.GetSceneByName(sceneName).isLoaded;
    public void SetModeAdditive() => loadingMode = LoadSceneMode.Additive;
    public void SetModeSingle() => loadingMode = LoadSceneMode.Single;
}


public enum SceneName
{
    MainMenu, VisualNovel, CardMinigame, AccountSelect, OpeningScene, HOLMTutorial
}