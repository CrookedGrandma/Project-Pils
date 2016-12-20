using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class Tooltip : MonoBehaviour
{
    private Item item;
    private string data;
    private string stats;
    private int damage;
    private int defence;
    private int health;
    private GameObject tooltip;
    public GameObject weaponslot;
    public GameObject ammoslot;
    public GameObject headslot;
    public GameObject bodyslot;
    public GameObject lowerslot;
    public GameObject shoeslot;
    public GameObject InventoryData;
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
        UpdateInventoryData();
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
    public void Activate(Item item)
    {
        this.item = item;
        ConstructData();
        tooltip.SetActive(true);
    }

    public void Deactivate(Item item)
    {
        tooltip.SetActive(false);
    }

    public void ConstructData()
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
            data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>" + item.Type + "\n" + item.Subtype + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>\n\n<color=#C000FF>Heal: " + item.Heal + "</color>";
        }
        else if (item.Type == "weapon")
        {
<<<<<<< HEAD
            data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>" + item.Type + "\n" + item.Subtype + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>\n\n<color=#C000FF>Damage: " + item.Damage + "</color>";
            if (item.Set != "none")
            {
                data += "\n\n<i><color=#FFF000>This item is part of the <color=#FFFFFF>" + item.Set + "</color> set </color></i>";
=======
            if (item.ID == 14) {
                data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>" + item.Type + "\n" + item.Subtype + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>\n\n<color=#C000FF>Damage: ???</color>";
            }
            else {
                data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>" + item.Type + "\n" + item.Subtype + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>\n\n<color=#C000FF>Damage: " + item.Damage + "</color>";
>>>>>>> f5004450fde485c9920ca8504e7406cc9b2db07e
            }
        }
        else if (item.Type == "miscellaneous" || item.Type == "ammunition")
        {
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
        //Toont informatie over de set

        data += "\n\n<color=#FFFFFF>Current Sets: </color>";
        if (setArray[0] >= 2)
        {
            data += "\n<color=#000000>Dumpster: " + setArray[0] + "</color>";
        }
        if (setArray[1] >= 2)
        {
            data += "\n<color=#A3A3A3>Medieval: " + setArray[1] + "</color>";
        }
        if (setArray[2] >= 2)
        {
            data += "\n<color=#C000FF>Gentleman: " + setArray[2] + "</color>";
        }
        if (setArray[3] >= 2)
        {
            data += "\n<color=#FFF700>Arabic: " + setArray[3] + "</color>";
        }
        if (setArray[4] >= 2)
        {
            data += "\n<color=#00FF26>Mexican: " + setArray[4] + "</color>";
        }
        if (setArray[5] >= 2)
        {
            data += "\n<color=#FF0000>Chinese: " + setArray[5] + "</color>";
        }
        if (setArray[6] >= 2)
        {
            data += "\n<color=#0000ff>Sports: " + setArray[6] + "</color>";
        }
        tooltip.transform.GetChild(0).GetComponent<Text>().supportRichText = true;
        tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
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
        stats = "Attack: " + damage + "\nDefense: " + defence + "\nHealth: " + health;
        InventoryData.GetComponent<Text>().supportRichText = true;
        InventoryData.GetComponent<Text>().text = stats;
    }
}
