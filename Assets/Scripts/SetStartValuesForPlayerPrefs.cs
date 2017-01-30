using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStartValuesForPlayerPrefs : MonoBehaviour
{
    // This script sets the first positions in the levels for the player to spawn in (and some other things). Every first time a level is loaded, it has to have a position 
    // for the player, otherwise the player won't spawn at the right place. These positions will be overriden the next time a level is exited. The first positions will just be hardcoded.

    // The levels at this moment in the script are:
    // BossLevel, Dungeon_FaceBeer, Dungeon_PiPi, FaceBeerLobby, FaceBeerOutside, Home, HomeFriend, Market, PiPi, PiPiOutside, TheVergeInn, Wok2Stay, Woonplaats

    public int playerHealth;

    public void StartPositions()
    {
        // Set the current level to Home
        PlayerPrefsManager.SetCurrentScene("Home");

        // Go through all the levels and set startpositions
        PlayerPrefsManager.SetStartPositions("BossLevel", -0.4f, 1.5f, -18.5f);
        PlayerPrefsManager.SetStartPositions("Dungeon_FaceBeer", 1.5f, 1.5f, 1.5f);
        PlayerPrefsManager.SetStartPositions("Dungeon_PiPi", 1.5f, 1.5f, 1.5f);
        PlayerPrefsManager.SetStartPositions("FaceBeerLobby", 0.0f, 1.5f, -6.0f);
        PlayerPrefsManager.SetStartPositions("FaceBeerOutside", 93.5f, 1.5f, -63.5f);
        PlayerPrefsManager.SetStartPositions("Home", 13.5f, 3.5f, 12.3f);
        PlayerPrefsManager.SetStartPositions("HomeFriend", 10.5f, 1.5f, -13.0f);
        PlayerPrefsManager.SetStartPositions("Market", 1.8f, 1.5f, -59.0f);
        PlayerPrefsManager.SetStartPositions("PiPi", -28.5f, 1.5f, 0.0f);
        PlayerPrefsManager.SetStartPositions("PiPiOutside", -25.5f, 1.5f, -13.5f);
        PlayerPrefsManager.SetStartPositions("TheVergeInn", -3.0f, 1.5f, -11.2f);
        PlayerPrefsManager.SetStartPositions("Wok2Stay", -3.0f, 1.5f, -11.2f);
        PlayerPrefsManager.SetStartPositions("Woonplaats", 49.0f, 1.5f, 25.5f);

        PlayerPrefsManager.SetStartPositions("Shop", -10.0f, -10.0f, -10.0f);
        PlayerPrefsManager.SetStartPositions("Inventory", -10.0f, -10.0f, -10.0f);
        PlayerPrefsManager.SetStartPositions("Combat", -10.0f, -10.0f, -10.0f);

        // Set the health, XP, level and currency for the player
        PlayerPrefsManager.SetPlayerHealth(playerHealth);
        PlayerPrefsManager.SetPlayerXP(0);
        PlayerPrefsManager.SetPlayerLevel(1);
        PersistentInventoryScript.instance.Currency = 100;

        // Set the starting values for the dungeons
        PlayerPrefsManager.SetFirstTimeFaceBeerDungeon(1);
        PlayerPrefsManager.SetFirstTimePiPiDungeon(1);
        PlayerPrefsManager.SetAmountOfEnemiesInPiPiDungeon(0);
        PlayerPrefsManager.SetAmountOfEnemiesInFaceBeerDungeon(0);
        PlayerPrefsManager.SetAmountOfWallsInPiPiDungeon(0);
        PlayerPrefsManager.SetAmountOfWallsInFaceBeerDungeon(0);
    }
}