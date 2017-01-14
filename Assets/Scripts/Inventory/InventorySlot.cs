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
                if (droppedItem.item.Type == "equipment" || droppedItem.item.Type == "weapon")
                {
                    if ((droppedItem.item.Subtype == "melee" && slotID == inv.slotCount) || (droppedItem.item.Subtype == "projectile" && slotID == inv.slotCount + 1) || (droppedItem.item.Subtype == "ranged" && slotID == inv.slotCount + 1) || (droppedItem.item.Subtype == "headwear" && slotID == inv.slotCount + 2) || (droppedItem.item.Subtype == "bodywear" && slotID == inv.slotCount + 3) || (droppedItem.item.Subtype == "lowerwear" && slotID == inv.slotCount + 4) || (droppedItem.item.Subtype == "footwear" && slotID == inv.slotCount + 5))
                    {
                    }
                    else
                    {       
                        droppedItem.item.active = false;
                    }
                }
                else droppedItem.item.active = false; Debug.Log("hoi2");
            }
        }
        else if (droppedItem.slot != slotID)
        {
            droppedItem.item.active = true;
            Transform item = this.transform.GetChild(0);
            item.GetComponent<ItemData>().slot = droppedItem.slot;
            item.transform.SetParent(inv.slots[droppedItem.slot].transform);
            item.transform.position = inv.slots[droppedItem.slot].transform.position;
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;
            inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
            inv.items[slotID] = droppedItem.item;
            droppedItem.slot = slotID;
            Debug.Log(slotID);
        }

    }
}
