using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class EggType : ScriptableObject
{
    public bool isRotten;
    public int raises;
    public int smacks;
    public EggModel model;
}
