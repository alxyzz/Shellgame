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
    public GameObject pickArea; 

    [SerializeField] float lerpSpeed = 2f;
    Vector3 startPos;
    Vector3 target;
    float lerpfactor;

    //for now

    public GameObject CrackPosition, PourPosition, BinPosition, PanPosition;

    public void HandAnimate_Crack()
    {
        startPos = transform.position;
        transform.position = CrackPosition.transform.position;
        //target = CrackPosition.transform.position;
    }

    public void HandAnimate_Pour()
    {
        startPos = transform.position;
        transform.position = PourPosition.transform.position;

        //target = PourPosition.transform.position;
    }


    public void HandAnimate_Bin()
    {
        startPos = transform.position;
        transform.position = BinPosition.transform.position;

        //target = BinPosition.transform.position;
    }

    public void HandAnimate_Pan()
    {
        startPos = transform.position;
        transform.position = PanPosition.transform.position;

        //target = PanPosition.transform.position;
    }


    void Update()
    {


     
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        //if (target != null)
        //{
        //    transform.position = Vector3.Lerp(startPos, target, lerpfactor);
        //    lerpfactor += lerpSpeed;
        //}
      
    }
}
