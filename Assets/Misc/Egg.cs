using UnityEngine;


public enum Movement { A, B, none }


[CreateAssetMenu(fileName = "Lay New Egg", menuName = "Egg")]
public class Egg : ScriptableObject
{
    public Movement first;
    public Movement second;
    public Movement third;
    public Movement fourth;

    public string description;

    public bool CheckifValid(string f, string s, string t, string fth)
    {
        if (first == check(f) && second == check(s) && third == check(t) && fourth == check(fth))
        {
            return true;
        }
        return false;


        Movement check(string incoming)
        {
            switch (incoming)
            {

                case "a":
                    return Movement.A;
                case "b":
                    return Movement.B;
                case "none":
                    return Movement.none;
            }
            return Movement.none;
        }



    }
}
