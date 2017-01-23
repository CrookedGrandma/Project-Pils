using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] levelMusicChangeArray;

    private AudioSource audioSource;
    private static MusicManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = PlayerPrefsManager.GetMusicVolume();
        audioSource.clip = levelMusicChangeArray[0];
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (levelMusicChangeArray.Length != 0)
        {
            AudioClip thisLevelMusic = levelMusicChangeArray[level];

            if (level < 2)
            {
                // If the current playing clip is the clip that has to be played at the first real scene in the build settings (Home)
                if (audioSource.clip != levelMusicChangeArray[0])
                {
                    audioSource.volume = PlayerPrefsManager.GetMusicVolume();
                    audioSource.clip = levelMusicChangeArray[0];
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
            else if (level > 2 && level < 18)
            {
                // Normal ingame music has to be played here

                // If the current playing clip is the clip that has to be played at the first real scene in the build settings (Home)
                if (audioSource.clip != levelMusicChangeArray[3])
                {
                    audioSource.volume = PlayerPrefsManager.GetMusicVolume();
                    audioSource.clip = levelMusicChangeArray[3];
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
            else if (thisLevelMusic)
            {
                audioSource.volume = PlayerPrefsManager.GetMusicVolume();
                audioSource.clip = thisLevelMusic;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            // Array is empty, so no music has to be played here
        }
    }

    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
