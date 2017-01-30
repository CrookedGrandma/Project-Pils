using UnityEngine;
using UnityEngine.UI;
using System;
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
        Currency = 2000;
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
        bool placed = false;
        if (slot == -1)
        {
            for (int i = 0; i < itemList.Length / 2; i++)
            {
                if (itemList[i, 0] == id)
                {
                    itemList[i, 1] += 1;
                    placed = true;
                    break;
                }
            }
            if (!placed)
            {
                for (int i = 0; i < itemList.Length / 2; i++)
                {
                    if (itemList[i, 0] == 0)
                    {
                        itemList[i, 0] = id;
                        itemList[i, 1] += 1;
                        placed = true;
                        break;
                    }
                }
            }
        }
        else
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
    }

    public void addItemToEnd(int id)
    {
        Debug.Log("AddItemToEnd");

        for(int i = 0; i < itemList.GetLength(0); i++)
        {
            if(itemList[i,0] == 0)
            {
                itemList[i, 0] = id;
                itemList[i, 1] = 1;
                return;
            }
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

    public void removeItemFromEnd(int id)
    {
        Debug.Log("RemoveItemFromEnd");

        for (int i = 0; i < itemList.GetLength(0); i++)
        {
            if (itemList[i, 0] == id)
            {
                itemList[i, 0] = 0;
                if (itemList[i, 1] > 1)
                {
                    itemList[i, 1] -= 1;
                }
                else
                {
                    itemList[i, 1] = 0;
                }
                return;
            }
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

    public int returnNumberOfItems(int id) {
        int numberOfItems = 0;
        for (int i = 0; i < itemList.Length; i++) {
            try {
                if (itemList[i, 0] == id) {
                    numberOfItems += itemList[i, 1];
                }
            }
            catch (Exception e) { }
        }
        return numberOfItems;
    }
}
