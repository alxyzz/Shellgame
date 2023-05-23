using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class BeatKeeper : MonoBehaviour
{
    public AudioClip musicClip;
    public AudioClip clickSound;
    public int beatsPerMinute;
    public UnityEvent<int> onBeat;
    public UnityEvent songEnd;
    public float Frequency => 60f / beatsPerMinute;
    [Range(0, 1)] public float leniency = 0.5f;
    public bool IsPlaying => _isPlaying;
    public float StartTime => StartTime;
    public float BeatFrac => (GetTrackTime() % Frequency) / Frequency;
    
    private AudioSource _audioSource;
    private double _startTime;
    private float _prevFrac;
    private bool _isPlaying;
    private int _beatCounter;
    public int finalBeat;
    private float GetTrackTime()
    {
        return (float)(AudioSettings.dspTime - _startTime);
    }

    private void OnBeat()
    {
        onBeat.Invoke(_beatCounter);
        if (_beatCounter >= finalBeat)
        {
            StopBeat();
            songEnd.Invoke();
        }

        _beatCounter++;
        
    }

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
            var score = ValidateInput();
            if (Input.GetKeyDown(KeyCode.W))
            {
                _audioSource.PlayOneShot(clickSound);
                if (score > leniency)
                {
                    Debug.Log($"Nice Shot! {score} : {GetNearestBeat() % 8}");
                }
            }

            if (BeatFrac < _prevFrac)
            {
                OnBeat();
            }
            _prevFrac = BeatFrac;
        }
    }

    public void StartBeat()
    {
        _isPlaying = true;
        _startTime = AudioSettings.dspTime;
        _audioSource.PlayOneShot(musicClip);
        _beatCounter = 0;
        OnBeat();
    }

    public void StopBeat()
    {
        _isPlaying = false;
        _audioSource.Stop();
    }

    public float ValidateInput()
    {
        if (!_isPlaying) return 0;
        var score = Mathf.Abs(BeatFrac - .5f) * 2;
        return score;
    }

    public int GetNearestBeat()
    {
        return _beatCounter + Mathf.RoundToInt(BeatFrac) - 1;
    }
}
