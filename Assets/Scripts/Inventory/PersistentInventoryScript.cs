using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PersistentInventoryScript : MonoBehaviour {
    public GameObject persistentInventory;
    public static PersistentInventoryScript instance;
    //0=weaponslot, 1=ranged/throwslot, 2=headslot, 3=bodyslot, 4=lowerslot, 5=shoeslot
    public int[,] equipmentList = new int[6,2];
    public int[,] itemList;
    public int slotCount;
    public bool InShop;
    public string shopType;
    public int itemDamage { get; set; }
    public int itemDefense { get; set; }
    public int itemHealth { get; set; }
    public int Currency { get; set; }

    void Start () {
        InShop = false;
        DontDestroyOnLoad(persistentInventory);
        Currency = 200;
        if (instance == null)
        {
            instance = this;
            InShop = true;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        slotCount = 40;
        itemList = new int[slotCount, 2];
        addEquipment(809, 1, 4);
    }
	
    public void addItem(int id, int slot)
    {
        Debug.Log("AddItem");
        if (id == itemList[slot, 0])
        {
            itemList[slot, 1] += 1;
        }
        else
        {
            itemList[slot, 0] = id;
            itemList[slot, 1] = 1;
        }
    }
    public void removeItem(int id, int slot)
    {
        Debug.Log("RemoveItem");
        if (itemList[slot, 1] > 1)
        {
            itemList[slot, 1] -= 1;
        }
        else
        {
            itemList[slot, 0] = 0;
            itemList[slot, 1] = 0;
        }
    }
    public void addEquipment(int id, int number, int slot)
    {
        Debug.Log("AddEquipment");
        equipmentList[slot,0] = id;
        equipmentList[slot,1] = number;
    }
    public void removeEquipment(int slot, int number)
    {
        Debug.Log("RemoveEquipment");
        if (equipmentList[slot, 1] > 1)
        {
            equipmentList[slot, 1] -= 1;
        }
        else
        {
            equipmentList[slot, 0] = 0;
            equipmentList[slot, 1] = 0;
        }
    }
}
