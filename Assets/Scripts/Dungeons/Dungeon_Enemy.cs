using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dungeon_Enemy : MonoBehaviour
{
    private bool canBeFought = true;
    private GameObject player;
    private PlayerFSM playerFSM;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerFSM = GameObject.FindObjectOfType<PlayerFSM>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && canBeFought)
        {
            // Enter combat scene
            canBeFought = false;
            PlayerPrefsManager.SetCurrentScene(SceneManager.GetActiveScene().name);
            PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);
            SceneManager.LoadScene("Combat");
            player.transform.position = PlayerPrefsManager.GetPositionInLevel("Combat", player);
            playerFSM.Enemy = 4;
            playerFSM.Envi = 0;
        }
    }
}
