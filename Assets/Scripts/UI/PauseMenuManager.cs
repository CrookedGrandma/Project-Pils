using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {
    public GameObject ConfirmationPanel;
    public GameObject PauseMenu;
    public GameObject Components;
    private GameManager gameManager;


    /// <summary>
    /// Makes sure The confirmation panel isn't visible when launching the game.
    void Awake()
    {
        ConfirmationPanel.SetActive(false);
    }

    void Update ()
    {
        print("NewFrame");

        Components.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
            //Components.SetActiveRecursively(true);
            ConfirmationPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Close Pause menu when continue is pressed
    public void TogglePauseMenu()
    {
        print("Pause Menu Toggled");
        Components.SetActive(true);
        ConfirmationPanel.SetActive(false);
    }


    //***METHODS FOR BUTTONS***//

    ///<summary>
    ///Will toggle the Confirmation Panel to visible.
    public void SetVisible()
    {
        print("Conf Panel toggled to Visible");
        ConfirmationPanel.SetActive(true);
    }

    ///<summary>
    ///Will toggle the Confirmation Panel to invisible.
    public void SetInvisible()
    {
        print("Conf Panel toggled to Invisible");
        ConfirmationPanel.SetActive(false);
    }

    /// <summary>
    /// Will load Inventory when button is pressed
    public void LoadInventory()
    {
        print("Pause is loading Inventory");
        PauseMenu.gameObject.SetActive(false);
        PersistentInventoryScript.instance.InShop = false;
        GameObject player = GameObject.Find("Player");
        PlayerPrefsManager.SetCurrentScene(SceneManager.GetActiveScene().name);
        PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);
        SceneManager.LoadScene("Inventory");
        player.transform.position = PlayerPrefsManager.GetPositionInLevel("Inventory", player);
    }

    public void ReturnMainMenu()
    {
        print("return to main menu");
        PauseMenu.gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }


    ///<summary>
    ///Will toggle the Confirmation Panel to invisible.
    public void SetAllInvisible()
    {
        Components.SetActive(false);
        print("Pause Menu toggled to Invisible");
        ConfirmationPanel.SetActive(false);

    }

    public void ContinueButton()
    {
        
    }


}
