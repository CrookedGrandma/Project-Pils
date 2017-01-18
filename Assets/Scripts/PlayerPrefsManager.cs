using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    // The strings used to store the values in Unity's PlayerPrefs
    const string MUSIC_VOLUME_KEY = "music_volume";
    const string SOUND_FX_VOLUME_KEY = "sound_fx_volume";
    const string LEVEL_POSITION_X_KEY = "_position_X_";
    const string LEVEL_POSITION_Y_KEY = "_position_Y_";
    const string LEVEL_POSITION_Z_KEY = "_position_Z_";
    const string PLAYER_HEALTH_KEY = "player_health";
    const string PLAYER_XP_KEY = "player_xp";
    const string PLAYER_LEVEL_KEY = "player_level";
    const string CURRENT_SCENE_KEY = "current_scene";

    // Sets the music volume to the float given
    public static void SetMusicVolume(float volume)
    {
        if (volume >= 0f && volume <= 1f)
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("Master volume must be between 0 and 1");
        }
    }

    // Gets the music volume
    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
    }

    // Sets the volume of the sound effects
    public static void SetSoundFXVolume(float volume)
    {
        if (volume >= 0f && volume <= 1f)
        {
            PlayerPrefs.SetFloat(SOUND_FX_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("Soundeffects volume must be between 0 and 1");
        }
    }

    // Gets the volume of the soundeffects
    public static float GetSoundFXVolume()
    {
        return PlayerPrefs.GetFloat(SOUND_FX_VOLUME_KEY);
    }

    // Stores the X, Y and Z positions of the given gameobject in a given level to floats in Unity's PlayerPrefs
    public static void SetPositionInLevel(string level, GameObject gameObject)
    {
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_X_KEY + gameObject.ToString(), gameObject.transform.position.x);
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_Y_KEY + gameObject.ToString(), gameObject.transform.position.y);
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_Z_KEY + gameObject.ToString(), gameObject.transform.position.z);
    }

    // Gets the X, Y and Z positions of the given gameobject in a given level and puts them together in a Vector3
    public static Vector3 GetPositionInLevel(string level, GameObject gameObject)
    {
        float x = PlayerPrefs.GetFloat(level + LEVEL_POSITION_X_KEY + gameObject.ToString());
        float y = PlayerPrefs.GetFloat(level + LEVEL_POSITION_Y_KEY + gameObject.ToString());
        float z = PlayerPrefs.GetFloat(level + LEVEL_POSITION_Z_KEY + gameObject.ToString());
        return new Vector3(x, y, z);
    }

    // Sets the health of the player to an int in Unity's PlayerPrefs
    public static void SetPlayerHealth(int health)
    {
        PlayerPrefs.SetInt(PLAYER_HEALTH_KEY, health);
    }

    // Gets the health from Unity's PlayerPrefs
    public static float GetPlayerHealth()
    {
        return PlayerPrefs.GetInt(PLAYER_HEALTH_KEY);
    }

    // Sets the XP of the player to an int in Unity's PlayerPrefs
    public static void SetPlayerXP(int XP)
    {
        PlayerPrefs.SetInt(PLAYER_XP_KEY, XP);
    }

    // Gets the XP from Unity's PlayerPrefs
    public static int GetPlayerXP()
    {
        return PlayerPrefs.GetInt(PLAYER_XP_KEY);
    }

    // Sets the level of the player to an int in Unity's PlayerPrefs
    public static void SetPlayerLevel(int player_level)
    {
        PlayerPrefs.SetInt(PLAYER_LEVEL_KEY, player_level);
    }

    // Gets the level from Unity's PlayerPrefs
    public static int GetPlayerLevel()
    {
        return PlayerPrefs.GetInt(PLAYER_LEVEL_KEY);
    }

    // Used for setting the startpositions in the levels
    public static void SetStartPositions(string level, GameObject gameObject, float xPos, float yPos, float zPos)
    {
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_X_KEY + gameObject.ToString(), xPos);
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_Y_KEY + gameObject.ToString(), yPos);
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_Z_KEY + gameObject.ToString(), zPos);
    }

    // Sets the current scene the player is in, used when going to inventory, shop and combat state
    public static void SetCurrentScene(string scene)
    {
        PlayerPrefs.SetString(CURRENT_SCENE_KEY, scene);
    }

    // Gets the current scene, used when coming back from the inventory, shop and combat state
    public static string GetCurrentScene()
    {
        return PlayerPrefs.GetString(CURRENT_SCENE_KEY);
    }
}