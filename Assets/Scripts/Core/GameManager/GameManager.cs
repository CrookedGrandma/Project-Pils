using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public static MessageQueue messageQueue = new MessageQueue();

    public bool IsPaused = false;
    public Canvas canvas;

    void Start()
    {
        canvas.enabled = false;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
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
    }

    void InitGame()
    {

    }

}
