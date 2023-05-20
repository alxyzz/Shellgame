using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

     public GameObject boundaryleft, boundaryright, boundaryfront, boundaryback;

    public float movespeedLeftRight = 2f;
    public float movespeedFrontBack = 2f;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void FixedUpdate()
    {

        float translation = Input.GetAxis("Mouse Y") * movespeedFrontBack;
      
        float rotation = Input.GetAxis("Mouse X") * movespeedLeftRight;

        // Make it move 10 meters per second instead of 10 meters per frame...

        Vector3 movement = new Vector3(Mathf.Clamp(transform.position.x + rotation, boundaryleft.transform.position.x, boundaryright.transform.position.x),
            transform.position.y, (Mathf.Clamp(transform.position.z + translation, boundaryback.transform.position.z,
            boundaryfront.transform.position.z)));






        transform.position = movement;
    }

}
