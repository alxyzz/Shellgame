using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public BeatKeeper beatKeeper;
    public List<EggType> eggs;

    private EggProcessState _state = EggProcessState.None;
    public EggProcessState State => _state;
    
    private EggType _egg;
    private int _currentRaises;
    private int _currentSmacks;
    private bool _beatUsed;

    private void Start()
    {
        beatKeeper.onBeat.AddListener((_) => _beatUsed = false);
    }

    public void StartProcess(EggType egg)
    {
        if (State is not EggProcessState.None)
        {
            Debug.Log("Already egg there yo.");
            return;
        }
        _egg = egg;
        _currentRaises = 0;
        _currentSmacks = 0;
        _state = EggProcessState.Taken;
        Debug.Log($"Starting Process. Current Egg: {egg.name}");
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
    }

    private void SmackEgg()
    {
        _state = EggProcessState.Smacked;
        _currentSmacks++;
        if (_currentRaises > _egg.raises)
        {
            SmashEgg();
            return;
        }

        if (_currentSmacks > _egg.smacks)
        {
            SmashEgg();
            return;
        }
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
        
        if (_egg.isRotten) Debug.Log("Yuckies");
        else Debug.Log("Yummies");
        EndProcess();
    }

    private void SmashEgg()
    {
        Debug.Log("OH NOOOOOOOO egg is kil");
        EndProcess();
    }

    private void CrackFailed()
    {
        Debug.Log("Not enough force");
        EndProcess();
    }

    private void DiscardEgg()
    {
        Debug.Log("Get de fuck outa here");
        EndProcess();
    }
    
    public void PerformAction(int index)
    {
        var score = beatKeeper.ValidateInput();
        if (score < beatKeeper.leniency) return;
        _beatUsed = true; //TODO maybe get working properly
        if (State == EggProcessState.None)
        {
            Debug.Log("Cannot Perform action without starting sequence");
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
            Debug.Log($"State: {State}, Raises:{_currentRaises}, Smacks: {_currentSmacks}");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)) StartProcess(eggs[Random.Range(0, eggs.Count)]);
        
        if(Input.GetKeyDown(KeyCode.UpArrow)) PerformAction(index: 0);
        if(Input.GetKeyDown(KeyCode.DownArrow)) PerformAction(index : 1);
        if(Input.GetKeyDown(KeyCode.LeftArrow)) PerformAction(index: 2);
    }
}
