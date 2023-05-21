using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// exclusively handles egg state transitions leaving timing and other stuff to PlayManager
/// </summary>
public class EggManager : MonoBehaviour
{

    [HideInInspector] public Egg currentEgg;
    public AudioClip EggCrackSound, EggPanSound, EggLiftSound, SoundClick,RottenEggSound, ShatterEggSound, ShieldedEggSound, ToughEggSound, GarbageSound;

    public AudioSource player;
    [SerializeReference] List<GameObject> eggPrefabs = new();

    public void GetNewEgg()
    {

        currentEgg = Instantiate(eggPrefabs[Random.Range(0, eggPrefabs.Count)], HandController.Instance.eggSpawner.transform).GetComponent<Egg>();
        HandController.Instance.InitiateEggInTheDarkArts((currentEgg.bottomPart, currentEgg.upperPart));
    }

    void DisposeOfEgg()
    {
        Debug.Log("Just disposed of an egg.");
        currentEgg.gameObject.SetActive(false);

        Destroy(currentEgg.upperPart);
        Destroy(currentEgg.bottomPart);
        Destroy(currentEgg);

        currentEgg = null;




    }



    public void Pan()
    {
        

        if (currentEgg == null)
        {
            
            return;
        }
       
        switch (currentEgg._state)
        {
            case Egg.EggState.Intact:
                PlayManager.Instance.eggShells += 1;
                PlayManager.Instance.eggsCracked++;
                PlayManager.Instance.PopUpWrong();
                HandController.Instance.HandAnimate_Pan(false); break;
            case Egg.EggState.Rotten:
                PlayManager.Instance.eggsCracked++;
                player.PlayOneShot(RottenEggSound);
                HandController.Instance.HandAnimate_Pan(false);
                PlayManager.Instance.PopUpWrong(); PlayManager.Instance.usedRotten = true; break;
            case Egg.EggState.Cracked:
                PlayManager.Instance.eggsCracked++;
                PlayManager.Instance.PopUpCorrect();
                HandController.Instance.HandAnimate_Pan(true);
                break;
            case Egg.EggState.Shattered:
                PlayManager.Instance.eggsCracked++;
                PlayManager.Instance.eggShells += 1;
                PlayManager.Instance.PopUpWrong();
                HandController.Instance.HandAnimate_Pan(false); break;
            default:
                break;
        }

       
        DisposeOfEgg();
    }


    public void Raise()
    {
        if (currentEgg == null)
        {
            return;
        }
        HandController.Instance.HandAnimate_Raise();
        

        player.PlayOneShot(EggLiftSound);
        PlayManager.Instance.PopUpRaise();
       
    }
    public void Garbage()
    {
        if (currentEgg == null)
        {
            return;
        }
        if (currentEgg._state == Egg.EggState.Rotten)
        {
            PlayManager.Instance.PopUpCorrect();

        }
       
        player.PlayOneShot(GarbageSound);
        HandController.Instance.HandAnimate_Bin();
        DisposeOfEgg();
    }

    public void Crack()
    {
        if (currentEgg != null)
        {
           
           

            //all done here
            switch (currentEgg.Crack())
            {
                case Egg.EggState.Intact:
                    IntactEggFeedback();
                    PlayManager.Instance.PopUpWrong();
                    HandController.Instance.HandAnimate_Crack(false);
                    break;
                case Egg.EggState.Rotten:
                    PlayManager.Instance.PopUpWrong();
                    HandController.Instance.HandAnimate_Crack(false);
                    RottenEggFeedback();
                    break;
                case Egg.EggState.Cracked:
                    PlayManager.Instance.PopUpCrack();
                    HandController.Instance.HandAnimate_Crack(true);
                    CrackEggFeedback();
                    //play crack sound
                    break;
                case Egg.EggState.Shattered:
                    PlayManager.Instance.PopUpWrong();
                    HandController.Instance.HandAnimate_Crack(false);
                    ShatterEggFeedback();
                    //play loud crack sound
                    break;
                default:
                    break;
            }

        }

    }



    void RottenEggFeedback()
    {
        player.PlayOneShot(RottenEggSound);
    }
    void CrackEggFeedback()
    {
        player.PlayOneShot(GarbageSound);

    }
    void ShatterEggFeedback()
    {
        player.PlayOneShot(ShatterEggSound);

    }
    void IntactEggFeedback() //for shielded one
    {
        if (currentEgg.SHIELDED)
        {
            player.PlayOneShot(ShieldedEggSound);

            return;
        }
        if (currentEgg.TOUGH)
        {
            player.PlayOneShot(ToughEggSound);

            return;
        }

        

    }


}
