using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggModel : MonoBehaviour
{
    public Transform topShell;
    public Transform bottomShell;

    public void UpdateTransforms(CookAnimationManager manager)
    {
        GameManager.CopyTransforms(manager.TopShell, topShell, switchAxes: true);
        GameManager.CopyTransforms(manager.BottomShell, bottomShell, switchAxes: true);
    }
}
