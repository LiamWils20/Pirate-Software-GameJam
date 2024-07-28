using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.ResourceProviders;

public class Level : MonoBehaviour
{
    [Header("Level Attributes")]
    [Tooltip("The player will be teleported here when the section is started")] public Transform playerStartPos;
    public string inGameName;
    public int[] levelSize = new int[2]{ 0, 0 }; 
    [HideInInspector] public string assetKey;

    [Header("References")]
    [HideInInspector] public SceneInstance sceneInstance;

    
    public LevelBlock[][] levelBlocks;
    
    public void Begin() //
    {
        
    }

    private void Awake()
    {
        assetKey = gameObject.scene.name;
    }
    void Update() 
    {
        
    }
    public StringBuilder debugGetStats()
    {
        return new StringBuilder()
            .Append(uiDebug.str_assetKey).Append(assetKey)
            .Append(uiDebug.str_inGameName).Append(inGameName)
            .Append(uiDebug.str_playerStartPos);
    }
    // Called by LevelLoader when unloading this level
    public void Unload()
    {
        
    }
    void End() // ends the level
    {
        uiMessage.instance.New("End of " + assetKey, uiDebug.str_level);
        LevelLoader.instance.UnloadLevel(this, true);// end of level sequence here
    }
}