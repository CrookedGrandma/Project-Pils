using UnityEngine;
using System.Collections;


public class DoorScript : MonoBehaviour
{
    public GameObject Object;

    ///<summary>
    ///Method for checking if the player enters the door's collider box.
    ///Will play DoorOpen animation when triggered.
    private void OnTriggerEnter(Collider trigger)
    {
        Object.GetComponent<Animation>().Play("DoorOpen");
    }

    ///<summary>
    ///Method for checking if the player leaves the door's collider box.
    ///Will play DoorClose animation when triggered.
    private void OnTriggerExit(Collider trigger)
    {
        Object.GetComponent<Animation>().Play("DoorClose");
    }
}
