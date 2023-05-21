using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{



    [SerializeReference] PlayManager play;
    [SerializeReference] GameObject credits, howtoplay, PlayCanvas;
    int logoClicked;
    bool diff;
    [SerializeReference] Material skyboxeasteregg;
    Material regular;
    [SerializeReference]AudioPlayer b;

    void Start()
    {
        regular = RenderSettings.skybox;
        PlayCanvas.SetActive(false);

    }



    void StartGame()
    {
        PlayCanvas.SetActive(true);
        play.StartGame();
        gameObject.SetActive(false);
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

    public void OnClickControls()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }





}
