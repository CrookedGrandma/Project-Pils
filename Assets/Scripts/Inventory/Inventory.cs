using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    GameObject inventoryPanel;
    GameObject slotPanel;
    GameObject equipmentPanel;
    public GameObject tooltip;
    public GameObject weaponslot;
    public GameObject ammoslot;
    public GameObject headslot;
    public GameObject bodyslot;
    public GameObject lowerslot;
    public GameObject shoeslot;
    public GameObject InventoryData;
    public GameObject InventoryMoney;
    GameObject persistentInventoryObject;
    ItemDatabase database;
    PersistentInventoryScript persistentInventory;
    public GameObject inventorySlot;
    public  GameObject inventoryItem;
    public int slotCount;
    private int equipmentCount;
    private GameObject[] equipmentList;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();
    private string stats;
    private int damage;
    private int defence;
    private int health;

    private void Start()
    {
        persistentInventoryObject = GameObject.Find("PersistentInventory");
        equipmentList = new GameObject[] { weaponslot, ammoslot, headslot, bodyslot, lowerslot, shoeslot };
        persistentInventory = persistentInventoryObject.GetComponent<PersistentInventoryScript>();
        database = GetComponent<ItemDatabase>();
        slotCount = persistentInventory.slotCount;
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
        //Zet items van PersistentInventory in normale inventory
        for (int i = 0; i < persistentInventory.itemList.Length / 2; i++)
        {
            if (persistentInventory.itemList[i, 0] != 0)
            {
                AddItem(persistentInventory.itemList[i, 0]);
            }
        }
        //Zet equipmentment van PersistentInventory in normale equipment
        for (int i = 0; i < (persistentInventory.equipmentList.Length / 2); i++)
        {
            if (persistentInventory.equipmentList[i, 0] != 0)
            {
                AddEquipment(persistentInventory.equipmentList[i, 0], i, persistentInventory.equipmentList[i, 1]);
            }
        }
    }

    public void Update()
    {
        UpdateInventoryData();
        UpdateInventoryMoney();
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
                if (items[i].ID == -1 || items[i].ID == 0)
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

    public void AddEquipment(int ID, int equipmentSlot, int number)
    {
        Item itemToAdd = database.FetchItemById(ID);
        int i = slotCount + equipmentSlot;
        if (items[i].ID == -1)
        {
            items[i] = itemToAdd;
            GameObject itemObj = Instantiate(inventoryItem);
            itemObj.GetComponent<ItemData>().item = itemToAdd;
            itemObj.GetComponent<ItemData>().slot = i;
            itemObj.transform.SetParent(slots[i].transform);
            itemObj.transform.position = slots[slotCount + equipmentSlot].transform.position;
            itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
            itemObj.name = itemToAdd.Title;
        }
        if (number > 1)
        {
            ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
            data.amount = number;
            data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
        }
    }

    public void RemoveItem(Item item, ItemData itemData)
    {
        if (item.Stackable)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == item.ID)
                {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    if (data.amount <= 1)
                    {
                        Destroy(itemData.gameObject);
                        tooltip.SetActive(false);
                        break;
                    }   
                    else if (data.amount > 1) data.amount--;
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
                    Destroy(itemData.gameObject);
                    tooltip.SetActive(false);
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
    public void UpdateInventoryData()
    {
        stats = "";
        damage = 0;
        defence = 0;
        health = 0;
        for (int i = 0; i < equipmentList.Length; i++)
        {
            if (equipmentList[i].transform.childCount > 0)
            {
                ItemData itemData = equipmentList[i].transform.GetChild(0).GetComponent<ItemData>();
                if (itemData.item.Type == "weapon")
                {
                    damage += itemData.item.Damage;
                }
                if (itemData.item.Type == "equipment")
                {
                    damage += itemData.item.Attack;
                    defence += itemData.item.Defence;
                    health += itemData.item.Health;
                }
            }
        }
        persistentInventory.itemDamage = damage;
        persistentInventory.itemDefense = defence;
        persistentInventory.itemHealth = health;
        stats = "Attack: " + damage + "\nDefense: " + defence + "\nHealth: " + health;
        InventoryData.GetComponent<Text>().supportRichText = true;
        InventoryData.GetComponent<Text>().text = stats;
    }

    public void UpdateInventoryMoney()
    {
        string currency = "CURRENCY: " + persistentInventory.Currency.ToString();
        InventoryMoney.GetComponent<Text>().supportRichText = true;
        InventoryMoney.GetComponent<Text>().text = currency;
    }
}
