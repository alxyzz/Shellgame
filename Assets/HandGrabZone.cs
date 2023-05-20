using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabZone : MonoBehaviour
{

    bool grabbing;

    GameObject grabbed;



    public void ToggleGrabbing(bool b)
    {
        grabbing = b;
    }

    public void Drop()
    {
        grabbing = false;
        grabbed.transform.parent = null;
    }

    void OnTriggerEnter(Collider other)
    {


        if (other.tag == "Egg")
        {
            if (!grabbing)
            {
                return;
            }
            other.GetComponent<Egg>();
            other.gameObject.transform.SetParent(this.transform);

        }
    }


}
