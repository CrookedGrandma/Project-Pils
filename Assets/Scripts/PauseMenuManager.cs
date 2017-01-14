using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour {

    public Image panel;
    public bool IsVisible = false;

    ///<summary>
    ///Will toggle the pause menu to invisible.
    void Start () {
        panel.gameObject.SetActive(false);
    }

    ///<summary>
    ///Will toggle the pause menu to invisible when 
    ///the escape-button is pressed while already in pause menu.
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)&& IsVisible)
        {
            panel.gameObject.SetActive(false);
            IsVisible = false;
        }
    }
   
    ///<summary>
    ///Will toggle the pause menu to visible.
    public void SetVisible()
    {
        print("Pause Menu toggled to Visible");
        panel.gameObject.SetActive(true);
        IsVisible = true;
    }
    
    ///<summary>
    ///Will toggle the pause menu to invisible.
    public void SetInvisible()
    {
        print("Pause Menu toggled to Invisible");
        panel.gameObject.SetActive(false);
        IsVisible = false;
    }

    /// <summary>
    /// Will load Inventory when button is pressed
    /// </summary>
    public void LoadInventory()
    {
        print("Pause Menu is loading Inventory");
        panel.gameObject.SetActive(false);
        IsVisible = false;
        Application.LoadLevel("Inventory");
    }
}
