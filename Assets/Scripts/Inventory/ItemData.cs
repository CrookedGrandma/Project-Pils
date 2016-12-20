using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item item;
    public int amount;
    public int slot;
    private Inventory inv;
    private Tooltip tooltip;
    private Vector2 offset;

    private void Start()
    {
        inv = GameObject.Find("InventoryObject").GetComponent<Inventory>();
        tooltip = inv.GetComponent<Tooltip>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            this.transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(inv.slots[slot].transform);
        this.transform.position = inv.slots[slot].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate(item);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (slot < inv.slotCount)
        {
            if (item.Type == "equipment" || item.Type == "weapon")
            {
                offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
                this.transform.SetParent(this.transform.parent.parent);
                this.transform.position = eventData.position - offset;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                if (item.Subtype == "melee")
                {
                    this.transform.SetParent(inv.slots[inv.slotCount].transform);
                    this.transform.position = inv.slots[inv.slotCount].transform.position;
                    slot = inv.slotCount;
                }
                if (item.Subtype == "ranged" || item.Subtype == "projectile")
                {
                    this.transform.SetParent(inv.slots[inv.slotCount + 1].transform);
                    this.transform.position = inv.slots[inv.slotCount + 1].transform.position;
                    slot = inv.slotCount + 1;
                }
                if (item.Subtype == "headwear")
                {
                    this.transform.SetParent(inv.slots[inv.slotCount + 2].transform);
                    this.transform.position = inv.slots[inv.slotCount + 2].transform.position;
                    slot = inv.slotCount + 2;
                }
                if (item.Subtype == "bodywear")
                {
                    this.transform.SetParent(inv.slots[inv.slotCount + 3].transform);
                    this.transform.position = inv.slots[inv.slotCount + 3].transform.position;
                    slot = inv.slotCount + 3;
                }
                if (item.Subtype == "lowerwear")
                {
                    this.transform.SetParent(inv.slots[inv.slotCount + 4].transform);
                    this.transform.position = inv.slots[inv.slotCount + 4].transform.position;
                    slot = inv.slotCount + 4;
                }
                if (item.Subtype == "footwear")
                {
                    this.transform.SetParent(inv.slots[inv.slotCount + 5].transform);
                    this.transform.position = inv.slots[inv.slotCount + 5].transform.position;
                    slot = inv.slotCount + 5;
                }
                Transform oldItem = this.transform.GetChild(0);
                oldItem.GetComponent<ItemData>().slot = droppedItem.slot;
                oldItem.transform.SetParent(inv.slots[droppedItem.slot].transform);
                oldItem.transform.position = inv.slots[droppedItem.slot].transform.position;

                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            Debug.Log(slot);
            /*if (slot >= inv.slotCount)
            {
                Debug.Log("hoihoi");
                offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
                this.transform.SetParent(this.transform.parent.parent);
                this.transform.position = eventData.position - offset;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                for (int i = 0; i < inv.items.Count; i++)
                {
                    if (inv.items[i].ID == -1)
                    {
                        this.transform.SetParent(inv.slots[i].transform);
                        this.transform.position = inv.slots[i].transform.position;
                    }
                }
            }*/
        }
    }
}

