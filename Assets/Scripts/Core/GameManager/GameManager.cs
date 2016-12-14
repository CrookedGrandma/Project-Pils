using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : Entity {

    public static GameManager instance = null;
    public MessageQueue messageQueue = new MessageQueue();
    public DialogueManager dialogueManager;

    public bool IsPaused = false;
    public Canvas canvas;

    public Entity player;

    void Start()
    {
        canvas.enabled = false;
        dialogueManager = GetComponent<DialogueManager>();
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Message m = new Message(this, player, MsgType.Dialogue, "Welcome to the game!");
        messageQueue.Add(m);
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

        messageQueue.Dispatch();

    }

    void InitGame()
    {

    }

}
