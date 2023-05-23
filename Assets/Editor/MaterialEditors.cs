
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;

public class GenericCustomShaderGUI : ShaderGUI
{

    protected MaterialEditor _editor;
    protected MaterialProperty[] _properties;
    protected Material _material;
    
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _editor = materialEditor;
        _properties = properties;
        _material = _editor.target as Material;
        DrawUI();
    }

    protected virtual void DrawUI()
    {
        base.OnGUI(_editor, _properties);
    }

    protected string[] GetPropertyNames()
    {
        return _properties.Select(x => x.name).ToArray();
    }

    protected MaterialProperty FindProperty(string name)
    {
        return FindProperty(name, _properties, propertyIsMandatory:false);
    }

    protected bool FindProperty(string name, out MaterialProperty prop)
    {
        prop = FindProperty(name);
        return prop is not null;
    }

    protected GUIContent MakeLabel(MaterialProperty property, string tooltip = null)
    {
        var result = new GUIContent();
        result.text = property.displayName;
        result.tooltip = tooltip;
        return result;
    }

    protected void DisplayProperty(MaterialProperty property, GUIContent label = null)
    {
        if (label is null)
        {
            label = MakeLabel(property);
        }
        _editor.ShaderProperty(property, label);
    }

    protected bool FindProperties(params string[] names)
    {
        return names.All(x => GetPropertyNames().Contains(x));
    }

    protected bool FindProperties(out MaterialProperty[] prop, params string[] names)
    {
        var exist = FindProperties(names);
        prop = exist? names.Select(FindProperty).ToArray() : null;
        return exist;
    }

    protected void SetKeyword(string keyword, bool state)
    {
        if (state) _material.EnableKeyword(keyword);
        else _material.DisableKeyword(keyword);
    }
}

public class CompactPBRGUI : GenericCustomShaderGUI
{

    protected int _mapIndex;
    protected override void DrawUI()
    {
        var compactMapsAvailable = FindProperties(out var props, GetCompactMaps().Keys.ToArray());
        if (!compactMapsAvailable)
        {
            base.DrawUI();
            return;
        }
        var excludeProps = DrawCompactMaps();
        base.OnGUI(_editor, _properties.Where(x=>!excludeProps.Contains(x)).ToArray());
    }

    protected virtual Dictionary<string, string> GetCompactMaps()
    {
        return new (){ { "_MainTex", "BaseColor" }, { "_MRATex", "MRA" }, { "_NormalTex", "Normal" } };
    }

    protected List<MaterialProperty> DrawCompactMaps()
    {
        var maps = GetCompactMaps();
        var propertyKeys = maps.Keys.ToArray();
        var assetSuffixes = maps.Values.ToArray();

        FindProperties(out var textureProps, propertyKeys);

        var drawnProperties = textureProps.ToList();
        
        var assetPaths = AssetDatabase.FindAssets("t:texture", new[] { "Assets/Textures" })
            .Select(x => AssetDatabase.GUIDToAssetPath(x)).ToArray();

        var assetNames = assetPaths.Select(Path.GetFileNameWithoutExtension).ToArray();

        var baseColorTexturePaths = assetNames
            .Where(x => x.Contains("TEX_"))
            .Where(x=> x.Contains(assetSuffixes[0]))
            .ToArray();
        var validTextureSetNames = baseColorTexturePaths;

        for (int i = 1; i < propertyKeys.Length; i++)
        {
            validTextureSetNames = validTextureSetNames
                .Where(x => assetNames.Contains(x.Replace(assetSuffixes[0], assetSuffixes[i]))).ToArray();
        }

        validTextureSetNames = validTextureSetNames.Select(x => x.Replace("_" + assetSuffixes[0], "")).ToArray();

        GUILayout.BeginHorizontal();
        
            GUILayout.Label("Autofill Maps:", EditorStyles.boldLabel);
            _mapIndex = EditorGUILayout.Popup(_mapIndex, validTextureSetNames);
            if (GUILayout.Button("Apply!"))
            {
                var setName = validTextureSetNames[_mapIndex];
                Debug.Log($"Apply Texture Set {setName}");

                foreach (var (propName, suffix) in maps)
                {
                    var tex = AssetDatabase.LoadAssetAtPath<Texture>(assetPaths.First(x => x.Contains($"{setName}_{suffix}")));
                    FindProperty(propName).textureValue = tex;
                }
            }
        GUILayout.EndHorizontal();

        for (int i = 0; i < textureProps.Length; i++)
        {
            var texProp = textureProps[i];

            if (i == 0)
            {
                var albedoLabel = MakeLabel(texProp);
                if (FindProperty("_Tint", out var tint)) drawnProperties.Add(tint);

                _editor.TexturePropertySingleLine(albedoLabel, texProp, tint);
                if((!texProp.flags.HasFlag(MaterialProperty.PropFlags.NoScaleOffset)))
                    _editor.TextureScaleOffsetProperty(texProp);
            }

            else if (i == 1)
            {
                _editor.TexturePropertySingleLine(MakeLabel(texProp), texProp);
                var hasMetallicRoughness = FindProperties(out var metallicRoughness, "_Metallic", "_Roughness");
                if (hasMetallicRoughness)
                {
                    drawnProperties.AddRange(metallicRoughness);
                    var mraEmpty = texProp.textureValue is null;
                    // SetKeyword("_USE_METALLIC_ROUGHNESS_SLIDER", mraEmpty);
                    if (mraEmpty)
                    {
                        DisplayProperty(metallicRoughness[0]);
                        DisplayProperty(metallicRoughness[1]);
                    }
                }
            }
            else
            {
                _editor.TexturePropertySingleLine(MakeLabel(texProp), texProp);
            }
        }

        return drawnProperties;
    }
}
