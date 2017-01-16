using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour
{
    public string levelName;
    public GameObject player;

    private void Awake()
    {
        // Find the player in the level
        player = GameObject.Find("Player");
    }

    // Checks if the player is in the Collider area (trigger).
    public void OnTriggerStay(Collider trigger)
    {
        Debug.Log(levelName + " Scene trigger active");

        if (Input.GetButtonDown("Accept"))
        {
            // Store the playerposition in the current level
            PlayerPrefsManager.SetPositionInLevel(Application.loadedLevelName, player);

            // Freeze the player so he doesn't fall through the map while loading a new level.
            if (levelName != "Dungeon_FaceBeer" && levelName != "Dungeon_PiPi")
            {
                // Not in the dungeons because the player will not unfreeze here
                player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }

            // Load the next level
            Application.LoadLevel(levelName);

            // Create a new position from the saved position of the new level in Unity's PlayerPrefs and set the player's position to this value
            Vector3 newPosition = PlayerPrefsManager.GetPositionInLevel(levelName, player);
            player.transform.position = newPosition;

            Debug.Log("Loading " + levelName + " Scene");

            if (levelName == "Dungeon_FaceBeer" || levelName == "Dungeon_PiPi")
            {
                // Scale the player so he fits in the dungeons
                player.transform.localScale = new Vector3(0.5f, 0.5f * player.transform.localScale.y, 0.5f);
            }
            else
            {
                // Scale the player to his normal scale
                player.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        // As soon as the level is loaded, unfreeze the player and freeze his rotation again (normal situation)
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }
}
