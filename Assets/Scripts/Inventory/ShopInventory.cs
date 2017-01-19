using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class ShopInventory : MonoBehaviour {
    GameObject shopPanel;
    GameObject slotPanel;
    PersistentInventoryScript PersistentInventory;
    public GameObject tooltip;
    ItemDatabase database;
    public GameObject shopSlot;
    public GameObject shopItem;
    public GameObject shopMoney;
    public int slotCount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    void Start () {
        PersistentInventory = GameObject.Find("PersistentInventory").GetComponent<PersistentInventoryScript>();
        PersistentInventory.InShop = true;
        Debug.Log("shopinventory name: " + PersistentInventory.shopType);
        slotCount = PersistentInventory.slotCount;
        database = GetComponent<ItemDatabase>();
        shopPanel = GameObject.Find("ShopPanel");
        slotPanel = shopPanel.transform.FindChild("SlotPanel").gameObject;
        for (int i = 0; i < slotCount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(shopSlot));
            slots[i].GetComponent<ShopSlot>().slotID = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }
        switch (PersistentInventory.shopType)
        {
            case "Bartender":
                AddItem(100);
                AddItem(102);
                AddItem(103);
                AddItem(104);
                AddItem(106);
                break;
            case "Waiter":
                AddItem(100);
                AddItem(101);
                AddItem(203);
                AddItem(205);
                AddItem(301);
                AddItem(303);
                AddItem(608);
                break;
            case "Chinese Man":
                AddItem(210);
                AddItem(308);
                AddItem(601);
                AddItem(708);
                AddItem(805);
                AddItem(906);
                break;
            case "Hobo":
                AddItem(200);
                AddItem(204);
                AddItem(300);
                AddItem(500);
                AddItem(606);
                AddItem(707);
                AddItem(806);
                AddItem(905);
                break;
            case "Dress Store":
                AddItem(603);
                AddItem(604);
                AddItem(607);
                AddItem(609);
                AddItem(610);
                AddItem(702);
                AddItem(703);
                AddItem(705);
                AddItem(709);
                AddItem(800);
                AddItem(802);
                AddItem(900);
                AddItem(902);
                AddItem(907);
                break;
            case "Clothing Store":
                AddItem(601);
                AddItem(602);
                AddItem(701);
                AddItem(702);
                AddItem(704);
                AddItem(709);
                AddItem(801);
                AddItem(804);
                AddItem(807);
                AddItem(808);
                AddItem(809);
                AddItem(901);
                AddItem(904);
                AddItem(908);
                break;
            case "Goods Store":
                AddItem(201);
                AddItem(202);
                AddItem(206);
                AddItem(212);
                AddItem(302);
                AddItem(304);
                AddItem(307);
                AddItem(400);
                AddItem(401);
                AddItem(402);
                AddItem(403);
                AddItem(501);
                AddItem(502);
                AddItem(503);
                AddItem(909);
                break;
            case "Antique Store":
                AddItem(207);
                AddItem(209);
                AddItem(211);
                AddItem(404);
                AddItem(504);
                AddItem(604);
                AddItem(605);
                AddItem(702);
                AddItem(706);
                AddItem(800);
                AddItem(803);
                AddItem(900);
                AddItem(903);
                break;




        }

    }

    void Update()
    {
        UpdateShopMoney();
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
                    ShopData data = slots[i].transform.GetChild(0).GetComponent<ShopData>();
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
                    Debug.Log("check");
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(shopItem);
                    itemObj.GetComponent<ShopData>().item = itemToAdd;
                    itemObj.GetComponent<ShopData>().slot = i;
                    itemObj.GetComponent<ShopData>().amount = 1;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObj.name = itemToAdd.Title;
                    break;
                }
            }
        }
    }

    public void RemoveItem(Item item, ShopData shopData)
    {
        if (item.Stackable)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == item.ID)
                {
                    ShopData data = slots[i].transform.GetChild(0).GetComponent<ShopData>();
                    if (data.amount <= 1)
                    {
                        Destroy(shopData.gameObject);
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
                    Destroy(shopData.gameObject);
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

    public void UpdateShopMoney()
    {
        string currency = "CURRENCY: " + PersistentInventory.Currency.ToString();
        shopMoney.GetComponent<Text>().supportRichText = true;
        shopMoney.GetComponent<Text>().text = currency;
    }
}
