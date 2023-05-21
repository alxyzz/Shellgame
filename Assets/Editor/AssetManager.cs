using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class AssetManager
{
    [MenuItem("Assets/Set up Textures")]
    private static void SetupTextures()
    {
        string[] assets = AssetDatabase.FindAssets("t: texture", new string[] { "Assets/Textures" });
        AssetDatabase.StartAssetEditing();
        foreach (string asset in assets)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(asset);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            CustomTextureImporter.ProcessTextureImporter(importer);
        }
        AssetDatabase.StopAssetEditing();
    }
    
    [MenuItem("Assets/Set up Rigs")]
    private static void SetupRigs()
    {
        string[] assets = AssetDatabase.FindAssets("t: model", new string[] { "Assets/Rigs" });
        AssetDatabase.StartAssetEditing();
        foreach (string asset in assets)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(asset);
            ModelImporter importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            CustomAnimationImporter.ProcessAnimationImporter(importer);
        }
        
        AssetDatabase.StopAssetEditing();
    }
}

public class CustomTextureImporter : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {
        TextureImporter importer = assetImporter as TextureImporter;
        ProcessTextureImporter(importer);
    }

    public static bool ProcessTextureImporter(TextureImporter importer)
    {
        string assetName = System.IO.Path.GetFileNameWithoutExtension(importer.assetPath);
        bool needsReimport = false;

        string message = assetName;
        if (assetName.ToLower().EndsWith("_normal"))
        {
            importer.textureType = TextureImporterType.NormalMap;
            message += " is a Normal Map";
            needsReimport = true;
        }
        if (assetName.ToLower().EndsWith("_mra"))
        {
            importer.textureType = TextureImporterType.Default;
            importer.sRGBTexture = false;
            message += " is an MRA Map";
            needsReimport = true;
        }

        if (assetName.ToLower().EndsWith("_mask"))
        {
            importer.textureType = TextureImporterType.Default;
            importer.sRGBTexture = false;
            message += "is a Mask Map";
            needsReimport = true;
        }
        if (needsReimport)
        {
            message += ". Reimport!";
            Debug.Log(message);
            importer.SaveAndReimport();
        }
        return needsReimport;
    }
}

public class CustomAnimationImporter : AssetPostprocessor
{
    private void OnPreprocessAnimation()
    {
        if (assetImporter is not ModelImporter importer || !importer.assetPath.StartsWith("Assets/Rigs/"))
        {
            return;
        }
        ProcessAnimationImporter(importer);
    }

    public static bool ProcessAnimationImporter(ModelImporter importer)
    {
        bool needsReimport = false;

        if (importer.clipAnimations == null || importer.clipAnimations.Length == 0)
        {
            importer.clipAnimations = importer.defaultClipAnimations;
            needsReimport = true;
        }

        var clips = importer.clipAnimations;
        
        foreach (ModelImporterClipAnimation clip in clips)
        {
            if (!clip.takeName.EndsWith("_LOOP")) continue;

            needsReimport = true;
            Debug.Log(clip.takeName);
            clip.loop = true;
            clip.loopTime = true;
        }

        if (needsReimport)
        {
            importer.clipAnimations = clips;
            importer.SaveAndReimport();
            Debug.Log(importer.assetPath);
        }

        return needsReimport;
    }
}
