using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BeatKeeper : MonoBehaviour
{
    public AudioClip musicClip;
    public AudioClip clickSound;
    public int beatsPerMinute;

    public float Frequency => 60f / beatsPerMinute;
    [Range(0, 1)] public float leniency = 0.5f;
    public bool IsPlaying => _isPlaying;
    public float StartTime => StartTime;
    public float beatFrac => ((Time.time - _startTime) % Frequency) / Frequency;
    
    private AudioSource _audioSource;
    private float _startTime;
    private bool _isPlaying;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isPlaying) StopBeat();
            else StartBeat();
        }

        if (_isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _audioSource.PlayOneShot(clickSound);
                var score = ValidateInput();
                if (score > leniency)
                {
                    Debug.Log($"Nice Shot! {score}");
                }
            }
        }
    }

    public void StartBeat()
    {
        _startTime = Time.time;
        _isPlaying = true;
        _audioSource.PlayOneShot(musicClip);
    }

    public void StopBeat()
    {
        _isPlaying = false;
        _audioSource.Stop();
    }

    public float ValidateInput()
    {
        var score = Mathf.Abs(beatFrac - .5f) * 2;
        return score;
    }
}
