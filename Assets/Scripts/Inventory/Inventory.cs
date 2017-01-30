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
    public GameObject InventorySets;
    GameObject persistentInventoryObject;
    ItemDatabase database;
    PersistentInventoryScript persistentInventory;
    public GameObject inventorySlot;
    public GameObject inventoryItem;
    public int slotCount;
    private int equipmentCount;
    private GameObject[] equipmentList;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();
    private string stats;
    private int damage;
    private int defence;
    private int health;
    private int equipDamage;
    //volgorde : dumpster, medieval, gentleman, arabic, mexican, chinese, sports,
    private int[] setArray = new int[7];

    private void Start() {
        persistentInventoryObject = GameObject.Find("PersistentInventory");
        equipmentList = new GameObject[] { weaponslot, ammoslot, headslot, bodyslot, lowerslot, shoeslot };
        persistentInventory = persistentInventoryObject.GetComponent<PersistentInventoryScript>();
        database = GetComponent<ItemDatabase>();
        slotCount = persistentInventory.slotCount;
        equipmentCount = 6;
        inventoryPanel = GameObject.Find("InventoryPanel");
        slotPanel = inventoryPanel.transform.FindChild("SlotPanel").gameObject;
        equipmentPanel = inventoryPanel.transform.FindChild("EquipmentPanel").gameObject;
        for (int i = 0; i < slotCount; i++) {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<InventorySlot>().slotID = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }
        for (int i = slotCount; i < equipmentCount + slotCount; i++) {
            items.Add(new Item());
            slots.Add(equipmentList[i - slotCount]);
            slots[i].GetComponent<InventorySlot>().slotID = i;
            slots[i].transform.SetParent(equipmentPanel.transform);
        }
        //Zet items van PersistentInventory in normale inventory
        for (int i = 0; i < persistentInventory.itemList.Length / 2; i++) {
            if (persistentInventory.itemList[i, 0] != 0) {
                for (int x = 0; x < persistentInventory.itemList[i,1]; x++)
                AddItem(persistentInventory.itemList[i, 0]);
            }
        }
        //Zet equipmentment van PersistentInventory in normale equipment
        for (int i = 0; i < (persistentInventory.equipmentList.Length / 2); i++) {
            if (persistentInventory.equipmentList[i, 0] != 0) {
                AddEquipment(persistentInventory.equipmentList[i, 0], i, persistentInventory.equipmentList[i, 1]);
            }
        }
    }

    public void Update() {
        UpdateInventoryData();
        UpdateInventoryMoney();
    }

    public void AddItem(int ID) {
        Item itemToAdd = database.FetchItemById(ID);
        if (itemToAdd.Stackable && CheckIfItemInInventory(itemToAdd)) {
            for (int i = 0; i < items.Count; i++) {
                if (items[i].ID == ID) {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
            }
        }
        else {
            for (int i = 0; i < items.Count; i++) {
                if (items[i].ID == -1 || items[i].ID == 0) {
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemToAdd;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.GetComponent<ItemData>().amount = 1;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObj.name = itemToAdd.Title;
                    break;
                }
            }
        }
    }

    public void AddEquipment(int ID, int equipmentSlot, int number) {
        Item itemToAdd = database.FetchItemById(ID);
        int i = slotCount + equipmentSlot;
        if (items[i].ID == -1) {
            items[i] = itemToAdd;
            GameObject itemObj = Instantiate(inventoryItem);
            itemObj.GetComponent<ItemData>().item = itemToAdd;
            itemObj.GetComponent<ItemData>().slot = i;
            itemObj.GetComponent<ItemData>().amount = 1;
            itemObj.transform.SetParent(slots[i].transform);
            itemObj.transform.position = slots[slotCount + equipmentSlot].transform.position;
            itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
            itemObj.name = itemToAdd.Title;
        }
        if (number > 1) {
            ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
            data.amount = number;
            data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
        }
    }

    public void RemoveItem(Item item, ItemData itemData) {
        if (item.Stackable) {
            for (int i = 0; i < items.Count; i++) {
                if (items[i].ID == item.ID) {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    if (data.amount <= 1) {
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
        else {
            for (int i = 0; i < items.Count; i++) {
                if (items[i].ID == item.ID) {
                    Destroy(itemData.gameObject);
                    tooltip.SetActive(false);
                    break;
                }
            }
        }
    }

    bool CheckIfItemInInventory(Item item) {
        for (int i = 0; i < items.Count; i++) {
            if (items[i].ID == item.ID) {
                return true;
            }
        }
        return false;
    }
    public void UpdateInventoryData() {
        stats = "";
        damage = 0;
        defence = 0;
        health = 0;
        equipDamage = 0;
        string[] setCounter = new string[6];
        for (int i = 0; i < equipmentList.Length; i++) {
            if (equipmentList[i].transform.childCount > 0) {
                ItemData itemData = equipmentList[i].transform.GetChild(0).GetComponent<ItemData>();
                if (itemData.item.Type == "weapon" && itemData.item.active) {
                    damage += itemData.item.Damage;
                    setCounter[i] = itemData.item.Set;
                }
                else if (itemData.item.Type == "equipment" && itemData.item.active) {
                    damage += itemData.item.Attack;
                    defence += itemData.item.Defence;
                    health += itemData.item.Health;
                    equipDamage += itemData.item.Attack;
                    setCounter[i] = itemData.item.Set;
                }
            }

        }
        //volgorde : dumpster, medieval, gentleman, arabic, mexican, chinese, sports,
        for (int i = 0; i < setCounter.Length; i++) {
            if (setCounter[i] == "dumpster") {
                setArray[0] += 1;
            }
            if (setCounter[i] == "medieval") {
                setArray[1] += 1;
            }
            if (setCounter[i] == "gentleman") {
                setArray[2] += 1;
            }
            if (setCounter[i] == "arabic") {
                setArray[3] += 1;
            }
            if (setCounter[i] == "mexican") {
                setArray[4] += 1;
            }
            if (setCounter[i] == "chinese") {
                setArray[5] += 1;
            }
            if (setCounter[i] == "sports") {
                setArray[6] += 1;
            }
        }
        CreateSets();
        persistentInventory.itemDamage = damage;
        persistentInventory.EquipmentDamage = equipDamage;
        persistentInventory.itemDefense = defence;
        persistentInventory.itemHealth = health;
        stats = "Attack: " + damage + "\nDefense: " + defence + "\nHealth from items: " + health + "\nBase health: " + XPManager.xpmanager.Health() + "\nTotal health: " + (XPManager.xpmanager.Health() + health) + "\nCurrent health: " + PlayerPrefsManager.GetPlayerHealth() + "\nCurrent Level: " + XPManager.xpmanager.playerlvl_() + "\nXP to next level: " + XPManager.xpmanager.xptonext_();
        InventoryData.GetComponent<Text>().supportRichText = true;
        InventoryData.GetComponent<Text>().text = stats;
    }

    public void UpdateInventoryMoney() {
        string currency = "CURRENCY: " + persistentInventory.Currency.ToString();
        InventoryMoney.GetComponent<Text>().supportRichText = true;
        InventoryMoney.GetComponent<Text>().text = currency;
    }

    public void CreateSets() {
        string sets = "";
        //volgorde : dumpster, medieval, gentleman, arabic, mexican, chinese, sports,
        //Dumpster set
        sets += "Sets:";
        if (setArray[0] > 1) {
            sets += "\n<color=#000000>Dumpster: " + setArray[0] + " </color>";
            if (setArray[0] == 2) {
                sets += "<color=#FFFFFF>Bonus: 5 Defence</color>";
                defence += 5;
            }
            if (setArray[0] == 3) {
                sets += "<color=#FFFFFF>Bonus: 10 Defence</color>";
                defence += 10;
            }
            if (setArray[0] == 4) {
                sets += "<color=#FFFFFF>Bonus: 10 Defence, 10 Damage</color>";
                defence += 10;
                damage += 10;
                equipDamage += 10;
            }
            if (setArray[0] == 5) {
                sets += "<color=#FFFFFF>Bonus: 20 Defence, 20 Damage</color>";
                defence += 20;
                damage += 20;
                equipDamage += 20;
            }
            if (setArray[0] == 6) {
                sets += "<color=#FFFFFF>Bonus: 50 Defence, 20 Damage</color";
                defence += 50;
                damage += 20;
                equipDamage += 20;
            }
        }
        //Medieval set
        if (setArray[1] > 1) {
            sets += "\n<color=#A3A3A3>Medieval: " + setArray[1] + " </color>";
            if (setArray[1] == 2) {
                sets += "<color=#FFFFFF>Bonus: 10 Defence</color>";
                defence += 10;
            }
            if (setArray[1] == 3) {
                sets += "<color=#FFFFFF>Bonus: 20 Defence, -5 Damage </color>";
                defence += 10;
                damage -= 5;
                equipDamage -= 5;
            }
            if (setArray[1] == 4) {
                sets += "<color=#FFFFFF>Bonus: 30 Defence, 30 Health, -10 Damage</color>";
                defence += 30;
                health += 30;
                damage -= 10;
                equipDamage -= 10;
            }
            if (setArray[1] == 5) {
                sets += "<color=#FFFFFF>Bonus: 50 Defence, 50 Health, -20 Damage</color>";
                defence += 50;
                health += 50;
                damage -= 20;
                equipDamage -= 20;
            }
            if (setArray[1] == 6) {
                sets += "<color=#FFFFFF>Bonus: 100 Defence, 50 Health, -30 Damage</color>";
                defence += 100;
                health += 50;
                damage -= 30;
                equipDamage -= 30;
            }
        }
        //Gentleman
        if (setArray[2] > 1) {
            sets += "\n<color=#C000FF>Gentleman: " + setArray[2] + " </color>";
            if (setArray[2] == 2) {
                sets += "<color=#FFFFFF>Bonus: 5 Damage, 5 Defence, 5 Health</color>";
                defence += 5;
                damage += 5;
                health += 5;
                equipDamage += 5;
            }
            if (setArray[2] == 3) {
                sets += "<color=#FFFFFF>Bonus: 10 Damage, 10 Defence, 10 Health</color>";
                defence += 10;
                damage += 10;
                health += 10;
                equipDamage += 10;
            }
            if (setArray[2] == 4) {
                sets += "<color=#FFFFFF>Bonus: 15 Damage, 15 Defence, 15 Health</color>";
                defence += 15;
                damage += 15;
                health += 15;
                equipDamage += 15;
            }
            if (setArray[2] == 5) {
                sets += "<color=#FFFFFF>Bonus: 20 Damage, 20 Defence, 20 Health</color>";
                defence += 20;
                damage += 20;
                health += 20;
                equipDamage += 20;
            }
            if (setArray[2] == 6) {
                sets += "<color=#FFFFFF>Bonus: 30 Damage, 30 Defence, 30 Health</color>";
                defence += 30;
                damage += 30;
                health += 30;
                equipDamage += 30;
            }
        }
        //Arabic
        if (setArray[3] > 1) {
            sets += "\n<color=#FFF700>Arabic: " + setArray[3] + " </color>";
            if (setArray[3] == 2) {
                sets += "<color=#FFFFFF>Bonus: 5 Damage, 5 Health</color>";
                damage += 15;
                health += 5;
                equipDamage += 15;
            }
            if (setArray[3] == 4) {
                sets += "<color=#FFFFFF>Bonus: 20 Damage, 15 Health, 10 Defence</color>";
                defence += 10;
                damage += 20;
                health += 15;
                equipDamage += 20;
            }
            if (setArray[3] == 5) {
                sets += "<color=#FFFFFF>Bonus: 25 Damage, 20 Health, 15 Defence</color>";
                defence += 15;
                damage += 25;
                health += 20;
                equipDamage += 25;
            }
            if (setArray[3] == 6) {
                sets += "<color=#FFFFFF>Bonus: 30 Damage, 25 Health, 20 Defence</color>";
                defence += 20;
                damage += 30;
                health += 25;
                equipDamage += 30;
            }
        }
        //Mexican
        if (setArray[4] > 1) {
            sets += "\n<color=#00FF26>Mexican: " + setArray[4] + " </color>";
            if (setArray[4] == 2) {
                sets += "<color=#FFFFFF>Bonus: 5 Health</color>";
                health += 5;
            }
            if (setArray[4] == 3) {
                sets += "<color=#FFFFFF>Bonus: 20 Health</color>";
                health += 20;
            }
            if (setArray[4] == 4) {
                sets += "<color=#FFFFFF>Bonus: 40 Health, -10 Damage</color>";
                damage -= 10;
                equipDamage -= 10;
                health += 40;
            }
            if (setArray[4] == 5) {
                sets += "<color=#FFFFFF>Bonus: 60 Health, -10 Damage, -5 Defense</color>";
                defence -= 5;
                damage -= 10;
                health += 60;
                equipDamage -= 10;
            }
            if (setArray[4] == 6) {
                sets += "<color=#FFFFFF>Bonus: 100 Health, -20 Damage, -10 Defense</color>";
                defence -= 10;
                damage -= 20;
                health += 100;
                equipDamage -= 10;
            }
        }
        //Chinese
        if (setArray[5] > 1) {
            sets += "\n<color=#FF0000>Chinese: " + setArray[5] + " </color>";
            if (setArray[5] == 2) {
                sets += "<color=#FFFFFF>Bonus: 10 Damage</color>";
                damage += 10;
                equipDamage += 10;
            }
            if (setArray[5] == 3) {
                sets += "<color=#FFFFFF>Bonus: 20 Damage</color>";
                damage += 20;
                equipDamage += 20;
            }
            if (setArray[5] == 4) {
                sets += "<color=#FFFFFF>Bonus: 40 Damage, -5 Health</color>";
                damage += 40;
                health -= 5;
                equipDamage += 40;
            }
            if (setArray[5] == 5) {
                sets += "<color=#FFFFFF>Bonus: 70 Damage, , -15 Health, -15 Defense</color>";
                defence -= 15;
                damage += 70;
                health -= 15;
                equipDamage += 70;
            }
            if (setArray[5] == 6) {
                sets += "<color=#FFFFFF>Bonus: 70 Damage</color>";
                damage += 70;
                equipDamage += 70;
            }
        }
        //Sports
        if (setArray[6] > 1) {
            sets += "\n<color=#0000FF>Sports: " + setArray[6] + " </color>";
            if (setArray[6] == 2) {
                sets += "\n<color=#FFFFFF>Bonus: 10 Health</color>";
                health += 10;
            }
            if (setArray[6] == 3) {
                sets += "<color=#FFFFFF>Bonus: 20 Health, 5 Damage</color>";
                health += 20;
                damage += 5;
                equipDamage += 5;
            }
            if (setArray[6] == 4) {
                sets += "<color=#FFFFFF>Bonus: 30 Health, 10 Damage</color>";
                damage += 10;
                health += 30;
                equipDamage += 10;
            }
            if (setArray[6] == 5) {
                sets += "<color=#FFFFFF>Bonus: 50 Health, 10 Damage</color>";
                health += 50;
                damage += 10;
                equipDamage += 10;
            }
            if (setArray[6] == 6) {
                sets += "<color=#FFFFFF>Bonus: 80 Health, 10 Damage</color>";
                health += 80;
                damage += 10;
                equipDamage += 10;
            }
        }
        InventorySets.GetComponent<Text>().supportRichText = true;
        InventorySets.GetComponent<Text>().text = sets;
        for (int i = 0; i < setArray.Length; i++) {
            setArray[i] = 0;
        }
    }
}
