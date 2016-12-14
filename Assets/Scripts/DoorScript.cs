using UnityEngine;
using System.Collections;


public class DoorScript : MonoBehaviour
{
    public GameObject Object;
    private void OnTriggerEnter(Collider trigger)
    {
        
        Object.GetComponent<Animation>().Play("DoorOpen");
    }
    private void OnTriggerExit(Collider trigger)
    {
        Object.GetComponent<Animation>().Play("DoorClose");
    }
}
