using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadLevel : MonoBehaviour
{
    public string levelName;
    public GameObject player;

    private string levelToBeLoaded;
    private TextBox textBox;

    private void Awake()
    {
        // Find the player, the textbox and the keycode in the level
        player = GameObject.Find("Player");
        textBox = GameObject.FindObjectOfType<TextBox>();
    }

    // When entering the collider, put a message on the screen which informs the player where he will go
    private void OnTriggerEnter(Collider other)
    {
        textBox.AddLine("Game", "Press \"E\" or \"Return\" to go to " + levelName, "White");
    }

    private void Update()
    {
        SceneManager.GetActiveScene();
    }

    // Checks if the player is in the Collider area (trigger).
    public void OnTriggerStay(Collider trigger)
    {
        Debug.Log(levelName + " Scene trigger active");

        if (Input.GetButtonDown("Accept"))
        {
            // Used to remove the constraints on the player
            levelToBeLoaded = levelName;

            // Store the playerposition in the current level
            PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);

            // Freeze the player so he doesn't fall through the map while loading a new level
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            // Load the next level
            SceneManager.LoadScene(levelName);

            // Create a new position from the saved position of the new level in Unity's PlayerPrefs and set the player's position to this value
            Vector3 newPosition = PlayerPrefsManager.GetPositionInLevel(levelName, player);
            player.transform.position = newPosition;

            Debug.Log("Loading " + levelName + " Scene");

            Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
            Debug.Log("levelName: " + levelName);
            Debug.Log("levelToBeLoaded: " + levelToBeLoaded);
            if (SceneManager.GetActiveScene().name == levelToBeLoaded)
            {
                Debug.Log("Level geladen");
            }
        }
    }

    /*private void OnLevelWasLoaded(string level)
    {
        // As soon as the level is loaded, unfreeze the player and freeze his rotation again (normal situation)
        Debug.Log(levelName + " loaded");
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }*/

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("level loaded");
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }
}
