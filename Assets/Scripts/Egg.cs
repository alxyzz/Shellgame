using UnityEngine;


public enum Movement { crack, pour, none }

/// <summary>
/// egg class. moves are handled with AddNewMove() when a tick passes and the move has been picked or not picked.
/// </summary>
public class Egg : MonoBehaviour
{
    public enum EggState
    {
        Intact,//crack, pour
        Rotten,//just dispose
        Cracked,//can pour
        Shattered
    }

    public EggState _state;
    public int HITS_TO_CRACK; //how many times to crack
    public bool TOUGH;//lift twice
    public bool SHIELDED;//crack twice
    public int HEIGHT_LIFTED; //0, 1 or 2
    public string DESC;
    public string ID;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="high"> wether egg was lifted higher than usual</param>
    public EggState Crack()
    {
        if (SHIELDED)
        {
            SHIELDED = false;
            HEIGHT_LIFTED = 0;
            return EggState.Intact;
        }

        if (TOUGH)
        {
            if (HEIGHT_LIFTED < 2)
            {
                return EggState.Intact;
            }
            HITS_TO_CRACK--;
            if (HITS_TO_CRACK == 0)
            {
                HEIGHT_LIFTED = 0;
                _state = EggState.Cracked;
            }
            else if (HITS_TO_CRACK < 0)
            {
                _state = EggState.Shattered;
            }

        }

       
        else if (HEIGHT_LIFTED > 0)
        {
            HITS_TO_CRACK--;
            if (HITS_TO_CRACK == 0)
            {
                _state = EggState.Cracked;
            }
            else if (HITS_TO_CRACK < 0)
            {
                _state = EggState.Shattered;
            }
          
        }

        HEIGHT_LIFTED = 0;
        return _state;
    }


    public string Raise()
    {
       
        HEIGHT_LIFTED = Mathf.Clamp(HEIGHT_LIFTED + 1, 0, 2);
        return ID + " was lifted " + HEIGHT_LIFTED.ToString() + " times!";
    }



    /// <summary>
    /// returns true if no eggshells in omelette
    /// </summary>
    /// <returns></returns>
    public bool  FinishCheckSuccess()
    {
        if (_state == EggState.Cracked)
        {
            return true;
        }
        return false; //if we let it fall into the  pan/bown
    }




}



