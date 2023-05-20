using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    private static PlayManager _instance;
    public static PlayManager Instance
    {
        get
        {
            return _instance;
        }
    }

    #region References

    //states
    float BPM = 60;
    float sweet_spot = 0.5f;
    float BeatInterval;
    float SubBeatInterval;

    float timeElapsed;
    float BeatCount;
    float SubBeatCount;
    float BeatsPerBar;
    float Buffer = 0.05f;

    [SerializeReference] Image beatIndicator;
    [SerializeReference] AudioClip beat;
    [SerializeReference] AudioSource player;




    public void Initialise(AudioClip clip, int bpm)
    {
        // AudioPlayer.Instance.player.clip = clip;
        BPM = bpm;
        BeatInterval = 60.0f / bpm;
        SubBeatInterval = BeatInterval / 2;
        BeatCount = 0;
       player.clip = beat;

        InvokeRepeating("EveryBeat", 0, BeatInterval);
        InvokeRepeating("EverySubBeat", 0, SubBeatInterval);
        InvokeRepeating("AfterSubBeat", 0, SubBeatInterval - Buffer);
    }

    void EveryBeat()
    {
        player.Play();
        BeatCount++;
        Debug.Log("beat");
    }

    void EverySubBeat()
    {
        SubBeatCount++;
        beatIndicator.enabled = false;
        Debug.Log("subBeat");

    }

    void AfterSubBeat()
    {
        beatIndicator.enabled = true;

        Debug.Log("subBeat");

    }


    #endregion
    #region Variables
    bool playing;
    float elapsedTime;
    Egg currentEgg;


    #endregion
    void Awake()
    {

        if (_instance != null)
        {
            Destroy(this);

        }
        else
        {
            _instance = this;
        }


    }


    void Update()
    {

    }

    void Start()
    {
        Initialise(null, 60);
    }








}
