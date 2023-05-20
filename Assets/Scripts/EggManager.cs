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
    [SerializeReference] GameObject eggSpawnerObject;

    public void GetNewEgg()
    {

        Debug.Log("Just created a new egg.");
        currentEgg = Instantiate(eggPrefabs[Random.Range(0, eggPrefabs.Count)], eggSpawnerObject.transform).GetComponent<Egg>();
        PlayManager.Instance.tooltip.Appear(currentEgg.ID + " is your new egg!");

        //currentEgg = Instantiate(eggPrefabs[Random.Range(0, eggPrefabs.Count)]).GetComponent<Egg>();

        //currentEgg.transform.SetParent(eggSpawnerObject.transform);
        //currentEgg.transform.position = Vector3.zero;



    }

    void DisposeOfEgg()
    {
        Debug.Log("Just disposed of an egg.");
        currentEgg.gameObject.SetActive(false);
        
        Destroy(currentEgg.gameObject);

        currentEgg = null;




    }



    public void Pan()
    {
        

        if (currentEgg == null)
        {
            
            return;
        }
        HandController.Instance.HandAnimate_Pan();
        switch (currentEgg._state)
        {
            case Egg.EggState.Intact:
        PlayManager.Instance.tooltip.Appear(currentEgg.ID + " was thrown in the sizzling pan whole!");
                break;
            case Egg.EggState.Rotten:
        PlayManager.Instance.tooltip.Appear(currentEgg.ID + " is contaminating the omelette now!");
                break;
            case Egg.EggState.Cracked:
        PlayManager.Instance.tooltip.Appear(currentEgg.ID + " is sizzling deliciously in the pan.");
                break;
            case Egg.EggState.Shattered:
        PlayManager.Instance.tooltip.Appear("Shards of " +currentEgg.ID+" await your tongue in the omelette.");
                break;
            default:
                break;
        }


        switch (currentEgg._state)
        {
            case Egg.EggState.Intact:
                PlayManager.Instance.eggShells += 1;
                PlayManager.Instance.eggsCracked++;

                break;
            case Egg.EggState.Rotten:
                PlayManager.Instance.eggsCracked++;
                player.PlayOneShot(RottenEggSound);
                PlayManager.Instance.usedRotten = true; break;
            case Egg.EggState.Cracked:
                PlayManager.Instance.eggsCracked++;

                break;
            case Egg.EggState.Shattered:
                PlayManager.Instance.eggsCracked++;
                PlayManager.Instance.eggShells += 1;

                break;
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
        PlayManager.Instance.tooltip.Appear(currentEgg.Raise());
       
    }
    public void Garbage()
    {
        if (currentEgg == null)
        {
            return;
        }
        PlayManager.Instance.tooltip.Appear(currentEgg.ID + " was thrown in the trash!");
        HandController.Instance.HandAnimate_Bin();
        player.PlayOneShot(GarbageSound);

        DisposeOfEgg();
    }

    public void Crack()
    {
        if (currentEgg != null)
        {
            HandController.Instance.HandAnimate_Crack();
           

            //all done here
            switch (currentEgg.Crack())
            {
                case Egg.EggState.Intact:
                    IntactEggFeedback();
                    PlayManager.Instance.tooltip.Appear(currentEgg.ID + " is hit, but is intact!");
                    break;
                case Egg.EggState.Rotten:
                    PlayManager.Instance.tooltip.Appear(currentEgg.ID + " was cracked! Gribbly juices spew out on the kitchen table.");
                    RottenEggFeedback();
                    break;
                case Egg.EggState.Cracked:
                    PlayManager.Instance.tooltip.Appear(currentEgg.ID + " was cracked!");
                    CrackEggFeedback();
                    //play crack sound
                    break;
                case Egg.EggState.Shattered:
                    PlayManager.Instance.tooltip.Appear(currentEgg.ID + " was obliterated into eggshell!");
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
