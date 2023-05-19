using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeReference] PauseUI uiPause;
    [SerializeReference] GameObject credits;


    public void OnClickStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void OnClickOptions()
    {
        //uiPause.PopUp();
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
