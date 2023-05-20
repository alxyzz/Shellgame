using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //what this one should do: let user move hand around in a limited area - no object interaction
    private static HandController _instance;

    public static HandController Instance
    {
        get
        {
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);

        }
        else
        {
            _instance = this;
        }


    }


    [SerializeField] float lerpSpeed = 2f;
    Vector3 startPos;
    Vector3 target;
    float lerpfactor;

    //for now

    public GameObject pos_crack, pos_raise, pos_raise2, pos_garbage, pos_pan;
    public GameObject THE_HAND;

     Vector3 OrigPos;
    void Move(GameObject a, GameObject b)
    {
        StopAllCoroutines();
        a.transform.position = b.transform.position;
        StartCoroutine(returnToRestingPlace());

    }


    IEnumerator returnToRestingPlace()
    {
        yield return new WaitForSecondsRealtime(0.4f);

        THE_HAND.transform.position = startPos;


    }
    public void HandAnimate_Crack()
    {
        Move(THE_HAND, pos_crack);
        //target = CrackPosition.transform.position;

        //animate here later
    }

    public void HandAnimate_Raise()
    {
        Move(THE_HAND, pos_raise);


        //target = PourPosition.transform.position;
    }


    public void HandAnimate_Bin()
    {
        Move(THE_HAND, pos_garbage);


        //target = BinPosition.transform.position;
    }

    public void HandAnimate_Pan()
    {
        Move(THE_HAND, pos_pan);
        //target = PanPosition.transform.position;
    }




    // Start is called before the first frame update
    void Start()
    {
        startPos = THE_HAND.transform.position;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
      
    }
}
