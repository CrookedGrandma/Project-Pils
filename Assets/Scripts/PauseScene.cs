using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScene : MonoBehaviour
{
    public bool inPauseMenu = false;
    public GameObject confirmationPanel;

    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void EnterPauseMenu()
    {
        PlayerPrefsManager.SetCurrentScene(SceneManager.GetActiveScene().name);
        PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);
        SceneManager.LoadScene("Pause");
        SetConfirmationScreenInvisible();
    }

    public void ExitPauseMenu()
    {
        SceneManager.LoadScene(PlayerPrefsManager.GetCurrentScene());
        player.transform.position = PlayerPrefsManager.GetPositionInLevel(PlayerPrefsManager.GetCurrentScene(), player);
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
    }

    private void SetConfirmationScreenVisible()
    {
        confirmationPanel.SetActive(true);
    }
}
