using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    const string MASTER_VOLUME_KEY = "master_volume";
    const string LEVEL_POSITION_X = "_position_X_";
    const string LEVEL_POSITION_Y = "_position_Y_";
    const string LEVEL_POSITION_Z = "_position_Z_";
    const string PLAYER_HEALTH = "player_health";
    const string PLAYER_XP = "player_xp";
    const string PLAYER_LEVEL = "player_level";

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

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
    }

    public static void SetPositionInLevel(string level, GameObject gameObject)
    {
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_X + gameObject.ToString(), gameObject.transform.position.x);
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_Y + gameObject.ToString(), gameObject.transform.position.y);
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_Z + gameObject.ToString(), gameObject.transform.position.z);
    }

    public static Vector3 GetPositionInLevel(string level, GameObject gameObject)
    {
        float x = PlayerPrefs.GetFloat(level + LEVEL_POSITION_X + gameObject.ToString());
        float y = PlayerPrefs.GetFloat(level + LEVEL_POSITION_Y + gameObject.ToString());
        float z = PlayerPrefs.GetFloat(level + LEVEL_POSITION_Z + gameObject.ToString());
        Vector3 position = new Vector3(x, y, z);

        if (level == "Home")
        {
            position = new Vector3(2f, 2f, -3f);
            Debug.LogError("No position stored in Home");
        }

        return position;
    }

    public static void SetPlayerHealth(float health)
    {
        PlayerPrefs.SetFloat(PLAYER_HEALTH, health);
    }

    public static float GetPlayerHealth()
    {
        return PlayerPrefs.GetFloat(PLAYER_HEALTH);
    }

    public static void SetPlayerXP(int XP)
    {
        PlayerPrefs.SetInt(PLAYER_XP, XP);
    }

    public static int GetPlayerXP()
    {
        return PlayerPrefs.GetInt(PLAYER_XP);
    }

    public static void SetPlayerLevel(int player_level)
    {
        PlayerPrefs.SetInt(PLAYER_LEVEL, player_level);
    }

    public static int GetPlayerLevel()
    {
        return PlayerPrefs.GetInt(PLAYER_LEVEL);
    }
}