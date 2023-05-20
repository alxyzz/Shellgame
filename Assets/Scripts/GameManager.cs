using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton


    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }



    void Awake()
    {
        if (Instance != null)
        {

            Destroy(this);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion
    #region References
    [SerializeReference] PauseUI pause;
    [HideInInspector] public PlayManager pManager;
    [SerializeReference] AudioSource soundMan;

    #endregion
    #region Variables

    List<Player> players = new();

    [SerializeField] public float timeToStart = 2;
    [SerializeField] public  float processInterval = 0.7f;
    [SerializeField] float c;
    [SerializeField] float d;
    public int Score = 0;

    //settings
    [HideInInspector] public float musicVolume;
    [HideInInspector] public float SFXvolume;


    #endregion
    #region Unity Native
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    #endregion
    #region Scene Management

    public void SceneAdvance()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Win()
    {
        SceneManager.LoadScene(2);
    }


    public void GotoMenu()
    {
        SceneManager.LoadScene(0);
    }

    #endregion
    #region PauseMenu
    public void TogglePauseMenu() 
    {

        if (pause.gameObject.activeInHierarchy)
        {
            Time.timeScale = 1f;

            pause.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            pause.gameObject.SetActive(true);

        }

    }


    #endregion






}
