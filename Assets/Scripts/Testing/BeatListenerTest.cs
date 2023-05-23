using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BeatListenerTest : MonoBehaviour
{
    public BeatKeeper beatKeeper;

    private void Update()
    {
        if (!beatKeeper.IsPlaying)
        {
            return;
        }

        var yPos = beatKeeper.ValidateInput();
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
