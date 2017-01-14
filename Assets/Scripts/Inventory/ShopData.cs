using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ShopData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item item;
    public int amount;
    public int slot;
    private ShopInventory sInv;
    private ShopTooltip shopTooltip;
    private PersistentInventoryScript persistentInventory;
    private Vector2 offset;

    private void Start()
    {
        sInv = GameObject.Find("ShopObject").GetComponent<ShopInventory>();
        persistentInventory = GameObject.Find("PersistentInventory").GetComponent<PersistentInventoryScript>();
        shopTooltip = sInv.GetComponent<ShopTooltip>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        shopTooltip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shopTooltip.Deactivate(item);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (persistentInventory.Currency >= item.Value)
        {

        shopTooltip.BuyItem(item);
        for(int i = 0; i < (persistentInventory.itemList.Length / 2); i++)
        {
            if (persistentInventory.itemList[i,0] == 0)
            {
                    //Checks for sword of deception
                if (item.ID == 207)
                    {
                        persistentInventory.addItem(208, i);
                    }
                else
                    {
                persistentInventory.addItem(item.ID,i);
                    }
                    persistentInventory.Currency -= item.Value;
                    break;
            }
        }
        }
        else
        {
            shopTooltip.TooPoor(item);
        }

    }
}

