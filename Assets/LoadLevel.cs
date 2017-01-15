using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour
{
    public string levelName;
    public GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    /// <summary>
    /// Checks's if the player is in the Collider area (trigger).
    /// When triggered, if the "Accept"-button is pressed, load the Home scene.
    public void OnTriggerStay(Collider trigger)
    {
        print(levelName + " Scene trigger active");
        if (Input.GetButtonDown("Accept"))
        {
            PlayerPrefsManager.SetPositionInLevel(Application.loadedLevelName, player);
            Application.LoadLevel(levelName);
            Vector3 newPosition = PlayerPrefsManager.GetPositionInLevel(levelName, player);
            player.transform.position = newPosition;
            print("Loading " + levelName + " Scene");
        }
    }
}