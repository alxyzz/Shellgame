using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PBRShaderGUI : ShaderGUI
{
    protected MaterialProperty[] _properties;
    protected MaterialEditor _editor;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _properties = properties;
        _editor = materialEditor;
        DrawUI();
    }

    void DrawUI()
    {
        base.OnGUI(_editor, _properties);
    }
}
