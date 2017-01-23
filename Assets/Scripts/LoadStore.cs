using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStore : MonoBehaviour
{

    private PersistentInventoryScript Script;
    private GameObject player;
    public string VendorName;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        VendorName = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<Nameplate>() != null)
            Debug.Log(other.GetComponentInParent<Nameplate>().nameplateName);

        Script = GameObject.Find("PersistentInventory").GetComponent<PersistentInventoryScript>();
        string Shopnaam = other.GetComponentInParent<Nameplate>().nameplateName;
        Script.shopType = Shopnaam;
        PlayerPrefsManager.SetCurrentScene(SceneManager.GetActiveScene().name);
        PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);
    }

    /*private void OnTriggerExit(Collider other)
    {
        Script.shopType = null;
    }*/
}
