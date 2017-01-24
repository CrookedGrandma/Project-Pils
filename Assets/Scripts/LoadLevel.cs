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

    private void Awake()
    {
        // Find the player and the textbox in the level
        player = GameObject.Find("Player");
        textBox = GameObject.FindObjectOfType<TextBox>();
    }

    private void Start()
    {
        // Set all the load level collider to inactive
        isActive = false;
    }

    // When entering the collider
    private void OnTriggerEnter(Collider trigger)
    {
        // Only if the player is the one to collide with the collider
        if (trigger.name == "Player")
        {
            // If the scene change has to be accepted, put a message on the screen which informs the player where he will go
            if (isActive)
            {
                if (hasToBeAccepted)
                {
                    // Dialogue text changes depending on the level
                    string message = "go to " + levelName;
                    if (levelName == "Home")
                    {
                        message = "go to your home";
                    }
                    if (levelName == "HomeFriend")
                    {
                        message = "go to Ian's home";
                    }
                    if (levelName == "Dungeon_PiPi")
                    {
                        message = "enter PiPi";
                    }
                    if (levelName == "Dungeon_FaceBeer")
                    {
                        message = "go down the elevator";
                    }
                    if (levelName == "FaceBeerLobby")
                    {
                        message = "enter the building of FaceBeer";
                    }
                    if (levelName == "BossLevel")
                    {
                        message = "enter the serverroom";
                    }
                    if (levelName == "TheVergeInn")
                    {
                        message = "enter TheVergeInn";
                    }
                    if (levelName == "Wok2Stay")
                    {
                        message = "enter the Wok2Stay";
                    }

                    // Add the text to the dialoguebox
                    textBox.AddLine("Game", "Press \"E\" or \"Return\" to " + message + ".", "White");
                }
                // Load the level automatically
                else
                {
                    LoadALevel(levelName);
                }
            }
        }
    }

    // Checks if the player is in the Collider area (trigger).
    private void OnTriggerStay(Collider trigger)
    {
        // Only if the player is the one to collide with the collider
        if (trigger.name == "Player")
        {
            if (hasToBeAccepted)
            {
                if (Input.GetButtonDown("Accept"))
                {
                    LoadALevel(levelName);
                }
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
        if(SceneManager.GetActiveScene().name == "Home" && levelName == "Woonplaats")
        {
            if (GameManager.instance.questManager.questLog.ContainsKey("Quest002"))
            {
                GameManager.instance.questManager.CompleteObjective("Quest002LeaveHouse");
                GameManager.instance.questManager.AddQuestToLog("Quest003");

            } else
            {
                Message m = new Message(GameManager.instance.questManager, GameManager.instance.dialogueManager, MsgType.Dialogue, "You need to find your phone before you can leave your house!");
                GameManager.instance.messageQueue.Add(m);
                return;
            }
        }

       /* if (SceneManager.GetActiveScene().name == "Woonplaats" && levelName == "Market")
        {
            if (!GameManager.instance.questManager.questLog.ContainsKey("Quest004"))
            {
                Message m = new Message(GameManager.instance.questManager, GameManager.instance.dialogueManager, MsgType.Dialogue, "Meet up with Ian before going here!");
                GameManager.instance.messageQueue.Add(m);
                return;
            }
        }*/

        GameManager.instance.cutscene.SetText("");
        GameManager.instance.cutscene.FadeInPanel(true);

        // Store the active scene
        PlayerPrefsManager.SetCurrentScene(SceneManager.GetActiveScene().name);

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

        GameManager.instance.cutscene.FadeOutPanel();
    }

    public void LoadStartLevel()
    {
        // Store the playerposition in the current level
        PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);

        // Load Woonplaats
        SceneManager.LoadScene("Home");

        // Create a new position from the saved position of the new level in Unity's PlayerPrefs and set the player's position to this value
        Vector3 newPosition = PlayerPrefsManager.GetPositionInLevel("Home", player);
        player.transform.position = newPosition;
    }

    //Special method for the pausescreen to load the main menu

}

