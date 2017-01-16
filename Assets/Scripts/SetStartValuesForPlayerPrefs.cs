using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStartValuesForPlayerPrefs : MonoBehaviour
{
    // This script sets the first positions in the levels for the player to spawn in. Every first time a level is loaded, it has to have a position for the player, otherwise
    // the player won't spawn at the right place. These positions will be overriden the next time a level is exited. The first positions will just be hardcoded.

    // The levels at this moment in the script are:
    // BossLevel, Dungeon_FaceBeer, Dungeon_PiPi, FaceBeerLobby, FaceBeerOutside, Home, HomeFriend, Market, PiPiOutside, TheVergeInn, Wok2Stay, Woonplaats

    public int playerHealth = 100;

    private GameObject player;

    private void Start()
    {
        // Find the player
        player = GameObject.Find("Player");

        // Go through all the levels and set startpositions
        PlayerPrefsManager.SetStartPositions("BossLevel", player, -0.4f, 1.5f, -18.5f);
        PlayerPrefsManager.SetStartPositions("Dungeon_FaceBeer", player, 0.5f, 1.5f, 0.5f);
        PlayerPrefsManager.SetStartPositions("Dungeon_PiPi", player, 0.5f, 1.5f, 0.5f);
        PlayerPrefsManager.SetStartPositions("FaceBeerLobby", player, 0f, 1.5f, -6.8f);
        PlayerPrefsManager.SetStartPositions("FaceBeerOutside", player, 93.5f, 1.5f, -63.5f);
        PlayerPrefsManager.SetStartPositions("Home", player, -10.5f, 1.5f, -13.5f);
        PlayerPrefsManager.SetStartPositions("HomeFriend", player, 10.5f, 1.5f, -13.5f);
        PlayerPrefsManager.SetStartPositions("Market", player, 1.8f, 1.5f, -74.0f);
        PlayerPrefsManager.SetStartPositions("PiPiOutside", player, -37.5f, 1.5f, -13.5f);
        PlayerPrefsManager.SetStartPositions("TheVergeInn", player, -3.0f, 1.5f, -11.2f);
        PlayerPrefsManager.SetStartPositions("Wok2Stay", player, -3.0f, 1.5f, -11.2f);
        PlayerPrefsManager.SetStartPositions("Woonplaats", player, 48.3f, 1.5f, 25.4f);

        // Set the health, XP and level for the player
        PlayerPrefsManager.SetPlayerHealth(playerHealth);
        PlayerPrefsManager.SetPlayerXP(0);
        PlayerPrefsManager.SetPlayerLevel(1);

        // Set start values for the music and soundeffects volume
        PlayerPrefsManager.SetMusicVolume(1f);
        PlayerPrefsManager.SetSoundFXVolume(1f);
    }
}
