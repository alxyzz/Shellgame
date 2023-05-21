using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingUI : MonoBehaviour
{

    List<AudioSource> allOtherSound = new();
    [SerializeReference] AudioSource endingPlayer;
    [SerializeReference] AudioClip drums, tadaa;

    [SerializeReference] GameObject preEnding, crunchyEnding, verycrunchyEnding, rottenEnding, GOODending;



    void DoGeneralEndingStuff()
    {

        Debug.Log("DoGeneralEndingStuff() ran");
        foreach (var item in allOtherSound)
        {
            item.mute = true;
        }

        endingPlayer.mute = false;


        gameObject.SetActive(true);
    }


    IEnumerator ending(GameObject desiredEnding)
    {
        preEnding.SetActive(true);
        //playdrumbeat
        yield return new WaitForSecondsRealtime(1.5f);
        preEnding.SetActive(false);
        desiredEnding.SetActive(true);


        //tadaa
    }



    public void DoGoodEnding()
    {
        DoGeneralEndingStuff();
        StartCoroutine(ending(GOODending));

    }
    public void DoRottenEnding()
    {

        DoGeneralEndingStuff();
        StartCoroutine(ending(rottenEnding));
    }
    public void DoCrunchyENding()
    {
        DoGeneralEndingStuff();
        StartCoroutine(ending(crunchyEnding));
    }
    public void DoVeryCrunchyEnding()
    {
        DoGeneralEndingStuff();
        StartCoroutine(ending(verycrunchyEnding));
    }



    public void OnClickPlayAgain()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }


    public void OnClickQuit()
    {
        Application.Quit();
    }



}
