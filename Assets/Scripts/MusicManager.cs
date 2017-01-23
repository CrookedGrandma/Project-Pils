using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] levelMusicChangeArray;

    private AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (levelMusicChangeArray.Length != 0)
        {
            AudioClip thisLevelMusic = levelMusicChangeArray[level];

            if (level > 2 && level < 18)
            {
                // Normal ingame music has to be played here

                // If the current playing clip is the clip that has to be played at the first real scene in the build settings (Home)
                if (audioSource.clip == levelMusicChangeArray[3])
                {
                    // Do nothing
                }
                else
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
