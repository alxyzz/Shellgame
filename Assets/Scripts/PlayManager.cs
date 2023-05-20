using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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



    TextMeshProUGUI LastMove;
    Sprite spr_none, spr_a, spr_b;
    #endregion
    #region Variables
    bool playing;
    float elapsedTime;

    [SerializeReference] List<Egg> allEggs = new();

    #endregion
    #region Unity Native
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

    void Start()
    {
        StartCoroutine(delayedStart());
    }


    int actionNumber; //if this is 3 and we don't act for x time, just throw it into the bowl








    void FixedUpdate()
    {


        Process();
    }

    #endregion



    int tick;
    int maxTick = 4; //rework egg if you want to use more
    List<bool?> lastInputs = new();
    [SerializeReference] List<TextMeshProUGUI> lastInputsTxt = new();
    [HideInInspector]public Egg currentEgg;
    [SerializeReference] TextMeshProUGUI tickLabel;
    [SerializeReference] PopUp PopUpAdvisor;

    Egg GetRandomEgg()
    {
        currentEgg = Instantiate(allEggs[Random.Range(0, allEggs.Count)], HandController.Instance.pickArea.transform).GetComponent<Egg>();
        return currentEgg;
    }

    void Process()
    {
        if (!playing)
        {//when paused or have won
            return;
        }
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= GameManager.Instance.processInterval)
        {//periodic tick
            tick++;//every processInterval seconds.
            tickLabel.text = tick.ToString();

            if (currentEgg == null)
            {
                currentEgg = GetRandomEgg();
            }

            if (tick >= maxTick)
            {//if 4 ticks have accumulated
                CheckIfvalidAndThrowEggInBowl();
                tick = 0;
                currentEgg = null;
            }


            return;

        }
        elapsedTime = 0;
        Debug.Log("Turn happened.");
    }


    void CheckIfvalidAndThrowEggInBowl()
    {
        if (currentEgg.CheckifValid())
        {
            GameManager.Instance.Score++;
        }
        else
        {
            ShowPlayerWasWrong();
        }

        CheckVictory();
    }

    void ShowPlayerWasWrong()
    {
        PopUpAdvisor.Appear("Wrong combo!");
    }

    IEnumerator delayedStart()
    {
        yield return new WaitForSecondsRealtime(GameManager.Instance.timeToStart);
        playing = true;
        PopUpAdvisor.Appear("Game has started. Time is now processing.");

        GetRandomEgg();
    }






    void CheckVictory()
    {
        if (GameManager.Instance.Score >= 15)
        {
            playing = false;
            StartCoroutine(WinDelay());
        }
    }


    IEnumerator WinDelay()
    {

        PopUpAdvisor.Appear("Player just won. Ending in 3..");
        yield return new WaitForSecondsRealtime(1f);

        PopUpAdvisor.Appear("Player just won. Ending in 2..");
        yield return new WaitForSecondsRealtime(1f);

        PopUpAdvisor.Appear("Player just won. Ending in 1..");
        yield return new WaitForSecondsRealtime(1f);
        GameManager.Instance.Win();



    }



}
