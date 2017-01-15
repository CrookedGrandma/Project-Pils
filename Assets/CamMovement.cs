using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public GameObject Parent;

    //Checks if the camera collides with something
    void OnTriggerStay(Collider other)
    {
        //If true, the camera moves up and back from the player object          
        transform.position += new Vector3(0, 0.2f, -0.2f);
    }


    void Update()
    {
        //makes sure the camera always looks at the player object   
        transform.LookAt(Parent.transform);
        /* if (transform.localPosition.y > 5.04f)
         {
             transform.localPosition = new Vector3(0, -0.05f, +0.05f);
         } */
    }
    /*
    void OnTriggerExit(Collider other)
    {
        transform.localPosition = new Vector3(0f, 5.04f, -11.3f);
    }*/
}
