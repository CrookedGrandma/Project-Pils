using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScene : MonoBehaviour
{
    public GameObject confirmationPanel;
    public GameObject gameSavedPanel;
    public bool inPauseMenu;

    private bool confirmationpanelVisible;
    private bool savedGamePanelVisible;
    private float countDown = -1;
    private GameObject player;
    private PersistentInventoryScript inventory;

    private void Start()
    {
        player = GameObject.Find("Player");
        inventory = GameObject.FindObjectOfType<PersistentInventoryScript>();
        SetConfirmationScreenInvisible();
        SetSavedGamePanelInvisible();
        inPauseMenu = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !confirmationpanelVisible && inPauseMenu)
        {
            ExitPauseMenu();
        }

        if (savedGamePanelVisible)
        {
            if (countDown > 0)
            {
                countDown -= Time.deltaTime;
            }
            if (countDown <= 0)
            {
                SetSavedGamePanelInvisible();
            }
        }
    }

    public void ExitPauseMenu()
    {
        SceneManager.LoadScene(PlayerPrefsManager.GetCurrentScene());
        player.transform.position = PlayerPrefsManager.GetPositionInLevel(PlayerPrefsManager.GetCurrentScene(), player);
        inPauseMenu = false;
    }

    public void Continue()
    {
        ExitPauseMenu();
    }

    public void LoadInventory()
    {
        PersistentInventoryScript.instance.InShop = false;
        PlayerPrefsManager.SetPositionInLevel(SceneManager.GetActiveScene().name, player);
        SceneManager.LoadScene("Inventory");
        player.transform.position = PlayerPrefsManager.GetPositionInLevel("Inventory", player);
    }

    public void SaveGame()
    {
        PlayerPrefsManager.SetSavedScene(PlayerPrefsManager.GetCurrentScene());
        PlayerPrefsManager.SetSavedPosition(PlayerPrefsManager.GetPositionInLevel(PlayerPrefsManager.GetCurrentScene(), player));
        PlayerPrefsManager.SetSavedPlayerXP(XPManager.xpmanager.playerxp);
        PlayerPrefsManager.SetSavedCurrency(PersistentInventoryScript.instance.Currency);
        PlayerPrefsManager.SetSavedCurrentHealth((int)PlayerPrefsManager.GetPlayerHealth());
        SaveInventoryItems();

        SetSavedGamePanelVisible();
        countDown = 1;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(PlayerPrefsManager.GetSavedScene());
        player.transform.position = PlayerPrefsManager.GetSavedPosition();
        XPManager.xpmanager.playerxp = PlayerPrefsManager.GetSavedPlayerXP();
        PersistentInventoryScript.instance.Currency = PlayerPrefsManager.GetSavedCurrency();
        PlayerPrefsManager.SetPlayerHealth(PlayerPrefsManager.GetSavedCurrentHealth());
        LoadInventoryItems();
    }

    public void ExitToMainMenu()
    {
        SetConfirmationScreenVisible();
    }

    public void Confirm()
    {
        SetConfirmationScreenInvisible();
        SceneManager.LoadScene("MainMenu");
    } 

    public void Cancel()
    {
        SetConfirmationScreenInvisible();
    }

    private void SetConfirmationScreenInvisible()
    {
        confirmationPanel.SetActive(false);
        confirmationpanelVisible = false;
    }

    private void SetConfirmationScreenVisible()
    {
        confirmationPanel.SetActive(true);
        confirmationpanelVisible = true;
    }

    private void SetSavedGamePanelInvisible()
    {
        gameSavedPanel.SetActive(false);
        savedGamePanelVisible = false;
    }

    private void SetSavedGamePanelVisible()
    {
        gameSavedPanel.SetActive(true);
        savedGamePanelVisible = true;
    }

    private void SaveInventoryItems()
    {
        for (int i = 0; i < inventory.equipmentList.GetLength(0); i++)
        {
            PlayerPrefs.SetInt("save_inventory_equipment_" + i + "_slot", i);
            PlayerPrefs.SetInt("save_inventory_equipment_" + i + "_id", inventory.equipmentList[i, 0]);
            PlayerPrefs.SetInt("save_inventory_equipment_" + i + "_number", inventory.equipmentList[i, 1]);
        }

        for (int j = 0; j < inventory.itemList.GetLength(0); j++)
        {
            PlayerPrefs.SetInt("save_inventory_items_" + j + "_slot", j);
            PlayerPrefs.SetInt("save_inventory_items_" + j + "_id", inventory.itemList[j, 0]);
            PlayerPrefs.SetInt("save_inventory_items_" + j + "_number", inventory.itemList[j, 1]);
        }
    }

    private void LoadInventoryItems()
    {
        for (int i = 0; i < inventory.equipmentList.GetLength(0); i++)
        {
            for (int inventorySlot = 0; inventorySlot < inventory.equipmentList.GetLength(1); inventorySlot++)
            {
                inventory.removeEquipment(inventorySlot, 0);
            }

            int slot = PlayerPrefs.GetInt("save_inventory_equipment_" + i + "_slot");
            int id = PlayerPrefs.GetInt("save_inventory_equipment_" + i + "_id");
            int number = PlayerPrefs.GetInt("save_inventory_equipment_" + i + "_number");

            if (slot == 1)
            {
                // Ranged weapon
                number += 4;
            }
            if (slot == 0)
            {
                // Melee weapon
                number += 5;
            }

            inventory.addEquipment(id, number, slot);
        }

        for (int j = 0; j < inventory.itemList.GetLength(0); j++)
        {
            int slot = PlayerPrefs.GetInt("save_inventory_items_" + j + "_slot");
            int id = PlayerPrefs.GetInt("save_inventory_items_" + j + "_id");
            int number = PlayerPrefs.GetInt("save_inventory_items_" + j + "_number");

            for (int num = 0; num <= number; num++)
            {
                inventory.removeItem(0, j);
                inventory.addItem(id, slot);
            }
        }
    }
}
