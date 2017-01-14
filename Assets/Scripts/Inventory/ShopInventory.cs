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
        AddItem(207);
        AddItem(301);
        AddItem(701);
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
                    Debug.Log("check");
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(shopItem);
                    itemObj.GetComponent<ShopData>().item = itemToAdd;
                    itemObj.GetComponent<ShopData>().slot = i;
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
