using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    ///<summary>
    ///Method that loads a certain level.
    public void Loadlevel(string name)
    {
        Application.LoadLevel(name);
    }

    ///<summary>
    ///Method that quits the game.
    public void QuitGame()
    {
        Application.Quit();
    }

    ///<summary>
    ///Method that unpauses the game when it is paused.
    public void UnPause()
    {
        Time.timeScale = 1;
    }
}
