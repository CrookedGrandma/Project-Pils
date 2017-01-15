using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public Scrollbar soundFXScrollBar;
    public Scrollbar musicScrollBar;
    public LevelManager levelManager;

    private MusicManager musicManager;
    private SoundFXManager soundFXManager;

    private void Start()
    {
        musicManager = GameObject.FindObjectOfType<MusicManager>();
        soundFXManager = GameObject.FindObjectOfType<SoundFXManager>();
        musicScrollBar.value = PlayerPrefsManager.GetMusicVolume();
        soundFXScrollBar.value = PlayerPrefsManager.GetSoundFXVolume();
    }

    private void Update()
    {
        musicManager.ChangeVolume(musicScrollBar.value);
        soundFXManager.ChangeVolume(soundFXScrollBar.value);
    }

    public void SaveAndExit()
    {
        PlayerPrefsManager.SetMusicVolume(musicScrollBar.value);
        PlayerPrefsManager.SetSoundFXVolume(soundFXScrollBar.value);
        levelManager.Loadlevel("MainMenu");
    }
}
