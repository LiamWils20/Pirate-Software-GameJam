using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace Editor.Scripts
{
    public class ImportLevel : MonoBehaviour
    {
        [MenuItem("Assets/Create Level", true)]
        static bool IsModel()
        {
            return Selection.activeObject is GameObject;
        }
        [MenuItem("Assets/Create Level")]
        static void CreateLevel()
        {
            GameObject assetObject = (GameObject)Selection.activeObject;
            string levelName = assetObject.name[..(assetObject.name.IndexOf('_'))];
            if (assetObject.name.ToLower()[..5] == "level")
            {
                string newLevelPath = "Assets/Scenes/Levels/" + levelName + ".unity";
                
                GameObject root = 
                    SceneTemplateService.Instantiate(
                        AssetDatabase.LoadAssetAtPath<SceneTemplateAsset>("Assets/Scenes/level (TEMPLATE).scenetemplate"),
                        true,
                        newLevelPath)
                    .scene.GetRootGameObjects()[0];
                
                Level level = root.GetComponent<Level>();
                level.levelSize = new int[2] { 24, 14 };
                
                AddressableAssetSettingsDefaultObject.Settings.
                    CreateOrMoveEntry(
                        AssetDatabase.AssetPathToGUID(newLevelPath),
                        AddressableAssetSettingsDefaultObject.Settings.FindGroup("Default Local Group"))
                    .address = levelName;
                
                Dictionary<LevelBlock.blockTypes, GameObject> levelBlockPrefabs = new();
                string[] blockTypeNames = Enum.GetNames(typeof(LevelBlock.blockTypes));
                for (int i = 0; i < blockTypeNames.Length; i++)
                {
                    string prefabPath = "Assets/Prefabs/Block_" + blockTypeNames[i] + ".prefab";
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                    if (!prefab) { Debug.LogError("Could not find prefab: " + prefabPath); continue; }
                    levelBlockPrefabs.Add((LevelBlock.blockTypes)i, prefab);
                }
                
                foreach (Transform child in assetObject.transform)
                {
                    if (GetBlockTypeFromName(child.name, out LevelBlock.blockTypes blockType))
                    {
                        GameObject newLevelBlockObject = Instantiate(levelBlockPrefabs[blockType], root.transform);
                        newLevelBlockObject.transform.position = child.position;
                        newLevelBlockObject.name = child.name;
                        LevelBlock newLevelBlock = newLevelBlockObject.GetComponent<LevelBlock>();
                        newLevelBlock.blockType = blockType;
                    }
                    else if (child.name.ToLower() == "player")
                    {
                        level.playerStartPos = Instantiate(new GameObject(), root.transform).transform;
                        level.playerStartPos.name = "playerStartPos";
                        level.playerStartPos.position = child.position;
                    }
                    else
                    {
                        Debug.LogWarning("Unknown Object: " + child.name);
                    }
                }
            }
            else
            {
                Debug.LogError("The provided GameObject is not a level");
            }
        }

        static bool GetBlockTypeFromName(string name, out LevelBlock.blockTypes blockType)
        {
            blockType = LevelBlock.blockTypes.Normal;
            
            if (name.Length < 7) { return false; }

            string[] blockTypeNames = Enum.GetNames(typeof(LevelBlock.blockTypes));
            for (int i = 0; i < blockTypeNames.Length; i++)
            {
                if (name[6..].ToLower().Contains(blockTypeNames[i].ToLower()))
                {
                    blockType = (LevelBlock.blockTypes)i;
                    return true;
                }
            }
            return false;
        }
    }
}
