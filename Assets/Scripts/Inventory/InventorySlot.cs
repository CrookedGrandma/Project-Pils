using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class InventorySlot : MonoBehaviour , IDropHandler {
    public int slotID;
    private Inventory inv;

    void Start()
    {
        inv = GameObject.Find("InventoryObject").GetComponent<Inventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {

        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
        if (inv.items[slotID].ID == -1)
        {
            droppedItem.item.active = true;
            inv.items[droppedItem.slot] = new Item();
            inv.items[slotID] = droppedItem.item;
            droppedItem.slot = slotID;
            if (slotID >= inv.slotCount)
            {
                setActive(droppedItem.item, slotID);
            }
        }
        else if (droppedItem.slot != slotID)
        {
            setActive(droppedItem.item, slotID);
            Transform item = this.transform.GetChild(0);
            item.GetComponent<ItemData>().slot = droppedItem.slot;
            item.transform.SetParent(inv.slots[droppedItem.slot].transform);
            item.transform.position = inv.slots[droppedItem.slot].transform.position;
            setActive(item.GetComponent<ItemData>().item, droppedItem.slot);
            Debug.Log(item.GetComponent<ItemData>().item.Title);
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;
            inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
            inv.items[slotID] = droppedItem.item;
            droppedItem.slot = slotID;
        }

    }
    private void setActive(Item item, int slot)
    {
        if (slot >= inv.slotCount)
        {
            if (item.Type == "equipment" || item.Type == "weapon")
            {
                if ((item.Subtype == "melee" && slotID == inv.slotCount) || (item.Subtype == "projectile" && slotID == inv.slotCount + 1) || (item.Subtype == "ranged" && slotID == inv.slotCount + 1) || (item.Subtype == "headwear" && slotID == inv.slotCount + 2) || (item.Subtype == "bodywear" && slotID == inv.slotCount + 3) || (item.Subtype == "lowerwear" && slotID == inv.slotCount + 4) || (item.Subtype == "footwear" && slotID == inv.slotCount + 5))
                {
                }
                else
                {
                    item.active = false;
                    Debug.Log("verkeerde slot");
                }
            }
            else
            {
                item.active = false;
                Debug.Log("verkeerde type");
            }
        }
        else
        {
            item.active = true;
        }
    }
}
