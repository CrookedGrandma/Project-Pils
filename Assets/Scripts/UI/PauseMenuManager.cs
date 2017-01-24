using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {
    public GameObject ConfirmationPanel;
    public GameObject PauseMenu;
    public GameObject Components;
    public GameManager gameManager;
    public CanvasGroup canvas;

    /// <summary>
    /// Makes sure The confirmation panel isn't visible when launching the game.
    void Awake()
    {
        TogglePauseMenuOff();
        //ConfirmationPanel.SetActive(false);
        //canvas.alpha = 0;
    }

    void Update ()
    {
        //Debug.Log("Cycle");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameManager.IsPaused)
            {
                TogglePauseMenuOff();                
            }
            if (!gameManager.IsPaused)
            {
                TogglePauseMenuOn();                
            }
        }
    }

    /// <summary>
    /// Close Pause menu when continue is pressed
    public void TogglePauseMenuOn()
    {
        print("Pause Menu Visible");
        PauseMenu.SetActive(true);
        //Components.SetActive(true);
        SetConfInvisible();
        //canvas.alpha = 1;
    }
    public void TogglePauseMenuOff()
    {
        print("Pause Menu Invisible");
        PauseMenu.SetActive(false);
        //Components.SetActive(false);
        SetConfInvisible();
        //canvas.alpha = 0;
    }

    public void QuitButton()
    {
        SetConfVisible();
    }

    public void ConfirmButton()
    {
        print("Quit to main menu");
        SetConfInvisible();
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelButton()
    {
        SetConfInvisible();
    }

    public void ContinueButton()
    {
        //PauseMenu.SetActive(false);
        //canvas.alpha = 0;
        TogglePauseMenuOff();
        SetConfInvisible();
        gameManager.IsPaused = false;
    }

    public void SetConfVisible()
    {
        print("Conf Panel toggled to Visible");
        ConfirmationPanel.SetActive(true);
    }

    public void SetConfInvisible()
    {
        print("Conf Panel toggled to Invisible");
        ConfirmationPanel.SetActive(false);
    }

    /// <summary>
    /// Will load Inventory when button is pressed
    public void LoadInventory()
    {
        gameManager.IsPaused = false;
        print("Pause is loading Inventory");
        //TogglePauseMenuOff();
        PersistentInventoryScript.instance.InShop = false;
        GameObject player = GameObject.Find("Player");
        PlayerPrefsManager.SetCurrentScene(SceneManager.GetActiveScene().name);
        PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);
        SceneManager.LoadScene("Inventory");
        player.transform.position = PlayerPrefsManager.GetPositionInLevel("Inventory", player);
    }

}
