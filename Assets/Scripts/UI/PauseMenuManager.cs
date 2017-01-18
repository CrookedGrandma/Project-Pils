using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {
    public GameObject ConfirmationPanel;
    public GameObject PauseMenu;
    public bool IsVisible = false;

    /// <summary>
    /// Makes sure The confirmation panel isn't visible when launching the game.
    void Awake()
    {
        ConfirmationPanel.SetActive(false);
    }

    ///<summary>
    ///Will toggle the pause menu to invisible when 
    ///the escape-button is pressed while already in pause menu.
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsVisible)
        {
            PauseMenu.gameObject.SetActive(false);
            IsVisible = false;
            print("Pause menu Already Visible, Toggling off");
        }
    }
   
    ///<summary>
    ///Will toggle the Confirmation Panel to visible.
    public void SetVisible()
    {
        print("Conf Panel toggled to Visible");
        IsVisible = true;
        ConfirmationPanel.SetActive(true);
    }

    ///<summary>
    ///Will toggle the Confirmation Panel to invisible.
    public void SetInvisible()
    {
        print("Conf Panel toggled to Invisible");
        IsVisible = false;
        ConfirmationPanel.SetActive(false);
    }

    /// <summary>
    /// Will load Inventory when button is pressed
    public void LoadInventory()
    {
        print("Pause is loading Inventory");
        PauseMenu.gameObject.SetActive(false);
        IsVisible = false;
        PlayerPrefsManager.SetCurrentScene(SceneManager.GetActiveScene().name);
        Application.LoadLevel("Inventory");
    }


    ///<summary>
    ///Will toggle the Confirmation Panel to invisible.
    public void SetAllInvisible()
    {
        print("Pause Menu toggled to Invisible");
        IsVisible = false;
        ConfirmationPanel.SetActive(false);
        PauseMenu.SetActive(false);
    }
}
