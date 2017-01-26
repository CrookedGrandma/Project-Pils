using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dungeon_Enemy : MonoBehaviour
{
    private GameObject player;
    private PlayerFSM playerFSM;
    private DungeonCreator dungeonCreator;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerFSM = GameObject.FindObjectOfType<PlayerFSM>();
        dungeonCreator = GameObject.FindObjectOfType<DungeonCreator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            // Enter combat scene
            Destroy(gameObject);
            dungeonCreator.SaveLayoutOfEnemies();
            PlayerPrefsManager.SetCurrentScene(SceneManager.GetActiveScene().name);
            PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);
            SceneManager.LoadScene("Combat");
            player.transform.position = PlayerPrefsManager.GetPositionInLevel("Combat", player);

            if (SceneManager.GetActiveScene().name == "Dungeon_FaceBeer")
            {
                playerFSM.Enemy = 4;
                playerFSM.Envi = 0;
            }
            else if (SceneManager.GetActiveScene().name == "Dungeon_PiPi")
            {
                // playerFSM.Enemy = 3;         <-- Dit is degene die het moet worden als ie een sprite heeft
                playerFSM.Enemy = 4;            // <-- Deze is van FaceBeer
                playerFSM.Envi = 2;
            }
        }
    }
}
