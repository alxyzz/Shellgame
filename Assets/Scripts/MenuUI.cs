using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{



    [SerializeReference] AudioSource menuMusic;
    [SerializeReference] AudioClip menuMusicClip;
    [SerializeReference] PlayManager play;
    [SerializeReference] GameObject credits, howtoplay, PlayCanvas, endings;
    [SerializeReference]  List<GameObject> playObjects = new(); //hide these when start, show when playing
    int logoClicked;
    bool diff;
    [SerializeReference] Material skyboxeasteregg;
    Material regular;
    [SerializeReference]AudioPlayer b;

    void Start()
    {
        regular = RenderSettings.skybox;
        PlayCanvas.SetActive(false);
        foreach (var item in playObjects)
        {
            item.SetActive(false);
        }
        menuMusic.clip = menuMusicClip;
        menuMusic.loop = true;
        menuMusic.Play();
        //HandController.Instance.cook.RaisePan();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
        

    }

    void StartGame()
    {
        menuMusic.Stop();
        foreach (var item in playObjects)
        { //just so we dont have to deactivate them all the time in the editor
            item.SetActive(true);
        }
        endings.SetActive(false);
        PlayCanvas.SetActive(true);
        
        play.StartGame();
        gameObject.SetActive(false);

        HandController.Instance.HandAnimate_LowerPan();
    }

    public void OnClickLogo()
    {
        Debug.Log("click");
        b.PlayEggcrack();
        logoClicked++;
        if (logoClicked > 6)
        {
            if (!diff)
            {
                RenderSettings.skybox = skyboxeasteregg;
                
            }
            else
            {
                RenderSettings.skybox = regular;

            }
            diff = !diff;
            logoClicked = 0;

        }
    }

    public void OnClickStart()
    {
        StartGame();
    }

    public void OnClickOptions()
    {
        howtoplay.SetActive(!howtoplay.activeInHierarchy);
    }

    public void OnClickCredits()
    {
        credits.SetActive(!credits.activeInHierarchy);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

 





}
