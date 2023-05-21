using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PanAnim : MonoBehaviour
{
    public CookAnimationManager cookManager;

    private bool _isControlled = false;

    private void Start()
    {
        cookManager.onPanRaise.AddListener(() => _isControlled = true);
        cookManager.onPanLower.AddListener(() => _isControlled = false);
    }

    private void Update()
    {
        if (_isControlled)
        {
            transform.position = cookManager.Egg.position;
            transform.rotation = cookManager.Egg.rotation;
            // transform.localEulerAngles += new Vector3(0, 90, 0);
        }
    }
}
