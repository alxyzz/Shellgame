using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{



    

    void Start()
    {
       
  

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
        //menuMusic.Stop();
        //foreach (var item in playObjects)
        //{ //just so we dont have to deactivate them all the time in the editor
        //    item.SetActive(true);
        //}
        //endings.SetActive(false);
        //PlayCanvas.SetActive(true);

        //play.StartGame();
        //gameObject.SetActive(false);

        //HandController.Instance.HandAnimate_LowerPan();
        Debug.Log("game started");

    }

    public void OnClickLogo()
    {

    }

    public void OnClickStart()
    {
        StartGame();
    }

    public void OnClickOptions()
    {
        //howtoplay.SetActive(!howtoplay.activeInHierarchy);
    }

    public void OnClickCredits()
    {
        //credits.SetActive(!credits.activeInHierarchy);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

 





}
