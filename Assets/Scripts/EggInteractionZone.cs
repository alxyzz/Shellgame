using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggInteractionZone : MonoBehaviour
{


    public enum EggInteractionZoneType
    {
        HighLift,Lift,Crack,Bowl
    }

    public EggInteractionZoneType type;

    public bool high;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "egg")
        {
            switch (type)
            {
                case EggInteractionZoneType.HighLift:
                    HandController.Instance.hasLiftedEggHigh = true;
                    Debug.LogWarning("Just lifted egg up high.");


                    break;
                case EggInteractionZoneType.Lift:
                    Debug.LogWarning("Just lifted egg up.");
                    HandController.Instance.hasLiftedEgg = true;
                    break;
                case EggInteractionZoneType.Crack:
                    if (HandController.Instance.hasLiftedEgg)
                    {
                        other.GetComponent<Egg>().Crack();
                        Debug.LogWarning("Just cracked egg.");
                    }


                    break;
                case EggInteractionZoneType.Bowl:
                        Debug.LogWarning("Just poured egg.");
                    other.GetComponent<Egg>().Pour();
                    break;
                default:
                    break;
            }

        }


    }

}
