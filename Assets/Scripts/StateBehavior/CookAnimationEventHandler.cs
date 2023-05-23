using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookAnimationEventHandler : MonoBehaviour
{

    public void DestroyEgg()
    {
        Debug.Log("Destroy Egg from Event");
        GameManager.Instance.DestroyEgg();
    }
}
