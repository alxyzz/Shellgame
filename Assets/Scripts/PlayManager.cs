using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    //eggy states
    [HideInInspector] public int eggsCracked;
    [HideInInspector] public bool usedRotten;
    public int eggShells;


    [SerializeField] public PopUp tooltip;

    //beat states
    public float BPM = 120;
    public float offset;
    float BeatInterval;
    float SubBeatInterval;
    bool canMove = true;
    float timeElapsed;
    float BeatCount;
    int BeatsInThisCycle;
    float SubBeatCount;
    float BeatsPerBar;
    float Buffer = 0.05f;
    bool pannedEgg;

    bool hasMoved;
    [SerializeReference] List<Image> beatCycleIndicator = new();
    [SerializeReference] Image beatIndicator;
    [SerializeReference] AudioClip beat;
    [SerializeReference] AudioSource player;
    [SerializeReference] EggManager eggy;
    [SerializeReference] TextMeshProUGUI txt_eggshells;
    [SerializeReference] TextMeshProUGUI txt_eggsCracked;
    [SerializeReference] Image left, right, top, down;

    [SerializeReference] TextMeshProUGUI txt_eggDesc;
    [SerializeReference] TextMeshProUGUI txt_eggName;

    [SerializeReference] GameObject BadEnding, GoodEnding, RottenEnding, CrunchyEnding;



    void ShowEnding(GameObject b)
    {

        b.SetActive(true);

    }






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
        pannedEgg = false;
        eggy.GetNewEgg();
        txt_eggName.text = eggy.currentEgg.ID;
        txt_eggDesc.text = eggy.currentEgg.DESC;
        hasMoved = true;


        ///InvokeRepeating("AfterSubBeat", 0, SubBeatInterval - Buffer);
    }

    IEnumerator DelayedRecolor(Image b, Color d)
    {

        b.color = d;
        yield return new WaitForSecondsRealtime(0.2f);
        b.color = Color.white;
    }

    void EveryBeat()
    {
        beatCycleIndicator[BeatsInThisCycle].enabled = true;
        hasMoved = false;
        player.Play();
        BeatCount++;
        BeatsInThisCycle++;

        if (BeatsInThisCycle > 5)
        {
            foreach (var item in beatCycleIndicator)
            {
                item.enabled = false;
            }
            BeatsInThisCycle = 0;
            //throw egg in pan
            if (!pannedEgg)
            {
                eggy.Pan();
            }
            

            pannedEgg = false;
            eggy.GetNewEgg();
            txt_eggName.text = eggy.currentEgg.ID;
            txt_eggDesc.text = eggy.currentEgg.DESC;

            hasMoved = true;
        }
        CheckVictoryCondition();
        //Debug.Log("beat");
    }

    void EverySubBeat()
    {
        SubBeatCount++;
        canMove = !canMove;
        beatIndicator.gameObject.SetActive(!beatIndicator.gameObject.activeInHierarchy);
        // Debug.Log("subBeat");

    }



    void CheckVictoryCondition()
    {
        if (eggsCracked >= 15)
        {
            CancelInvoke();

            if (usedRotten)
            {
                ShowEnding(RottenEnding);
                return;
            }
            if (eggShells == 0)
            {
                ShowEnding(GoodEnding);
                return;
            }
            if (eggShells < 4)
            {
                ShowEnding(CrunchyEnding);
                return;
            }
            if (eggShells > 4)
            {
                ShowEnding(BadEnding);
                return;
            }

        }
    }


    #endregion
    #region Variables



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

        txt_eggshells.text = eggShells.ToString();
        txt_eggsCracked.text = eggsCracked.ToString();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (canMove && !hasMoved)
            {
                hasMoved = true;
                Debug.Log("you hit it!");
                StopCoroutine("DelayedRecolor");
                DelayedRecolor(left, Color.green);
                eggy.Garbage();

                return;
            }
            else
            {
                StopCoroutine("DelayedRecolor");
                DelayedRecolor(left, Color.red);

                Debug.Log("[Garbage]you missed it...");
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (canMove && !hasMoved)
            {
                hasMoved = true;
                Debug.Log("you hit it!");
                StopCoroutine("DelayedRecolor");
                DelayedRecolor(right, Color.green);
                eggy.Pan();
                pannedEgg = true;
                return;
            }
            else
            {
                StopCoroutine("DelayedRecolor");
                DelayedRecolor(right, Color.red);

                Debug.Log("[Pan] you missed it...");
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (canMove && !hasMoved)
            {
                hasMoved = true;
                Debug.Log("you hit it!");
                StopCoroutine("DelayedRecolor");
                DelayedRecolor(top, Color.green);

                eggy.Raise();
                return;
            }
            else
            {
                StopCoroutine("DelayedRecolor");
                DelayedRecolor(top, Color.red);

                Debug.Log("[Raise]you missed it...");
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (canMove && !hasMoved)
            {
                hasMoved = true;
                Debug.Log("you hit it!");
                eggy.Crack();
                StopCoroutine("DelayedRecolor");
                DelayedRecolor(down, Color.green);

                return;
            }
            else
            {
                StopCoroutine("DelayedRecolor");
                DelayedRecolor(down, Color.red);
                Debug.Log("[Crack]you missed it...");
            }
        }




    }





    void Start()
    {
        Initialise(null, 60);
    }








}