using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dungeon_Enemy : MonoBehaviour
{
    private bool canBeFought = true;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && canBeFought)
        {
            Debug.Log("IN COMBAT ZONE");

            // Enter combat scene
            PlayerPrefsManager.SetCurrentScene(SceneManager.GetActiveScene().name);
            PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);
            SceneManager.LoadScene("Combat");
            player.transform.position = PlayerPrefsManager.GetPositionInLevel("Combat", player);

            Debug.Log("saved scene: " + PlayerPrefsManager.GetCurrentScene());
            Debug.Log("saved pos in dungeon: " + PlayerPrefsManager.GetPositionInLevel("Dungeon_FaceBeer", player));

            // DEBUG, saving the dungeon immediately after
            Debug.Log("loading dungeon again");
            SceneManager.LoadScene(PlayerPrefsManager.GetCurrentScene());
            player.transform.position = PlayerPrefsManager.GetPositionInLevel(PlayerPrefsManager.GetCurrentScene(), player);
            Debug.Log("DONE");
            // END DEBUG
        }
    }
}
