using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : Entity {

    public static GameManager instance = null;
    public MessageQueue messageQueue = new MessageQueue(); //Holds an instance of the MessageQueue
    public DialogueManager dialogueManager; //Reference to the DIalogueManager
    public QuestManager questManager; //Reference to the QuestManager
    public Cutscene cutscene;

    public bool IsPaused = false;
    public Canvas canvas;

    public Entity player;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(GameObject.Find("UI"));

        //Give the player a welcome message when they start the game
        Message m = new Message(this, player, MsgType.Dialogue, "Welcome to the game!");
        messageQueue.Add(m);
    }

    void Start()
    {
        canvas.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                Time.timeScale = 1;
                IsPaused = false;
                canvas.enabled = false;
            }
            else
            {
                Time.timeScale = 0;
                IsPaused = true;
                canvas.enabled = true;
            }
        }

        //Process the MessageQueue
        messageQueue.Dispatch();

    }

    void InitGame()
    {

    }

}
