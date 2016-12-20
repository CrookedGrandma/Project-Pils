using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class Tooltip : MonoBehaviour {
    private Item item;
    private string data;
    private GameObject tooltip;

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
        }
        else if (item.Type == "consumable")
        {
            data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>"+ item.Type + "\n" + item.Subtype + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>\n\n<color=#C000FF>Heal: " + item.Heal + "</color>";
        }
        else if (item.Type == "weapon")
        {
            if (item.ID == 14) {
                data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>" + item.Type + "\n" + item.Subtype + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>\n\n<color=#C000FF>Damage: ???</color>";
            }
            else {
                data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>" + item.Type + "\n" + item.Subtype + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>\n\n<color=#C000FF>Damage: " + item.Damage + "</color>";
            }
        }
        else if (item.Type == "miscellaneous")
        {
            data = "<color=#00bfff><b>" + item.Title + "</b></color>\n\n<i><color=#FFFFFF>" + item.Type + "</color></i>\n\n<color=#40ff00>" + item.Description + "</color>";
        }
        if (item.Questrelated == true)
        {
            Debug.Log("ik trigger");
            data += "\n\n<i><color=#FFFFFF>Quest-related, not sellable</color></i>";
        }
        tooltip.transform.GetChild(0).GetComponent<Text>().supportRichText = true;
        tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
    }
	
	}
