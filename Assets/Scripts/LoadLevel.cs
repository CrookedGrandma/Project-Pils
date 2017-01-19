using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadLevel : MonoBehaviour
{
    public string levelName;
    public bool hasToBeAccepted;
    public GameObject player;

    [HideInInspector]
    public bool isActive;

    private TextBox textBox;

    private void Start()
    {
        // Find the player and the textbox in the level
        player = GameObject.Find("Player");
        textBox = GameObject.FindObjectOfType<TextBox>();

        // Set all the load level collider to inactive
        isActive = false;
    }

    // When entering the collider
    private void OnTriggerEnter(Collider trigger)
    {
        // If the scene change has to be accepted, put a message on the screen which informs the player where he will go
        if (hasToBeAccepted)
        {
            textBox.AddLine("Game", "Press \"E\" or \"Return\" to go to " + levelName, "White");
        }
    }

    // Checks if the player is in the Collider area (trigger).
    public void OnTriggerStay(Collider trigger)
    {
        // If the scene change has to be accepted
        if (hasToBeAccepted)
        {
            // And it is accepted
            if (Input.GetButtonDown("Accept"))
            {
                // Load the level
                LoadALevel(levelName);
            }
        }
        // If it does not have to be accepted, load the level automatically
        else if (isActive)
        {
            LoadALevel(levelName);
        }
    }

    private void LoadALevel(string levelName)
    {
        // Store the playerposition in the current level
        PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);

        // Load the next level
        SceneManager.LoadScene(levelName);

        // Create a new position from the saved position of the new level in Unity's PlayerPrefs and set the player's position to this value
        Vector3 newPosition = PlayerPrefsManager.GetPositionInLevel(levelName, player);
        player.transform.position = newPosition;

        // Put the collider on inactive
        isActive = false;
    }
}
