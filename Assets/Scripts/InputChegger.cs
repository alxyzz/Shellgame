using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;

public enum EggProcessState
{
    None,
    Taken,
    Raised,
    Smacked,
    Cracked
}

public class InputChegger : MonoBehaviour
{
    public List<EggType> eggs;

    private EggProcessState _state = EggProcessState.None;
    public EggProcessState State => _state;
    public BeatKeeper BeatKeeper => GameManager.Instance.beatKeeper;
    
    private EggType _egg;
    private int _currentRaises;
    private int _currentSmacks;
    private bool _beatUsed;

    public UnityEvent<EggType> onTakeEgg;
    public UnityEvent onRaiseEgg;
    public UnityEvent onSmackEggCorrect;
    public UnityEvent onSmackEggWrong;
    public UnityEvent<EggType> onCrackEgg;
    public UnityEvent onDiscardEgg;
    public UnityEvent<EggType> onWeakCrackEgg;

    private void Start()
    {
        BeatKeeper.onBeat.AddListener((_) => _beatUsed = false);
    }

    public void TakeEgg(EggType egg)
    {
        
        if (State is not EggProcessState.None)
        {
            // Debug.Log("Already egg there yo.");
            return;
        }
        _egg = egg;
        _currentRaises = 0;
        _currentSmacks = 0;
        _state = EggProcessState.Taken;
        // Debug.Log($"Starting Process. Current Egg: {egg.name}");
        onTakeEgg.Invoke(egg);
    }

    private void EndProcess()
    {
        _egg = null;
        _state = EggProcessState.None;
    }

    private void RaiseEgg()
    {
        _state = EggProcessState.Raised;
        _currentRaises++;
        onRaiseEgg.Invoke();
    }

    private void SmackEgg()
    {
        _state = EggProcessState.Smacked;
        _currentSmacks++;
        if (_currentRaises > _egg.raises || _currentSmacks > _egg.smacks)
        {
            SmashEgg();
            return;
        }
        onSmackEggCorrect.Invoke();
    }

    private void CrackEgg()
    {
        if (_currentRaises < _egg.raises)
        {
            CrackFailed();
            return;
        }

        if (_currentSmacks < _egg.smacks)
        {
            CrackFailed();
            return;
        }
        
        // if (_egg.isRotten) Debug.Log("Yuckies");
        ///else Debug.Log("Yummies");
        onCrackEgg.Invoke(_egg);
        EndProcess();
    }

    private void SmashEgg()
    {
        ///Debug.Log("OH NOOOOOOOO egg is kil");
        onSmackEggWrong.Invoke();
        EndProcess();
    }

    private void CrackFailed()
    {
        // Debug.Log("Not enough force");
        onWeakCrackEgg.Invoke(_egg);
        EndProcess();
    }

    private void DiscardEgg()
    {
        ///Debug.Log("Get de fuck outa here");
        onDiscardEgg.Invoke();
        EndProcess();
    }
    
    public void PerformAction(int index)
    {
        var score = BeatKeeper.ValidateInput();
        if (score < BeatKeeper.leniency) return;
        _beatUsed = true; //TODO maybe get working properly
        if (State == EggProcessState.None)
        {
            ///Debug.Log("Cannot Perform action without starting sequence");
            return;
        }
        if (index == 0) //Up
        {
            switch (State)
            {
                case EggProcessState.Taken:
                    RaiseEgg();
                    break;
                case EggProcessState.Raised:
                    RaiseEgg();
                    break;
                case EggProcessState.Smacked:
                    CrackEgg();
                    break;
            }
        }
        else if (index == 1) //Down
        {
            switch (State)
            {
                case EggProcessState.Raised:
                    SmackEgg();
                    break;
                case EggProcessState.Smacked:
                    SmackEgg();
                    break;
            }
        }
        else if (index == 2) // Left
        {
            DiscardEgg();
        }
        /// .Log($"State: {State}, Raises:{_currentRaises}, Smacks: {_currentSmacks}");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) TakeEgg(eggs[Random.Range(0, eggs.Count)]);

        if (!_beatUsed)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) PerformAction(index: 0);
            if (Input.GetKeyDown(KeyCode.DownArrow)) PerformAction(index: 1);
            if (Input.GetKeyDown(KeyCode.RightArrow)) PerformAction(index: 2);
        }
    }
}
