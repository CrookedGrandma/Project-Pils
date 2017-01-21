using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class ShopTooltip : MonoBehaviour
{
    private Item item;
    private string data;
    private GameObject tooltip;
    // In volgorde van 0 tot 6: dumpster, medieval, gentleman, arabic, mexican, chinese, sports
    private int[] setArray = new int[7];

    private void Start()
    {
        tooltip = GameObject.Find("Tooltip");
        tooltip.SetActive(false);
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
    }

    public void Activate(Item item)
    {
        this.item = item;
        ConstructData();
        tooltip.SetActive(true);
        Debug.Log("Activate (ShotTooltip)");
    }

    public void Deactivate(Item item)
    {
        tooltip.SetActive(false);
    }

    public void BuyItem(Item item)
    {
        tooltip.SetActive(true);
        data = "<b><color=#00ff00>You have succesfully bought: \n</color><color=#FFFFFF>" + item.Title + "</color></b>";
        tooltip.transform.GetChild(0).GetComponent<Text>().supportRichText = true;
        tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
    }
    public void TooPoor(Item item)
    {
        data = "<b><color=#ff0000>You Cannot afford: \n<color=#FFFFFF>" + item.Title + "</color>!</color></b>";
        tooltip.transform.GetChild(0).GetComponent<Text>().supportRichText = true;
        tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
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
            data += "\n\n<b><color=#FFFFFFF>Price: €" + item.Value + "</color></b>";
            tooltip.transform.GetChild(0).GetComponent<Text>().supportRichText = true;
            tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
        }
    }
