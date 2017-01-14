using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ShopSlot : MonoBehaviour, IDropHandler
{
    public int slotID;
    private ShopInventory sInv;

    void Start()
    {
        sInv = GameObject.Find("ShopObject").GetComponent<ShopInventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {

        ShopData droppedItem = eventData.pointerDrag.GetComponent<ShopData>();
        if (sInv.items[slotID].ID == -1)
        {
            droppedItem.item.active = true;
            sInv.items[droppedItem.slot] = new Item();
            sInv.items[slotID] = droppedItem.item;
            droppedItem.slot = slotID;
            if (slotID >= sInv.slotCount)
            {
                if (droppedItem.item.Type == "equipment" || droppedItem.item.Type == "weapon")
                {
                    if ((droppedItem.item.Subtype == "melee" && slotID == sInv.slotCount) || (droppedItem.item.Subtype == "projectile" && slotID == sInv.slotCount + 1) || (droppedItem.item.Subtype == "ranged" && slotID == sInv.slotCount + 1) || (droppedItem.item.Subtype == "headwear" && slotID == sInv.slotCount + 2) || (droppedItem.item.Subtype == "bodywear" && slotID == sInv.slotCount + 3) || (droppedItem.item.Subtype == "lowerwear" && slotID == sInv.slotCount + 4) || (droppedItem.item.Subtype == "footwear" && slotID == sInv.slotCount + 5))
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
            item.GetComponent<ShopData>().slot = droppedItem.slot;
            item.transform.SetParent(sInv.slots[droppedItem.slot].transform);
            item.transform.position = sInv.slots[droppedItem.slot].transform.position;
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;
            sInv.items[droppedItem.slot] = item.GetComponent<ShopData>().item;
            sInv.items[slotID] = droppedItem.item;
            droppedItem.slot = slotID;
            Debug.Log(slotID);
        }

    }
}
