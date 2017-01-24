using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dungeon_Enemy : MonoBehaviour
{
    private bool canBeFought = true;
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
        if (other.name == "Player" && canBeFought)
        {
            // Enter combat scene
            canBeFought = false;
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
                /*playerFSM.Enemy = ;
                playerFSM.Envi = ;*/
            }
        }
    }
}
