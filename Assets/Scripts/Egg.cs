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
        Lifted,
        Cracked,//can pour
        Overcracked
    }
    public EggState _state;
    public int HITS_TO_CRACK; //how many times to crack
    public bool FLOATING;//lifts itself
    public bool TOUGH;//lift twice
    public string DESC;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="high"> wether egg was lifted higher than usual</param>
    public void Crack( bool lifted, bool high)
    {
        


    }
    /// <summary>
    /// returns wether you let it fall into the bowl by trying to pour when it's not cracked
    /// </summary>
    /// <returns></returns>
    public bool  FinishEggCheckForMess()
    {
        if (_state == EggState.Cracked)
        {
            return false;
        }
        return true; //if we let it fall into the  pan/bown
    }




}



