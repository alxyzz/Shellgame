using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeReference] GameObject credits, howtoplay;

    int logoClicked;
    bool diff;
    [SerializeReference] Material skyboxeasteregg;
    Material regular;
    [SerializeReference]AudioPlayer b;

    void Start()
    {
        regular = RenderSettings.skybox;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
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
