using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeReference]TextMeshProUGUI text;


    public void Appear(string s)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        text.text = s;
        StartCoroutine(Disappear());

    }

    void Start()
    {
        gameObject.SetActive(false);
    }


    IEnumerator Disappear()
    {


        yield return new WaitForSecondsRealtime(5f);
        gameObject.SetActive(false);
    }

}
