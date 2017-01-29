using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    ///<summary>
    /// Method that loads a certain level.
    public void Loadlevel(string name)
    {
        SceneManager.LoadScene(name);
        GameObject player = GameObject.Find("Player");
        player.transform.position = PlayerPrefsManager.GetPositionInLevel(name, player);
    }

    ///<summary>
    /// Method that quits the game.
    public void QuitGame()
    {
        Application.Quit();
    }

    ///<summary>
    /// Method that unpauses the game when it is paused.
    public void UnPause()
    {
        Time.timeScale = 1;
    }

    ///<summary>
    /// Method that loads the saved game
    public void LoadSavedGame()
    {
        SceneManager.LoadScene(PlayerPrefsManager.GetSavedScene());
        GameObject player = GameObject.Find("Player");
        player.transform.position = PlayerPrefsManager.GetSavedPosition();
    }
}
