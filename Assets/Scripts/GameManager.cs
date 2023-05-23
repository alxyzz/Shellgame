using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InputChegger inputChegger;
    public CookAnimationManager cookAnimationManager;
    public BeatKeeper beatKeeper;
    public Transform camera;
    public Transform pan;

    private bool _panRaised;
    private EggModel _currentVisualEgg;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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
        inputChegger.onCrackEgg.AddListener(() => cookAnimationManager.Crack(true));
        inputChegger.onWeakCrackEgg.AddListener(() => cookAnimationManager.Crack(false));
        inputChegger.onDiscardEgg.AddListener(cookAnimationManager.Idle);
        cookAnimationManager.onPanRaise.AddListener(() => _panRaised = true);
        cookAnimationManager.onPanLower.AddListener(() => _panRaised = false);

    }

    private void Update()
    {
        CopyTransforms(cookAnimationManager.Head, camera);

        if (_panRaised) 
            CopyTransforms(cookAnimationManager.Egg, pan, switchAxes: true);
        if (_currentVisualEgg is not null)
            _currentVisualEgg.UpdateTransforms(cookAnimationManager);

        if (Input.GetKeyDown(KeyCode.P))
        {
            if(_panRaised)
            cookAnimationManager.LowerPan();
            else
            {
                cookAnimationManager.RaisePan();
            }
        }

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
}
