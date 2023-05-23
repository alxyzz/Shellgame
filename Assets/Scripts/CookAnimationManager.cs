using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum CookAnimationState
{
    Idle,
    Get,
    Lift,
    Smack,
    Crack,
    Menu
}

public class CookAnimationManager : MonoBehaviour
{

    public Animator animator;
    public Transform Egg;
    public Transform TopShell;
    public Transform BottomShell;
    public Transform Head;

    public UnityEvent onPanRaise;
    public UnityEvent onPanLowered;
    
    private CookAnimationState _state;

    public CookAnimationState State => _state;

    private void Start()
    {
        _state = CookAnimationState.Idle;
    }

    public void Idle()
    {
        _state = CookAnimationState.Idle;
        animator.SetTrigger("Idle");
    }
    public void GetEgg()
    {
        ///Debug.Log($"trying to get egg {_state}");
        if (!(_state is CookAnimationState.Idle or CookAnimationState.Crack)) return;
        _state = CookAnimationState.Get;
        ///Debug.Log("Animating GetEgg right now."); 
        animator.SetTrigger("Get");
    }
    public void LiftEgg()
    {
        if (!(_state is CookAnimationState.Get || _state is CookAnimationState.Lift)) return;
        _state = CookAnimationState.Lift;
        ///Debug.Log("Animating LiftEgg right now."); 
        animator.SetTrigger("Lift");
    }
    public void Smack(bool success)
    {
        if (!(_state is CookAnimationState.Lift || _state is CookAnimationState.Smack)) return;
        _state = CookAnimationState.Smack;
        ///Debug.Log("Animating Smack right now.");
        animator.SetTrigger(success? "Smack" : "SmackTooHard");
        if (!success) _state = CookAnimationState.Idle;
    }
    public void Crack(bool success)
    {
        if (_state is not CookAnimationState.Smack) return;
        _state = CookAnimationState.Crack;
        ///Debug.Log("Animating Crack right now.");
        animator.SetBool("CrackSuccess", success);
        animator.SetTrigger("Crack");
    }
    public void RaisePan()
    {
        if (_state is not CookAnimationState.Idle) return;
        if (_state is CookAnimationState.Menu) return;
        _state = CookAnimationState.Menu;
        onPanRaise.Invoke();
        animator.SetBool("ShowMenu", true);
    }
    public void LowerPan()
    {
        if (_state is not CookAnimationState.Menu) return;
        _state = CookAnimationState.Idle;
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
