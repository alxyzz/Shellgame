using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CookAnimationManager : MonoBehaviour
{

    public Animator animator;
    public Transform Egg;
    public Transform TopShell;
    public Transform BottomShell;
    public Transform Head;


    void Start()
    {
        RaisePan();
        animator.speed = 2;
    }


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    Crack(true);
        //}
    }

    public void Idle()
    {
        animator.SetTrigger("Idle");
    }
    public void GetEgg()
    {
        Debug.Log("Animating GetEgg right now."); animator.SetTrigger("Get");
    }
    public void LiftEgg()
    {
        Debug.Log("Animating LiftEgg right now."); animator.SetTrigger("Lift");
    }
    public void Smack(bool success)
    {
        Debug.Log("Animating Smack right now.");
        animator.SetTrigger(success? "Smack" : "SmackTooHard");
    }
    public void Crack(bool success)
    {
        Debug.Log("Animating Crack right now.");
        animator.SetBool("CrackSuccess", success);
        animator.SetTrigger("Crack");
    }
    public void RaisePan()
    {
        animator.SetBool("ShowMenu", true);
    }
    public void LowerPan()
    {
        animator.SetBool("ShowMenu", false);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new (1,1,.8f);
        Gizmos.DrawWireSphere(Egg.position, .05f);
        Gizmos.DrawWireCube(Head.position, Vector3.one * 0.1f);
    }
    
    #endif
}
