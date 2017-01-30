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
    const string MOTION_BLUR_SCROLLBAR_KEY = "motion_blur_scroll";
    const string DEPTH_OF_FIELD_SCROLL_KEY = "depth_of_field_scroll";
    const string SSAO_SCROLL_KEY = "ssao";
    const string NOISE_AND_GRAIN_TOGGLE_KEY = "noise_and_grain";
    const string BLOOM_TOGGLE_KEY = "bloom";
    const string ANTI_ALIASING_DROPDOWN_KEY = "anti-aliasing";
    const string TARGET_FRAMERATE_KEY = "target_framerate";
    const string FIRST_TIME_DUNGEON_PIPI_KEY = "first_time_dungeon_pipi";
    const string FIRST_TIME_DUNEGON_FACEBEER_KEY = "first_time_dungeon_facebeer";
    const string AMOUNT_OF_WALLS_IN_DUNGEON_PIPI_KEY = "amount_of_walls_in_dungeon_pipi";
    const string AMOUNT_OF_WALLS_IN_DUNGEON_FACEBEER_KEY = "amount_of_walls_in_dungeon_facebeer";
    const string AMOUNT_OF_ENEMIES_IN_DUNGEON_PIPI_KEY = "amount_of_enemies_in_dungeon_pipi";
    const string AMOUNT_OF_ENEMIES_IN_DUNGEON_FACEBEER_KEY = "amount_of_enemies_in_dungeon_facebeer";
    const string ENDPOINTPOS_DUNGEON_PIPI_KEY = "endpointpos_dungeon_pipi_";
    const string ENDPOINTPOS_DUNGEON_FACEBEER_KEY = "endpointpos_dungeon_facebeer_";
    const string SAVE_GAME_CURRENT_SCENE_KEY = "save_game_current_scene";
    const string SAVE_GAME_CURRENT_POSITION_KEY = "save_game_current_position_";
    const string SAVE_GAME_CURRENT_PLAYER_XP_KEY = "save_game_current_player_xp";
    const string SAVE_GAME_CURRENCY_KEY = "save_game_currency";
    const string SAVE_GAME_CURRENT_HEALTH_KEY = "save_game_current_health";

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
        int health = PlayerPrefs.GetInt(PLAYER_HEALTH_KEY);
        return health;
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

    // Set start positions for player
    public static void SetStartPositions(string level, float xPos, float yPos, float zPos)
    {
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_X_KEY + "Player (UnityEngine.GameObject)", xPos);
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_Y_KEY + "Player (UnityEngine.GameObject)", yPos);
        PlayerPrefs.SetFloat(level + LEVEL_POSITION_Z_KEY + "Player (UnityEngine.GameObject)", zPos);
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

    // Saves the current scene for the ingame save button
    public static void SetSavedScene(string scene)
    {
        PlayerPrefs.SetString(SAVE_GAME_CURRENT_SCENE_KEY, scene);
    }

    // Gets the saved scene for the ingame load button
    public static string GetSavedScene()
    {
        return PlayerPrefs.GetString(SAVE_GAME_CURRENT_SCENE_KEY);
    }
    
    // Saves the current playerposition for the ingame save button
    public static void SetSavedPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat(SAVE_GAME_CURRENT_POSITION_KEY + "x", position.x);
        PlayerPrefs.SetFloat(SAVE_GAME_CURRENT_POSITION_KEY + "y", position.y);
        PlayerPrefs.SetFloat(SAVE_GAME_CURRENT_POSITION_KEY + "z", position.z);
    }

    // Gets te saved position for the ingame load button
    public static Vector3 GetSavedPosition()
    {
        float x = PlayerPrefs.GetFloat(SAVE_GAME_CURRENT_POSITION_KEY + "x");
        float y = PlayerPrefs.GetFloat(SAVE_GAME_CURRENT_POSITION_KEY + "y");
        float z = PlayerPrefs.GetFloat(SAVE_GAME_CURRENT_POSITION_KEY + "z");
        return new Vector3(x, y, z);
    }

    // Set the saved playerxp for the ingame save button
    public static void SetSavedPlayerXP(int value)
    {
        PlayerPrefs.SetInt(SAVE_GAME_CURRENT_PLAYER_XP_KEY, value);
    }

    // Get the saved playerxp for the ingame load button
    public static int GetSavedPlayerXP()
    {
        return PlayerPrefs.GetInt(SAVE_GAME_CURRENT_PLAYER_XP_KEY);
    }

    // Store the currency for the ingame save button
    public static void SetSavedCurrency(int value)
    {
        PlayerPrefs.SetInt(SAVE_GAME_CURRENCY_KEY, value);
    }

    // Get the stored currency for the load button
    public static int GetSavedCurrency()
    {
        return PlayerPrefs.GetInt(SAVE_GAME_CURRENCY_KEY);
    }

    // Store the current playerhealth for the ingame save button
    public static void SetSavedCurrentHealth(int value)
    {
        PlayerPrefs.SetInt(SAVE_GAME_CURRENT_HEALTH_KEY, value);
    }

    // Get te stored current health for the ingame load button
    public static int GetSavedCurrentHealth()
    {
        return PlayerPrefs.GetInt(SAVE_GAME_CURRENT_HEALTH_KEY);
    }

    #region Dungeons
    // States if the facebeer dungeon has been made once
    public static void SetFirstTimeFaceBeerDungeon(int TrueIs1FalseIs0)
    {
        if (TrueIs1FalseIs0 == 1 || TrueIs1FalseIs0 == 0)
        {
            PlayerPrefs.SetInt(FIRST_TIME_DUNEGON_FACEBEER_KEY, TrueIs1FalseIs0);
        }
        else
        {
            Debug.LogError("Value must be 0 or 1");
        }
    }

    // Gets if the facebeer dungeon has been made once
    public static bool GetFirstTimeFaceBeerDungeon()
    {
        int value = PlayerPrefs.GetInt(FIRST_TIME_DUNEGON_FACEBEER_KEY);
        if (value == 1)
        {
            return true;
        }
        else if (value == 0)
        {
            return false;
        }
        else
        {
            throw new UnityException("Value is not 0 or 1");
        }
    }

    // States if the facebeer dungeon has been made once
    public static void SetFirstTimePiPiDungeon(int TrueIs1FalseIs0)
    {
        if (TrueIs1FalseIs0 == 1 || TrueIs1FalseIs0 == 0)
        {
            PlayerPrefs.SetInt(FIRST_TIME_DUNGEON_PIPI_KEY, TrueIs1FalseIs0);
        }
        else
        {
            Debug.LogError("Value must be 0 or 1");
        }
    }

    // Gets if the facebeer dungeon has been made once
    public static bool GetFirstTimePiPiDungeon()
    {
        int value = PlayerPrefs.GetInt(FIRST_TIME_DUNGEON_PIPI_KEY);
        if (value == 1)
        {
            return true;
        }
        else if (value == 0)
        {
            return false;
        }
        else
        {
            throw new UnityException("Value is not 0 or 1");
        }
    }

    // Sets the amount of walls in the pipi dungeon
    public static void SetAmountOfWallsInPiPiDungeon(int value)
    {
        PlayerPrefs.SetInt(AMOUNT_OF_WALLS_IN_DUNGEON_PIPI_KEY, value);
    }

    // Gets the amount of walls in the pipi dungeon
    public static int GetAmountOfWallsInPiPiDungeon()
    {
        return PlayerPrefs.GetInt(AMOUNT_OF_WALLS_IN_DUNGEON_PIPI_KEY);
    }

    // Sets the amount of walls in the facebeer dungeon
    public static void SetAmountOfWallsInFaceBeerDungeon(int value)
    {
        PlayerPrefs.SetInt(AMOUNT_OF_WALLS_IN_DUNGEON_FACEBEER_KEY, value);
    }

    // Gets the amount of walls in the facebeer dungeon
    public static int GetAmountOfWallsInFaceBeerDungeon()
    {
        return PlayerPrefs.GetInt(AMOUNT_OF_WALLS_IN_DUNGEON_FACEBEER_KEY);
    }

    // Sets the amount of enemies in the pipi dungeon
    public static void SetAmountOfEnemiesInPiPiDungeon(int value)
    {
        PlayerPrefs.SetInt(AMOUNT_OF_ENEMIES_IN_DUNGEON_PIPI_KEY, value);
    }

    // Gets the amount of enemies in the pipi dungeon
    public static int GetAmountOfEnemiesInPiPiDungeon()
    {
        return PlayerPrefs.GetInt(AMOUNT_OF_ENEMIES_IN_DUNGEON_PIPI_KEY);
    }

    // Sets the amount of enemies in the facebeer dungeon
    public static void SetAmountOfEnemiesInFaceBeerDungeon(int value)
    {
        PlayerPrefs.SetInt(AMOUNT_OF_ENEMIES_IN_DUNGEON_FACEBEER_KEY, value);
    }

    // Gets the amount of enemies in the facebeer dungeon
    public static int GetAmountOfEnemiesInFaceBeerDungeon()
    {
        return PlayerPrefs.GetInt(AMOUNT_OF_ENEMIES_IN_DUNGEON_FACEBEER_KEY);
    }

    // Stores the position of the endpoint in the pipi dungeon
    public static void SetEndPointPosPiPiDungeon(Vector3 pos)
    {
        PlayerPrefs.SetFloat(ENDPOINTPOS_DUNGEON_PIPI_KEY + "x", pos.x);
        PlayerPrefs.SetFloat(ENDPOINTPOS_DUNGEON_PIPI_KEY + "y", pos.y);
        PlayerPrefs.SetFloat(ENDPOINTPOS_DUNGEON_PIPI_KEY + "z", pos.z);
    }

    // Get the position of the endpoint in the pipi dungeon
    public static Vector3 GetEndPointPosPiPiDungeon()
    {
        float x = PlayerPrefs.GetFloat(ENDPOINTPOS_DUNGEON_PIPI_KEY + "x");
        float y = PlayerPrefs.GetFloat(ENDPOINTPOS_DUNGEON_PIPI_KEY + "y");
        float z = PlayerPrefs.GetFloat(ENDPOINTPOS_DUNGEON_PIPI_KEY + "z");
        return new Vector3(x, y, z);
    }

    // Stores the position of the endpoint in the facebeer dungeon
    public static void SetEndPointPosFaceBeerDungeon(Vector3 pos)
    {
        PlayerPrefs.SetFloat(ENDPOINTPOS_DUNGEON_FACEBEER_KEY + "x", pos.x);
        PlayerPrefs.SetFloat(ENDPOINTPOS_DUNGEON_FACEBEER_KEY + "y", pos.y);
        PlayerPrefs.SetFloat(ENDPOINTPOS_DUNGEON_FACEBEER_KEY + "z", pos.z);
    }

    // Get the position of the endpoint in the facebeer dungeon
    public static Vector3 GetEndPointPosFaceBeerDungeon()
    {
        float x = PlayerPrefs.GetFloat(ENDPOINTPOS_DUNGEON_FACEBEER_KEY + "x");
        float y = PlayerPrefs.GetFloat(ENDPOINTPOS_DUNGEON_FACEBEER_KEY + "y");
        float z = PlayerPrefs.GetFloat(ENDPOINTPOS_DUNGEON_FACEBEER_KEY + "z");
        return new Vector3(x, y, z);
    }
    #endregion

    #region Graphicssettings
    // Set the motion blur value
    public static void SetMotionBlur(float value)
    {
        if (value >= 0 && value <= 0.92)
        {
            PlayerPrefs.SetFloat(MOTION_BLUR_SCROLLBAR_KEY, value);
        }
        else
        {
            Debug.LogError("Motion Blur value must be between 0 and 0.92");
        }
    }

    // Get the motion blur value
    public static float GetMotionBlur()
    {
        return PlayerPrefs.GetFloat(MOTION_BLUR_SCROLLBAR_KEY);
    }

    // Set the depth of field value
    public static void SetDepthOfField(float value)
    {
        PlayerPrefs.SetFloat(DEPTH_OF_FIELD_SCROLL_KEY, value);
    }

    // Get the depth of field value
    public static float GetDepthOfField()
    {
        return PlayerPrefs.GetFloat(DEPTH_OF_FIELD_SCROLL_KEY);
    }

    // Set the SSAO
    public static void SetSSAO(float value)
    {
        PlayerPrefs.SetFloat(SSAO_SCROLL_KEY, value);
    }

    // Get the SSAO
    public static float GetSSAO()
    {
        return PlayerPrefs.GetFloat(SSAO_SCROLL_KEY);
    }

    // Toggle the noise and grain script
    public static void SetNoiseAndGrain(int TrueIs1FalseIs0)
    {
        if (TrueIs1FalseIs0 == 1 || TrueIs1FalseIs0 == 0)
        {
            PlayerPrefs.SetInt(NOISE_AND_GRAIN_TOGGLE_KEY, TrueIs1FalseIs0);
        }
        else
        {
            Debug.LogError("Value must be 0 or 1");
        }
    }

    // Get the noise and grain toggle
    public static bool GetNoiseAndGrain()
    {
        int value = PlayerPrefs.GetInt(NOISE_AND_GRAIN_TOGGLE_KEY);
        if (value == 1)
        {
            return true;
        }
        else if (value == 0)
        {
            return false;
        }
        else
        {
            throw new UnityException("Value is not 0 or 1");
        }
    }
    
    // Toggle the bloom script
    public static void SetBloom(int TrueIs1FalseIs0)
    {
        if (TrueIs1FalseIs0 == 1 || TrueIs1FalseIs0 == 0)
        {
            PlayerPrefs.SetInt(BLOOM_TOGGLE_KEY, TrueIs1FalseIs0);
        }
        else
        {
            Debug.LogError("Value must be 0 or 1");
        }
    }

    // Get the bloom toggle
    public static bool GetBloom()
    {
        int value = PlayerPrefs.GetInt(BLOOM_TOGGLE_KEY);
        if (value == 1)
        {
            return true;
        }
        else if (value == 0)
        {
            return false;
        }
        else
        {
            throw new UnityException("Value is not 0 or 1");
        }
    }

    // Store the anti-aliasing option
    public static void SetAntiAliasing(int value)
    {
        if (value == 0 || value == 1 || value == 2)
        {
            PlayerPrefs.SetInt(ANTI_ALIASING_DROPDOWN_KEY, value);
        }
        else
        {
            Debug.LogError("Invalid choise of anti-aliasing");
        }
    }

    // Get the anti-aliasing option
    public static int GetAntiAliasing()
    {
        return PlayerPrefs.GetInt(ANTI_ALIASING_DROPDOWN_KEY);
    }

    // Set the target framerate
    public static void SetTargetFramerate(int value)
    {
        PlayerPrefs.SetInt(TARGET_FRAMERATE_KEY, value);
    }

    // Gets the target framerate
    public static int GetTargetFramerate()
    {
        return PlayerPrefs.GetInt(TARGET_FRAMERATE_KEY);
    }
    #endregion
}