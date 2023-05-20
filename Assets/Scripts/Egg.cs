using System.Collections.Generic;
using UnityEngine;


public enum Movement { crack, pour, none }

/// <summary>
/// egg class. moves are handled with AddNewMove() when a tick passes and the move has been picked or not picked.
/// </summary>
public class Egg : MonoBehaviour
{
    public int crackAmt;
    public int pourAmt;
    public bool floatsUp;
    
     public string description;

    public Egg()
    {
      crackAmt = 1;
      pourAmt = 1;
    description = "Just your regular, run-of-the-mill egg.";
    }

    public bool CheckifValid()
    {
        if (crackAmt != 0 && pourAmt != 0)
        {
            return false;
        }
        return true;
    }


    public void Crack()
    {
        crackAmt--;
    }

    public void Pour()
    {
        pourAmt--;
    }




}



