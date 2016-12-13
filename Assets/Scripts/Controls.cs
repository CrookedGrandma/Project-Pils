using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour
{
    public Canvas canvas;

    // Use this for initialization
    void Start()
    {
        canvas.enabled = false;
    }


    // Update is called once per frame
    public void Update()
    {
        // Pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.IsPaused)
            {
                Time.timeScale = 1;
                GameManager.instance.IsPaused = false;
                canvas.enabled = false;
            }
            else
            {
                Time.timeScale = 0;
                GameManager.instance.IsPaused = true;
                canvas.enabled = true;
            }
        }
    }
}
