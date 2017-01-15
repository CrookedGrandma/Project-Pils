using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PersistentInventoryScript : MonoBehaviour {
    public GameObject persistentInventory;
    public static PersistentInventoryScript instance;
    //weaponslot, ammoslot, headslot, bodyslot, lowerslot, shoeslot
    public int[,] equipmentList = new int[6,2];
    public int[,] itemList;
    public int slotCount;
    public bool InShop;
    public int itemDamage { get; set; }
    public int itemDefense { get; set; }
    public int itemHealth { get; set; }
    public int Currency { get; set; }

    void Start () {
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
        addItem(1,0);
        addEquipment(602, 1, 2);
        addEquipment(809, 1, 4);
        for (int x = 0; x < itemList.Length / 2; x++)
        {
        }
    }
	
    public void addItem(int id, int slot)
    {
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
        equipmentList[slot,0] = id;
        equipmentList[slot,1] = number;
    }
    public void removeEquipment(int slot, int number)
    {
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
