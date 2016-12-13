using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

public class ItemDatabase : MonoBehaviour {
    private List<Item> database = new List<Item>();
    private JsonData ItemData;

    private void Start()
    {
        ItemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        ConstructItemDatabase();
    }

    public Item FetchItemById(int ID)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (ID == database[i].ID)
            {
                return database[i];
            }
        }
        return null;

    }

    private void ConstructItemDatabase()
    {
        for (int i = 0; i < ItemData.Count; i++)
        {
            if (ItemData[i]["type"].ToString() == "equipment")
            {
                database.Add(new Item((int)ItemData[i]["id"], ItemData[i]["title"].ToString(), (int)ItemData[i]["value"], ItemData[i]["type"].ToString(), ItemData[i]["subtype"].ToString(), (int)ItemData[i]["stats"]["attack"], (int)ItemData[i]["stats"]["defence"], (int)ItemData[i]["stats"]["health"], ItemData[i]["description"].ToString(), (bool)ItemData[i]["stackable"], (bool)ItemData[i]["quest-related"], (int)ItemData[i]["rarity"], ItemData[i]["slug"].ToString()));
            }
            else if (ItemData[i]["type"].ToString() == "weapon")
            {
                database.Add(new Item((int)ItemData[i]["id"], ItemData[i]["title"].ToString(), (int)ItemData[i]["value"], ItemData[i]["type"].ToString(), ItemData[i]["subtype"].ToString(), (int)ItemData[i]["damage"], ItemData[i]["description"].ToString(), (bool)ItemData[i]["stackable"], (bool)ItemData[i]["quest-related"], (int)ItemData[i]["rarity"], ItemData[i]["slug"].ToString()));
            }
            else if(ItemData[i]["type"].ToString() == "consumable")
            {
                database.Add(new Item((int)ItemData[i]["id"], ItemData[i]["title"].ToString(), (int)ItemData[i]["value"], ItemData[i]["type"].ToString(), ItemData[i]["subtype"].ToString(), (int)ItemData[i]["heal"], ItemData[i]["description"].ToString(), (bool)ItemData[i]["stackable"], (bool)ItemData[i]["quest-related"], (int)ItemData[i]["rarity"], ItemData[i]["slug"].ToString()));
            }
            else if (ItemData[i]["type"].ToString() == "miscellaneous")
            {
                database.Add(new Item((int)ItemData[i]["id"], ItemData[i]["title"].ToString(), (int)ItemData[i]["value"], ItemData[i]["type"].ToString(), ItemData[i]["description"].ToString(), (bool)ItemData[i]["stackable"], (bool)ItemData[i]["quest-related"], (int)ItemData[i]["rarity"], ItemData[i]["slug"].ToString()));
            }
            else if (ItemData[i]["type"].ToString() == "nullitem")
            {
                database.Add(new Item((int)ItemData[i]["id"], ItemData[i]["title"].ToString(),  ItemData[i]["type"].ToString()));
            }
        }
    }
}

public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public string Type { get; set; }
    public string Subtype { get; set; }
    public int Attack { get; set; }
    public int Defence { get; set; }
    public int Health { get; set; }
    public int Heal { get; set; }
    public int Damage { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public bool Questrelated { get; set; }
    public int Rarity { get; set; }
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }

    //Constructor voor Equipment
    public Item(int id, string title, int value, string type, string subtype, int attack, int defence, int health, string description, bool stackable, bool questrelated, int rarity, string slug)
    {
        this.ID = id;
        this.Title = title;
        this.Value = value;
        this.Type = type;
        this.Subtype = subtype;
        this.Attack = attack;
        this.Defence = defence;
        this.Health = health;
        this.Description = description;
        this.Stackable = stackable;
        this.Questrelated = questrelated;
        this.Rarity = rarity;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("Sprites/Inventory/Sprites/" + slug);
    }
    //Constructor voor Consumables/Weapons
    public Item(int id, string title, int value, string type, string subtype, int healdamage, string description, bool stackable, bool questrelated, int rarity, string slug)
    {
        this.ID = id;
        this.Title = title;
        this.Value = value;
        this.Type = type;
        this.Subtype = subtype;
        if(type == "consumable")this.Heal = healdamage;
        else if(type == "weapon") this.Damage = healdamage;
        this.Description = description;
        this.Stackable = stackable;
        this.Questrelated = questrelated;
        this.Rarity = rarity;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("Sprites/Inventory/Sprites/" + slug);
    }

    //Constructor voor Miscellaneous
    public Item(int id, string title, int value, string type, string description, bool stackable, bool questrelated, int rarity, string slug)
    {
        this.ID = id;
        this.Title = title;
        this.Value = value;
        this.Type = type;
        this.Description = description;
        this.Stackable = stackable;
        this.Questrelated = questrelated;
        this.Rarity = rarity;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("Sprites/Inventory/Sprites/" + slug);
    }
    //Constructor voor nullitem
    public Item(int id, string title, string type)
    {
        this.ID = id;
        this.Title = title;
        this.Type = type;
    }

    public Item()
    {
        this.ID = -1;
    }
}
