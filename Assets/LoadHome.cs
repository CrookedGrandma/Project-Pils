using UnityEngine;
using System.Collections;

public class LoadHome : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Checks's if the player is in the Collider area (trigger).
    /// When triggered, if the "E"-button is pressed, load the Home scene.
    public void OnTriggerStay(Collider trigger)
    {
        print("Home Scene trigger active");
        if (Input.GetKeyDown(KeyCode.E))
        {
            Application.LoadLevel("Home");
            print("Loading Home Scene");
        }

    }

}