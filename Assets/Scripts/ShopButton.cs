using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopButton : MonoBehaviour
{
    public GameObject knopInventoryShop;
    public GameObject KnopGame;
    GameObject persistentInventoryObject;
    PersistentInventoryScript persistentInventory;
    private GameObject player;

    private void Start()
    {
        persistentInventoryObject = GameObject.Find("PersistentInventory");
        persistentInventory = persistentInventoryObject.GetComponent<PersistentInventoryScript>();
        if (persistentInventory.InShop == false)
        {
            knopInventoryShop.SetActive(false);
            KnopGame.SetActive(true);
        }
        else
        {
            knopInventoryShop.SetActive(true);
            KnopGame.SetActive(false);
        }

        player = GameObject.Find("Player");
    }
    public void GoToInventory()
    {
        SceneManager.LoadScene("Inventory");
    }
    public void GoToShop()
    {
        SceneManager.LoadScene("Shop");
    }
    public void ReturnToGame()
    {
        SceneManager.LoadScene(PlayerPrefsManager.GetCurrentScene());
        Vector3 playerPos = PlayerPrefsManager.GetPositionInLevel(PlayerPrefsManager.GetCurrentScene(), player);
        player.transform.position = playerPos;
    }
}