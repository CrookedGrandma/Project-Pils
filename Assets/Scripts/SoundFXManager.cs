﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public AudioClip[] soundEffectsArray;

    private AudioSource audioSource;
    private static SoundFXManager instance;

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
    }

    public void ChangeVolume(float volume)
    {
        foreach (AudioClip sfx in soundEffectsArray)
        {
            audioSource.volume = PlayerPrefsManager.GetSoundFXVolume();
        }
    }
}
