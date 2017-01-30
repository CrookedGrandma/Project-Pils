using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    ///<summary>
    /// Method that loads a certain level.
    public void Loadlevel(string name)
    {
        SceneManager.LoadScene(name);
        GameObject player = GameObject.Find("Player");
        player.transform.position = PlayerPrefsManager.GetPositionInLevel(name, player);
    }

    ///<summary>
    /// Method that quits the game.
    public void QuitGame()
    {
        Application.Quit();
    }

    ///<summary>
    /// Method that unpauses the game when it is paused.
    public void UnPause()
    {
        Time.timeScale = 1;
    }

    ///<summary>
    /// Method that loads the saved game
    public void LoadSavedGame()
    {
        SceneManager.LoadScene(PlayerPrefsManager.GetSavedScene());
        GameObject player = GameObject.Find("Player");
        player.transform.position = PlayerPrefsManager.GetSavedPosition();
        XPManager.xpmanager.playerxp = PlayerPrefsManager.GetSavedPlayerXP();
        PersistentInventoryScript.instance.Currency = PlayerPrefsManager.GetSavedCurrency();
        LoadInventory();
    }

    ///<summary>
    /// Loads te inventory including the saved items
    private void LoadInventory()
    {
        PersistentInventoryScript inventory = GameObject.FindObjectOfType<PersistentInventoryScript>();
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
