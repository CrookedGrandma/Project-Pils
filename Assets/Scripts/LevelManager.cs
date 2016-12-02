using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public void Loadlevel(string name)
    {
        Application.LoadLevel(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }
}
