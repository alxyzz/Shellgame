using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //what this one should do: let user move hand around in a limited area - no object interaction
    private static HandController _instance;

    public static HandController Instance
    {
        get
        {
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);

        }
        else
        {
            _instance = this;
        }


    }


    [SerializeField] float animSpeed = 1f;

    //for now

    public CookAnimationManager cook;
    public GameObject egg_bottom_part, egg_top_part, eggSpawner;



    public void InitiateEggInTheDarkArts((GameObject, GameObject) bottomupperpart)
    {
        HandAnimate_GetEgg();
        bottomupperpart.Item1.transform.SetParent(egg_bottom_part.transform);
        bottomupperpart.Item2.transform.SetParent(egg_top_part.transform);
    }

   
    public void HandAnimate_Crack(bool success)
    {
        cook.Smack(success);

    }

    public void HandAnimate_GetEgg()
    {
        cook.GetEgg();

    }

    public void HandAnimate_Raise()
    {
        cook.LiftEgg();


        //target = PourPosition.transform.position;
    }


    public void HandAnimate_Bin()
    {
        //cook.Garbage(success);


        //target = BinPosition.transform.position;
    }

    public void HandAnimate_Pan(bool success)
    {
        cook.Crack(success);
        //target = PanPosition.transform.position;
    }

    
}
