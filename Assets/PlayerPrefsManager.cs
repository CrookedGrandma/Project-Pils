using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    // The strings used to store the values in Unity's PlayerPrefs
    const string MASTER_VOLUME_KEY = "master_volume";
    const string LEVEL_POSITION_X = "_position_X_";
    const string LEVEL_POSITION_Y = "_position_Y_";
    const string LEVEL_POSITION_Z = "_position_Z_";
    const string PLAYER_HEALTH = "player_health";
    const string PLAYER_XP = "player_xp";
    const string PLAYER_LEVEL = "player_level";

    // Sets the master volume to the float given
    public static void SetMasterVolume(float volume)
    {
        if (volume >= 0f && volume <= 1f)
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("Master volume must be between 0 and 1");
        }
    }

    // Gets the master volume
    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
    }

    // Stores the X, Y and Z positions of the given gameobject in a given level to floats in Unity's PlayerPrefs
    public static void SetPositionInLevel(string level, GameObject gameObject)
    {
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_X + gameObject.ToString(), gameObject.transform.position.x);
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_Y + gameObject.ToString(), gameObject.transform.position.y);
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_Z + gameObject.ToString(), gameObject.transform.position.z);
    }

    // Gets the X, Y and Z positions of the given gameobject in a given level and puts them together in a Vector3
    public static Vector3 GetPositionInLevel(string level, GameObject gameObject)
    {
        float x = PlayerPrefs.GetFloat(level + LEVEL_POSITION_X + gameObject.ToString());
        float y = PlayerPrefs.GetFloat(level + LEVEL_POSITION_Y + gameObject.ToString());
        float z = PlayerPrefs.GetFloat(level + LEVEL_POSITION_Z + gameObject.ToString());
        return new Vector3(x, y, z);
    }

    // Sets the health of the player to an int in Unity's PlayerPrefs
    public static void SetPlayerHealth(int health)
    {
        PlayerPrefs.SetInt(PLAYER_HEALTH, health);
    }

    // Gets the health from Unity's PlayerPrefs
    public static float GetPlayerHealth()
    {
        return PlayerPrefs.GetInt(PLAYER_HEALTH);
    }

    // Sets the XP of the player to an int in Unity's PlayerPrefs
    public static void SetPlayerXP(int XP)
    {
        PlayerPrefs.SetInt(PLAYER_XP, XP);
    }

    // Gets the XP from Unity's PlayerPrefs
    public static int GetPlayerXP()
    {
        return PlayerPrefs.GetInt(PLAYER_XP);
    }

    // Sets the level of the player to an int in Unity's PlayerPrefs
    public static void SetPlayerLevel(int player_level)
    {
        PlayerPrefs.SetInt(PLAYER_LEVEL, player_level);
    }

    // Gets the level from Unity's PlayerPrefs
    public static int GetPlayerLevel()
    {
        return PlayerPrefs.GetInt(PLAYER_LEVEL);
    }

    // Used for setting the startpositions in the levels
    public static void SetStartPositions(string level, GameObject gameObject, float xPos, float yPos, float zPos)
    {
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_X + gameObject.ToString(), xPos);
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_Y + gameObject.ToString(), yPos);
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_Z + gameObject.ToString(), zPos);
    }
}