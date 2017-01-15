using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour
{
    public string levelName;

    /// <summary>
    /// Checks's if the player is in the Collider area (trigger).
    /// When triggered, if the "Accept"-button is pressed, load the Home scene.
    public void OnTriggerStay(Collider trigger)
    {
        print(levelName + " Scene trigger active");
        if (Input.GetButtonDown("Accept"))
        {
            Application.LoadLevel(levelName);
            print("Loading " + levelName + " Scene");
        }
    }
}