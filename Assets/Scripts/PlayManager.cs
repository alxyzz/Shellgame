using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    #region References






    #endregion
    #region Variables
    bool playing;
    float elapsedTime;

public List<Egg> allEggs = new();

    #endregion
    #region Unity Native
    void Awake()
    {

        //GameManager.Instance.pManager = this;
    }

    void Start()
    {
        StartCoroutine(delayedStart());
    }


    void FixedUpdate()
    {


        HandleInput();
        Process();
    }

    #endregion

    Egg getRandomEgg()
    {
        return allEggs[Random.Range(0, allEggs.Count)];
    }



    enum LastInput
    {
        none,
        a,
        b
    }


    LastInput _lastInput = LastInput.none;

    Image LastMove;
    Sprite spr_none, spr_a, spr_b;

    

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {//a

            Debug.LogWarning("Just pressed button A. Tick is " + tick.ToString());
            switch (_lastInput)
            {
                case LastInput.none:
                    lastInputs[tick] = "none";
                    LastMove.sprite = spr_none;
                    lastInputsTxt[tick].text = "none";
                    break;
                case LastInput.a:
                    lastInputs[tick] = "a";
                    LastMove.sprite = spr_a;
                    lastInputsTxt[tick].text = "a";
                    break;
                case LastInput.b:
                    lastInputs[tick] = "b";
                    LastMove.sprite = spr_b;
                    lastInputsTxt[tick].text = "b";
                    break;

                default:

                    lastInputs[tick] = "none";
                    LastMove.sprite = spr_none;
                    lastInputsTxt[tick].text = "none";

                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {//b
            Debug.LogWarning("Just pressed button B. Tick is " + tick.ToString());

            switch (_lastInput)
            {
                case LastInput.none:
                    lastInputs[tick] = "none";
                    LastMove.sprite = spr_none; lastInputsTxt[tick].text = "none"; break;
                case LastInput.a:
                    lastInputs[tick] = "a";
                    LastMove.sprite = spr_a; lastInputsTxt[tick].text = "a"; break;
                case LastInput.b:
                    lastInputs[tick] = "b";
                    LastMove.sprite = spr_b; lastInputsTxt[tick].text = "b"; break;
                default:
                    lastInputs[tick] = "none";
                    LastMove.sprite = spr_none; lastInputsTxt[tick].text = "none"; break;

                    break;
            }

        }
    }
   


    int tick;
    int maxTick = 4; //rework egg if you want to use more
    List<string> lastInputs = new();
   [SerializeReference] List<TextMeshProUGUI> lastInputsTxt = new();
    Egg lastEgg;
    [SerializeReference]TextMeshProUGUI tickLabel, eggRequirementList;
    [SerializeReference] PopUp PopUpAdvisor;


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

            if (lastEgg == null)
            {
                lastEgg = getRandomEgg();
                eggRequirementList.text = lastEgg.description;
            }

            if (tick == maxTick)
            {//if 4 ticks have accumulated
                CheckLastValidity();
                tick = 0;
            }
           
           
            return;

        }
        elapsedTime = 0;
        Debug.Log("Turn happened.");
    }


    void CheckLastValidity()
    {
        if (lastEgg.CheckifValid(lastInputs[0], lastInputs[1], lastInputs[2], lastInputs[3]))
        {
            GameManager.Instance.Score++;
        }
        else
        {
            ShowPlayerWasWrong();
        }
        lastEgg = getRandomEgg();
        eggRequirementList.text = lastEgg.description;
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
