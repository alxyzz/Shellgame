using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateGoodEnding : MonoBehaviour
{

    public Sprite end1, end2;
    public Image target;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startOscilating());
    }

    IEnumerator startOscilating()
    {

        while (true)
        {
            target.sprite = end1;
            yield return new WaitForSecondsRealtime(1f);
            target.sprite = end2;
            yield return new WaitForSecondsRealtime(1f);

        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
