using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScene : MonoBehaviour
{
    public GameObject confirmationPanel;
    public bool inPauseMenu;

    private bool confirmationpanelVisible;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        SetConfirmationScreenInvisible();
        inPauseMenu = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !confirmationpanelVisible && inPauseMenu)
        {
            ExitPauseMenu();
        }
    }

    public void ExitPauseMenu()
    {
        SceneManager.LoadScene(PlayerPrefsManager.GetCurrentScene());
        player.transform.position = PlayerPrefsManager.GetPositionInLevel(PlayerPrefsManager.GetCurrentScene(), player);
        inPauseMenu = false;
    }

    public void Continue()
    {
        ExitPauseMenu();
    }

    public void LoadInventory()
    {
        PersistentInventoryScript.instance.InShop = false;
        PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);
        SceneManager.LoadScene("Inventory");
        player.transform.position = PlayerPrefsManager.GetPositionInLevel("Inventory", player);
    }

    public void SaveGame()
    {
        // SAVES THE GAME, DOES NOTHING AT THE MOMENT
    }

    public void LoadGame()
    {
        // LOADS THE GAME, DOES NOTHING AT THE MOMENT
    }

    public void ExitToMainMenu()
    {
        SetConfirmationScreenVisible();
    }

    public void Confirm()
    {
        SetConfirmationScreenInvisible();
        SceneManager.LoadScene("MainMenu");
    } 

    public void Cancel()
    {
        SetConfirmationScreenInvisible();
    }

    private void SetConfirmationScreenInvisible()
    {
        confirmationPanel.SetActive(false);
        confirmationpanelVisible = false;
    }

    private void SetConfirmationScreenVisible()
    {
        confirmationPanel.SetActive(true);
        confirmationpanelVisible = true;
    }
}
