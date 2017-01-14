using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopButton : MonoBehaviour
{
    public GameObject knop;
    GameObject persistentInventoryObject;
    PersistentInventoryScript persistentInventory;

    private void Start()
    {
        persistentInventoryObject = GameObject.Find("PersistentInventory");
        persistentInventory = persistentInventoryObject.GetComponent<PersistentInventoryScript>();
        if (persistentInventory.InShop == false)
        {
            knop.SetActive(false);
        }
        else
        {
            knop.SetActive(true);
        }
    }
    public void GoToInventory()
    {
        SceneManager.LoadScene("Inventory");
    }
    public void GoToShop()
    {
        SceneManager.LoadScene("Shop");
    }
}