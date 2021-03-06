﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class Tooltip : MonoBehaviour
{
    private Item item;
    private string data;
    private GameObject tooltip;
    public GameObject weaponslot;
    public GameObject ammoslot;
    public GameObject headslot;
    public GameObject bodyslot;
    public GameObject lowerslot;
    public GameObject shoeslot;
    // In volgorde van 0 tot 5: dumpster, medieval, gentleman, arabic, mexican, chinese, sports
    private int[] setArray = new int[7];
    private GameObject[] equipmentList;

    private void Start()
    {
        tooltip = GameObject.Find("Tooltip");
        tooltip.SetActive(false);
        equipmentList = new GameObject[] { weaponslot, ammoslot, headslot, bodyslot, lowerslot, shoeslot };

    }

    private void Update()
    {
        if (tooltip.activeSelf)
        {
            tooltip.transform.position = Input.mousePosition;
        }
        for (int x = 0; x < setArray.Length; x++)
        {
            setArray[x] = 0;
        }
        for (int i = 0; i < equipmentList.Length; i++)
        {
            if (equipmentList[i].transform.childCount > 0)
            {
                ItemData itemData = equipmentList[i].transform.GetChild(0).GetComponent<ItemData>();
                if (itemData.item.Set == "dumpster")
                {
                    setArray[0] += 1;
                }
                if (itemData.item.Set == "medieval")
                {
                    setArray[1] += 1;
                }
                if (itemData.item.Set == "gentleman")
                {
                    setArray[2] += 1;
                }
                if (itemData.item.Set == "arabic")
                {
                    setArray[3] += 1;
                }
                if (itemData.item.Set == "mexican")
                {
                    setArray[4] += 1;
                }
                if (itemData.item.Set == "chinese")
                {
                    setArray[5] += 1;
                }
                if (itemData.item.Set == "sports")
                {
                    setArray[6] += 1;
                }
            }
        }
    }
    //Activates the tooltip
    public void Activate(Item item)
    {
        this.item = item;
        ConstructData();
        tooltip.SetActive(true);
    }
    //Gets called when you want to sell an item in your inventory
    public void sellActivate(Item item, bool sellConfirm)
    {
        this.item = item;
        if (item.Sellable)
        {
            if (sellConfirm == false)
            {
                data = "<color=#0000ff>Are you sure you want to sell: <b><color=#ffffff>" + item.Title + "</color></b>?</color>";
            }
            else if (sellConfirm == true)
            {
                data = "<color=#00ff00>You have successfully sold: <b><color=#ffffff>" + item.Title + "</color></b>!</color>";
            }
        }
        else if (!item.Sellable)
        {
            data = "<color=#ff0000><b>You cannot sell this item!</b></color>";
        }
        tooltip.transform.GetChild(0).GetComponent<Text>().supportRichText = true;
        tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
    }
    //Deactivate the tooltip
    public void Deactivate(Item item)
    {
        tooltip.SetActive(false);
    }

    public void ConstructData()
    {
        if (item.active == true)
        {
            if (item.Type == "equipment")
            {
                data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>" + item.Type + "\n" + item.Subtype + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>\n\n<color=#C000FF>Attack: " + item.Attack + "\nDefence: " + item.Defence + "\nHealth: " + item.Health + "</color>";
                if (item.Set != "none")
                {
                    data += "\n\n<i><color=#FFF000>This item is part of the <color=#FFFFFF>" + item.Set + "</color> set </color></i>";
                }
            }
            else if (item.Type == "consumable")
            {
                data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>" + item.Type + "\n" + item.Subtype + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>\n\n<color=#C000FF>Heal: " + item.Heal + "%</color>";
            }
            else if (item.Type == "weapon")
            {
                data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>" + item.Type + "\n" + item.Subtype + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>\n\n<color=#C000FF>Damage: " + item.Damage + "</color>";
                if (item.Set != "none")
                {
                    data += "\n\n<i><color=#FFF000>This item is part of the <color=#FFFFFF>" + item.Set + "</color> set </color></i>";
                    if (item.ID == 14)
                    {
                        data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>" + item.Type + "\n" + item.Subtype + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>\n\n<color=#C000FF>Damage: ???</color>";
                    }
                    else
                    {
                        data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>" + item.Type + "\n" + item.Subtype + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>\n\n<color=#C000FF>Damage: " + item.Damage + "</color>";
                    }

                }
            }
            else if (item.Type == "miscellaneous" || item.Type == "ammunition")
            {
                Debug.Log(item.Title);
                data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>" + item.Type + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>";
            }
                if (item.Questrelated == true)
                {
                    data += "\n\n<i><color=#FFFFFF>Quest-related</color></i>";
                }
                if (item.Sellable == false)
                {
                    data += "\n\n<i><color=#FFFFFF>Not Sellable</color></i>";
                }
                tooltip.transform.GetChild(0).GetComponent<Text>().supportRichText = true;
                tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
            }
        else if (item.active == false)
        {
            data = "<color=#FF0000>THIS ITEM IS NOT ACTIVE, PLEASE MOVE IT TO THE PROPER SLOT</color>";
            tooltip.transform.GetChild(0).GetComponent<Text>().supportRichText = true;
            tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
        }
    }
}
