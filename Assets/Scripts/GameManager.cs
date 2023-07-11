using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum PanState
{
    Lowered,
    Raised
}


public class GameManager : MonoBehaviour
{
    public InputChegger inputChegger;
    public CookAnimationManager cookAnimationManager;
    public BeatKeeper beatKeeper;
    public Transform mainCamera;
    public Transform pan;
    
    private PanState _panState = PanState.Lowered;
    
    private EggModel _currentVisualEgg;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    private float _points = 0;
    private float _eggshells = 0;
    [FormerlySerializedAs("NeededEggs")] public float _neededEggs;
    private float _eggToShellRatio;
    private bool _rottenEggAdded;
    void Awake()
    {
        if (Instance != null)
        {

            Destroy(this);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        inputChegger.onTakeEgg.AddListener(OnTakeEgg);
        inputChegger.onRaiseEgg.AddListener(cookAnimationManager.LiftEgg);
        inputChegger.onSmackEggCorrect.AddListener(() => cookAnimationManager.Smack(true));
        inputChegger.onSmackEggWrong.AddListener(() => cookAnimationManager.Smack(false));
        inputChegger.onCrackEgg.AddListener((_) => cookAnimationManager.Crack(true));
        inputChegger.onCrackEgg.AddListener(AddPoint);
        inputChegger.onWeakCrackEgg.AddListener((_) => cookAnimationManager.Crack(false));
        inputChegger.onWeakCrackEgg.AddListener((_) => AddEggshell());
        inputChegger.onWeakCrackEgg.AddListener(AddPoint);
        inputChegger.onDiscardEgg.AddListener(cookAnimationManager.Idle);
        cookAnimationManager.onPanRaise.AddListener(() => _panState = PanState.Raised);
        cookAnimationManager.onPanLowered.AddListener(() => _panState = PanState.Lowered);
        beatKeeper.songEnd.AddListener(GetEnding);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(_panState is PanState.Raised) cookAnimationManager.LowerPan();
            else if(_panState is PanState.Lowered) cookAnimationManager.RaisePan();
        }
    }

    private void LateUpdate()
    {
        CopyTransforms(cookAnimationManager.Head, mainCamera);

        if (_panState is not PanState.Lowered) 
            CopyTransforms(cookAnimationManager.Egg, pan, switchAxes: true);
        if (_currentVisualEgg is not null)
            _currentVisualEgg.UpdateTransforms(cookAnimationManager);
    }

    public static void CopyTransforms(Transform source, Transform target, bool switchAxes = false)
    {
        target.position = source.position;
        if (switchAxes) target.rotation = Quaternion.LookRotation(-source.up, source.forward);
        else target.rotation = Quaternion.LookRotation(source.forward, source.up);
    }

    private void OnTakeEgg(EggType egg)
    {
        DestroyEgg();
        _currentVisualEgg = Instantiate(egg.model);
        cookAnimationManager.GetEgg();
    }

    public void DestroyEgg()
    {
        if (_currentVisualEgg is null) return;
        Destroy(_currentVisualEgg.gameObject);
        _currentVisualEgg = null;
    }

    public void SetPanState(PanState newState)
    {
        _panState = newState;
        Debug.Log(_panState);
    }

    private void AddPoint(EggType egg)
    {
        if (egg.isRotten) _rottenEggAdded = true;
        _points++;
    }

    private void AddEggshell()
    {
        _eggshells++;
    }

    private void GetEnding()
    {
        _eggToShellRatio = _eggshells / _points;
        Debug.Log("ending triggered");
        if (_rottenEggAdded)
        {
            Debug.Log("Lugubrious ending");
            return;
        }
        else if (_points < _neededEggs)
        {
            Debug.Log("too little eggs for an omelette man");
            return;
        }
        else if (_eggToShellRatio <= 0 && _eggshells<=0)
        {
            //perfect ending
            Debug.Log("wow! perf-egg-t");
            return;
        }
        else if (_eggToShellRatio >0 && _eggToShellRatio<=0.25)
        {
            //slightly crunchy ending
            Debug.Log("a bit of added texture");
            return;
        }
        else
        {
            //very crunchy ending
            Debug.Log("that's a lotta crunch");
            return;
        }
    }
}
