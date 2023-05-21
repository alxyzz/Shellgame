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

    bool _playing;
    public bool IsPlaying
    {
        get
        {
            return _playing;
        }
    }

    //eggy states
    [HideInInspector] public int eggsCracked;
    [HideInInspector] public bool usedRotten;
    public int eggShells;


    [SerializeField]  GameObject PopUp_Correct, PopUp_Crack, PopUp_Raise, PopUp_Wrong, popUpSpawnArea;

    IEnumerator PopUpObject(GameObject b)
    {

        Vector3 origVector = b.transform.localScale;
        for (int i = 0; i < 60; i++)
        {
            b.transform.localScale = new Vector3(origVector.x + (i * 0.02f), origVector.y + (i * 0.02f), origVector.z + (i * 0.02f));
            yield return new WaitForSecondsRealtime(0.00002f);
        }

        for (int i = 60; i > 0; i--)
        {
            b.transform.localScale = new Vector3(origVector.x + (i * 0.02f), origVector.y + (i * 0.02f), origVector.z + (i * 0.02f));
            yield return new WaitForSecondsRealtime(0.00005f);
        }
        b.transform.localScale = origVector;
        
        Destroy(b);

    }



    public void PopUpCorrect()
    {
        StartCoroutine(PopUpObject(Instantiate(PopUp_Correct, popUpSpawnArea.transform)));
    }

    public void PopUpCrack()
    {
        StartCoroutine(PopUpObject(Instantiate(PopUp_Crack, popUpSpawnArea.transform)));
    }

    public void PopUpRaise()
    {
        StartCoroutine(PopUpObject(Instantiate(PopUp_Raise, popUpSpawnArea.transform)));
    }
    public void PopUpWrong()
    {
        StartCoroutine(PopUpObject(Instantiate(PopUp_Wrong, popUpSpawnArea.transform)));
    }



    //beat states
    public float BPM = 120;
    public float offset;
    public int beatsInCycle;
    float BeatInterval;
    float SubBeatInterval;
    bool canMove = true;
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
    [SerializeReference] EndingUI endingUI;
    [SerializeReference] AudioSource player;
    [SerializeReference] EggManager eggy;
    [SerializeReference] TextMeshProUGUI txt_eggshells;
    [SerializeReference] TextMeshProUGUI txt_eggsCracked;

    




  

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
        if (!_playing)
        {
            return;
        }
        txt_eggshells.text = eggShells.ToString();
        txt_eggsCracked.text = eggsCracked.ToString();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (canMove && !hasMoved)
            {
                hasMoved = true;
                //Debug.Log("you hit it!");
                
                eggy.Garbage();

                return;
            }
            else
            {
                //Debug.Log("[Garbage]you missed it...");
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (canMove && !hasMoved)
            {
                hasMoved = true;
                //Debug.Log("you hit it!");
                
                eggy.Pan();
                pannedEgg = true;
                return;
            }
            else
            {
                

                //Debug.Log("[Pan] you missed it...");
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (canMove && !hasMoved)
            {
                hasMoved = true;
                //Debug.Log("you hit it!");
                

                eggy.Raise();
                return;
            }
            else
            {
                

                //Debug.Log("[Raise]you missed it...");
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (canMove && !hasMoved)
            {
                hasMoved = true;
                //Debug.Log("you hit it!");
                eggy.Crack();
               

                return;
            }
            else
            {
                
                //Debug.Log("[Crack]you missed it...");
            }
        }




    }


    


   public void StartGame()
    {

        
        Initialise(null, 60);
    }



    void ShowEnding(GameObject b)
    {

        b.SetActive(true);

    }




    IEnumerator delayStart()
    {
        yield return new WaitForSecondsRealtime(5f);
        _playing = true;
        foreach (var item in beatCycleIndicator)
        {
            item.enabled = false;
        }
        InvokeRepeating("EveryBeat", 0, BeatInterval);
        InvokeRepeating("EverySubBeat", offset, SubBeatInterval);
        pannedEgg = false;
        hasMoved = true;
        ///InvokeRepeating("AfterSubBeat", 0, SubBeatInterval - Buffer);
    }

    public void Initialise(AudioClip clip, float bpm)
    {


        eggy.GetNewEgg();
        HandController.Instance.cook.Idle();
        // AudioPlayer.Instance.player.clip = clip;
        ///bpm = BPM;
        BeatInterval = 60.0f / BPM / 2;
        SubBeatInterval = BeatInterval / 2;
        BeatCount = 0;
        player.clip = beat;
        StartCoroutine(delayStart());
       
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
      
        player.Play();
        BeatCount++;
        BeatsInThisCycle++;

        if (BeatsInThisCycle > beatsInCycle)
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
            

            hasMoved = true;
        }
        CheckVictoryCondition();
        //Debug.Log("beat");
    }

    void EverySubBeat()
    {
        hasMoved = false;
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
            _playing = false;
            if (usedRotten)
            {
                endingUI.DoRottenEnding();
               
                return;
            }
            if (eggShells == 0)
            {
                endingUI.DoGoodEnding();
                return;
            }
            if (eggShells < 4)
            {
                endingUI.DoCrunchyENding();
                return;
            }
            if (eggShells > 4)
            {
                endingUI.DoVeryCrunchyEnding();
                return;
            }

        }
    }





}