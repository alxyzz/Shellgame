#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoLoadMenu
{
    static AutoLoadMenu()
    {
        //var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        //var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = null;
        //Debug.Log(pathOfFirstScene + " was set as default play mode scene");
    }
}
#endif