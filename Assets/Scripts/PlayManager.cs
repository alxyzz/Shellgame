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
    public float BPM;
    public float offset;
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




    public void Initialise(AudioClip clip, float bpm)
    {
        // AudioPlayer.Instance.player.clip = clip;
        ///bpm = BPM;
        BeatInterval = 60.0f / BPM;
        SubBeatInterval = BeatInterval / 4;
        BeatCount = 0;
       player.clip = beat;

        InvokeRepeating("EveryBeat", 0, BeatInterval);
        InvokeRepeating("EverySubBeat", offset, SubBeatInterval);
        
        ///InvokeRepeating("AfterSubBeat", 0, SubBeatInterval - Buffer);
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
        beatIndicator.enabled = !beatIndicator.enabled;
        Debug.Log("subBeat");

    }

    void AfterSubBeat()
    {
        beatIndicator.enabled = true;

        Debug.Log("subBeatEnd");

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (beatIndicator.enabled)
            {
                Debug.Log("you hit it!");
            }
            else
            {
                Debug.Log("you missed it...");
            }
        }


    }

    void Start()
    {
        Initialise(null, 60);
    }








}
