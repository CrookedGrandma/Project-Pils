﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            if (thisLevelMusic)
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
