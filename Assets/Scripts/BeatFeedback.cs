using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatFeedback : MonoBehaviour
{
    public Renderer modelRenderer;
    public AnimationCurve remapping;
    public float multiplier = 1;

    private void Update()
    {
        var beatKeeper = GameManager.Instance.beatKeeper;
        var score = beatKeeper.BeatFrac;
        modelRenderer.material.SetFloat("_EmissiveStrength", beatKeeper.IsPlaying? remapping.Evaluate(score) * multiplier : 0.5f);
    }
}
