using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadLevel : MonoBehaviour
{
    public string levelName;
    public bool hasToBeAccepted;
    public GameObject player;

    private bool isActive = false;
    private TextBox textBox;

    private void Awake()
    {
        // Find the player, the textbox and the keycode in the level
        player = GameObject.Find("Player");
        textBox = GameObject.FindObjectOfType<TextBox>();
    }

    private void Update()
    {
        Debug.Log(isActive);
    }
    
    // When entering the collider
    private void OnTriggerEnter(Collider trigger)
    {
        // If the scene change has to be accepted, put a message on the screen which informs the player where he will go
        if (hasToBeAccepted)
        {
            textBox.AddLine("Game", "Press \"E\" or \"Return\" to go to " + levelName, "White");
        }
        // Load the level automatically
        else if (isActive)
        {
            LoadALevel(levelName);
        }
    }

    // Checks if the player is in the Collider area (trigger).
    public void OnTriggerStay(Collider trigger)
    {
        if (hasToBeAccepted)
        {
            if (Input.GetButtonDown("Accept"))
            {
                LoadALevel(levelName);
            }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        // As soon as the level is loaded, unfreeze the player and freeze his rotation again (normal situation)
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void LoadALevel(string levelName)
    {
        // Store the playerposition in the current level
        PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);

        // Freeze the player so he doesn't fall through the map while loading a new level
        if (levelName != "Dungeon_FaceBeer" && levelName != "Dungeon_PiPi" && levelName != "BossLevel")
        {
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        // Load the next level
        SceneManager.LoadScene(levelName);

        // Create a new position from the saved position of the new level in Unity's PlayerPrefs and set the player's position to this value
        Vector3 newPosition = PlayerPrefsManager.GetPositionInLevel(levelName, player);
        player.transform.position = newPosition;

        // Put the collider on inactive
        isActive = false;
    }
}
