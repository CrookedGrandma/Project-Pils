using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CamMovement : MonoBehaviour
{
    public GameObject Parent;
    public bool ForcedZoomOut = false;
    public bool IsMoving = false;
    private Vector3 CurPos;
    private Vector3 LastPos;
    public Collider CurrCollider;

    //Checks if the camera collides with something
    void OnTriggerStay(Collider other)
    {
        if (IsMoving)
            return;

        ForcedZoomOut = true;
        //When colliding, the camera moves up and back from the player object          
        transform.position += new Vector3(0, 0.2f, -0.2f);
        print("Zooming Out");
        CurrCollider = other;

    }

  
    void Update()
    {
        LastPos = CurPos;
        CurPos = Parent.transform.position;

        if (CurPos != LastPos)
        {
            IsMoving = true;
        }
        else if (CurPos == LastPos)
        {
            IsMoving = false;
        }

        if (IsMoving)
        {
            ForcedZoomOut = false;
        }


        //makes sure the camera always looks at the player object   
        transform.LookAt(Parent.transform);

        //Moves the camera back to the normal (local) position
        if (transform.localPosition.y > 11f && !ForcedZoomOut && !IsMoving)
        {
            transform.position += new Vector3(0, Time.deltaTime * -4f, Time.deltaTime * 4f);
            print("Zooming back in");
        }
    }
}