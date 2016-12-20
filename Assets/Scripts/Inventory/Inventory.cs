using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    GameObject inventoryPanel;
    GameObject slotPanel;
    GameObject equipmentPanel;
    public GameObject weaponslot;
    public GameObject ammoslot;
    public GameObject headslot;
    public GameObject bodyslot;
    public GameObject lowerslot;
    public GameObject shoeslot;
    ItemDatabase database;
    public GameObject inventorySlot;
    public  GameObject inventoryItem;
    public int slotCount;
    private int equipmentCount;
    private GameObject[] equipmentList;

    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        equipmentList = new GameObject[] { weaponslot, ammoslot, headslot, bodyslot, lowerslot, shoeslot };
        database = GetComponent<ItemDatabase>();
        slotCount = 36;
        equipmentCount = 6;
        inventoryPanel = GameObject.Find("InventoryPanel");
        slotPanel = inventoryPanel.transform.FindChild("SlotPanel").gameObject;
        equipmentPanel = inventoryPanel.transform.FindChild("EquipmentPanel").gameObject;
        for (int i = 0; i < slotCount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<InventorySlot>().slotID = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }
        for (int i = slotCount; i < equipmentCount + slotCount; i++)
        {
            items.Add(new Item());
            slots.Add(equipmentList[i - slotCount]);
            slots[i].GetComponent<InventorySlot>().slotID = i;
            slots[i].transform.SetParent(equipmentPanel.transform);
        }
        AddItem(0);
        AddItem(2);
        AddItem(3);
        AddItem(4);
        AddItem(5);
        AddItem(6);
        AddItem(7);
        AddItem(8);
        AddItem(9);
        AddItem(9);
        AddItem(9);
        AddItem(10);
        AddItem(11);
        AddItem(12);
        AddItem(13);
        AddItem(14);
    }

    public void AddItem(int ID)
    {
        Item itemToAdd = database.FetchItemById(ID);
        if (itemToAdd.Stackable && CheckIfItemInInventory(itemToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == ID)
                {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    if (data.amount == 0)
                    {
                        data.amount = 1;
                    }
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == -1)
                {
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemToAdd;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObj.name = itemToAdd.Title;
                    break;
                }
            }
        }
    }

    public void RemoveItem(Item item)
    {
        Item nullItem = database.FetchItemById(999);
        if (item.Stackable)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == item.ID)
                {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    if (data.amount == 1)
                    {
                        data.amount = 0;
                        break;
                    }
                    else if (data.amount != 0) data.amount--;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == item.ID)
                {
                    items[i] = nullItem;
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    GameObject oldItem = inventoryItem;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = nullItem;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = nullItem.Sprite;
                    itemObj.name = nullItem.Title;
                    break;
                }
            }
        }
    }

    bool CheckIfItemInInventory(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == item.ID)
            {
                return true;
            }
        }
        return false;
    }
}
