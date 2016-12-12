using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class InventorySlot : MonoBehaviour , IDropHandler{
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
            inv.items[droppedItem.slot] = new Item();
            inv.items[slotID] = droppedItem.item;
            droppedItem.slot = slotID;
        }
        else if (droppedItem.slot != slotID)
        {
            Transform item = this.transform.GetChild(0);
            item.GetComponent<ItemData>().slot = droppedItem.slot;
            item.transform.SetParent(inv.slots[droppedItem.slot].transform);
            item.transform.position = inv.slots[droppedItem.slot].transform.position;


            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;

            inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
            inv.items[slotID] = droppedItem.item;

            droppedItem.slot = slotID;
        }
    }
}
