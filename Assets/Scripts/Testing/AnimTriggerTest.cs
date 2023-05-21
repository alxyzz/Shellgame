using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTriggerTest : MonoBehaviour
{
    public CookAnimationManager cookManager;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)) cookManager.GetEgg();
        if(Input.GetKeyDown(KeyCode.L)) cookManager.LiftEgg();
        // var shiftPressed = Input.GetKey(KeyCode.LeftShift);
        if(Input.GetKeyDown(KeyCode.S)) cookManager.Smack(true);
        if(Input.GetKeyDown(KeyCode.C)) cookManager.Crack(true);
    }
}
