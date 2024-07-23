using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance { get; private set; }
    public Level levelCurrent { get; private set; }
    public bool inLevel { get; private set; }
    [HideInInspector] public UnityEvent levelLoaded = new();
    public Volume globalVolume;
    bool waitingForUnload;

    public struct levelLoadData
    {
        public levelLoadData(string _assetKey, bool _useFade)
        {
            assetKey = _assetKey;
            useFade = _useFade;
        }
        public string assetKey;
        public bool useFade;
    }
    void Awake()
    {
        instance = this;
        //levelLoaded.AddListener(delegate { inLevel = true; });
        Addressables.InitializeAsync();
#if !UNITY_EDITOR
        SceneManager.LoadScene("ui", LoadSceneMode.Additive);
        GoToMainMenu();
#endif
    }   
    void Start()
    {
#if UNITY_EDITOR
        if (unloadAllLevelScenesOnStart) { UnloadAllLevelScenesOnStart(); }
        levelCurrent = SceneManager.GetSceneByName("level0").GetRootGameObjects()[0].GetComponent<Level>();
#endif
    }
    void Update()
    {
        
    }
    /// <summary>
    /// Starts the level load sequence
    /// </summary>
    /// <param name="levelLoadData"></param>
    public void LoadLevel(levelLoadData levelLoadData)
    {
        // try to load level
        ui.instance.uiFadeToBlack = levelLoadData.useFade;
        AsyncOperationHandle sceneLoad = Addressables.LoadSceneAsync(levelLoadData.assetKey, LoadSceneMode.Additive);
        sceneLoad.Completed += delegate { StartCoroutine(LoadLevelCompleted(levelLoadData, sceneLoad)); };
    }
    /// <summary>
    /// Checks if the level loaded successfully and begins the level startup sequence
    /// </summary>
    /// <param name="levelLoadData"></param>
    /// <param name="sceneLoad"></param>
    IEnumerator LoadLevelCompleted(levelLoadData levelLoadData, AsyncOperationHandle sceneLoad)
    {
        // load level fail
        if (sceneLoad.OperationException is InvalidKeyException || sceneLoad.Status == AsyncOperationStatus.Failed) 
        { 
            uiMessage.instance.New("Load Level Failed; " + "Invalid Key: " + levelLoadData.assetKey);
            ui.instance.uiFadeToBlack = false;
            yield break;
        }
        // load level success
        // unloading previous level
        if (levelCurrent is not null) { UnloadLevel(levelCurrent); }
        while (waitingForUnload) { yield return new WaitForEndOfFrame(); }
        // getting new level references
        SceneInstance levelSceneInstance = (SceneInstance)sceneLoad.Result;
        levelCurrent = levelSceneInstance.Scene.GetRootGameObjects()[0].GetComponent<Level>();
        levelCurrent.sceneInstance = levelSceneInstance;
        // start the level
        levelCurrent.Begin();
        ui.instance.uiFadeToBlack = false;
        levelLoaded.Invoke();
    }
    /// <summary>
    /// Unloads either the given level or the current level
    /// </summary>
    /// <param name="level"></param>
    /// <param name="returningToMainMenu"></param>
    public void UnloadLevel(Level level, bool returningToMainMenu = false)
    {
        waitingForUnload = true;
        if (!inLevel && level.assetKey == "level0") { uiMessage.instance.New("Tried to unload main menu while in main menu", uiDebug.str_levelLoader); return; }
        level.Unload();
        AsyncOperation sceneUnload = SceneManager.UnloadSceneAsync(level.gameObject.scene);
        sceneUnload.completed += delegate { waitingForUnload = false; };
        if (returningToMainMenu) { sceneUnload.completed += delegate { GoToMainMenu(); }; }
        AsyncOperationHandle sceneInstanceUnload = Addressables.UnloadSceneAsync(levelCurrent.sceneInstance, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }
    
    void GoToMainMenu(bool notify = true)
    {
        LoadLevel(new levelLoadData("level0", true));
        if (notify) { uiMessage.instance.New("Returned To Main Menu", uiDebug.str_levelLoader); }
    }

    #region Unload all level scenes on start if in editor
    #if UNITY_EDITOR
    [SerializeField] bool unloadAllLevelScenesOnStart = true;
    List<UnityEngine.SceneManagement.Scene> scenes = new List<UnityEngine.SceneManagement.Scene> ();
    void UnloadAllLevelScenesOnStart()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++) { scenes.Add(SceneManager.GetSceneAt(i)); }
        string output = string.Empty;
        foreach (UnityEngine.SceneManagement.Scene scene in scenes)
        {
            if (scene.GetRootGameObjects()[0].GetComponent<Level>() is not null && scene.name != "level0")
            {
                output += scene.name + ", ";
    #pragma warning disable CS0618 // Type or member is obsolete
                SceneManager.UnloadScene(scene);
    #pragma warning restore CS0618 // Type or member is obsolete
            }
        }
        if (output != string.Empty) { Debug.LogWarning("The scenes " + output + "were found on startup and were unloaded."); }
    }
    #endif
    #endregion
    }