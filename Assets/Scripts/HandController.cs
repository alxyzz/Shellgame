using UnityEngine;

public class HandController : MonoBehaviour
{

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
    public GameObject boundaryleft, boundaryright, boundaryUp, boundaryDown;
    public GameObject pickArea;
    public float movespeedLeftRight = 2f;
    public float movespeedFrontBack = 2f;
    [HideInInspector]public bool hasLiftedEgg, hasLiftedEggHigh;//checks wether the hand was physically moved higher to crack the egg. or even higher for stronger eggs


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        float translation = 0;
        if (PlayManager.Instance.currentEgg == null)
        {
            translation = Input.GetAxis("Mouse Y") * movespeedFrontBack;

        }
        else if (PlayManager.Instance.currentEgg.floatsUp)
        {
            translation = 0.05f + Input.GetAxis("Mouse Y") * movespeedFrontBack;
        }
        else
        {
            translation = Input.GetAxis("Mouse Y") * movespeedFrontBack;

        }



        float rotation = Input.GetAxis("Mouse X") * movespeedLeftRight;

        // Make it move 10 meters per second instead of 10 meters per frame...

        Vector3 movement = new Vector3(Mathf.Clamp(
            transform.position.x + rotation, boundaryleft.transform.position.x, boundaryright.transform.position.x),
            Mathf.Clamp(transform.position.y + translation, boundaryDown.transform.position.y,
            boundaryUp.transform.position.y),
            transform.position.z);

        transform.position = movement;
    }








}
