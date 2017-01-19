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
    private PersistentInventoryScript persistentInventory;
    GameObject persistentInventoryObject;
    private bool sellConfirm;
    private bool? sold;
    private void Start()
    {
        persistentInventoryObject = GameObject.Find("PersistentInventory");
        inv = GameObject.Find("InventoryObject").GetComponent<Inventory>();
        persistentInventory = persistentInventoryObject.GetComponent<PersistentInventoryScript>();
        tooltip = inv.GetComponent<Tooltip>();
    }
    private void Update()
    {
        if (sold == true)
        {
            inv.RemoveItem(item, this);
            sold = null;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log(slot);
        Debug.Log(inv.slotCount);
        if (item != null)
        {
            for (int x = 0; x < amount; x++)
            {
                if (slot < inv.slotCount)
                {
                    persistentInventory.removeItem(item.ID, slot);
                }
                else if (slot >= inv.slotCount)
                {
                    Debug.Log("..");
                    persistentInventory.removeEquipment(slot - inv.slotCount, amount);
                }

            }
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
            for (int x = 0; x < amount; x++)
            {
            if (slot < inv.slotCount)
            {
                persistentInventory.addItem(item.ID, slot);
            }
            else
            {
                persistentInventory.addEquipment(item.ID ,amount, slot - inv.slotCount);
            }
        }
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
        //Verkoopt het item als speler in shop zit
        if (persistentInventory.InShop == true)
        {
            if (slot < inv.slotCount) {
            if (!sold.HasValue)
            {
                sold = false;
            }
            if (!sellConfirm)
            {
                tooltip.sellActivate(item, sellConfirm);
                if (item.Sellable)
                {
                    sellConfirm = true;
                }
            }
            else if (sellConfirm)
            {
                tooltip.sellActivate(item, sellConfirm);
                System.Threading.Timer timer = null;
                timer = new System.Threading.Timer((obj) =>
                {
                    if (sold == false)
                    {
                        persistentInventory.Currency += item.Value;
                    }
                    sold = true;
                    timer.Dispose();
                },
                            null, 1000, System.Threading.Timeout.Infinite);
            }

        }
    }
        else if (slot < inv.slotCount)
        {
            if (item.Type == "equipment" || item.Type == "weapon")
            {
                for (int x = 0; x < amount; x++)
                {
                    persistentInventory.removeItem(item.ID, slot);
                }
                offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
                this.transform.SetParent(this.transform.parent.parent);
                this.transform.position = eventData.position - offset;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                if (item.Subtype == "melee")
                {
                    this.transform.SetParent(inv.slots[inv.slotCount].transform);
                    this.transform.position = inv.slots[inv.slotCount].transform.position;
                    slot = inv.slotCount;
                    persistentInventory.addEquipment(item.ID, amount, 0);
                }
                if (item.Subtype == "ranged" || item.Subtype == "projectile")
                {
                    this.transform.SetParent(inv.slots[inv.slotCount + 1].transform);
                    this.transform.position = inv.slots[inv.slotCount + 1].transform.position;
                    slot = inv.slotCount + 1;
                    {
                        persistentInventory.addEquipment(item.ID, amount, 1);
                    }
                }
                if (item.Subtype == "headwear")
                {
                    this.transform.SetParent(inv.slots[inv.slotCount + 2].transform);
                    this.transform.position = inv.slots[inv.slotCount + 2].transform.position;
                    slot = inv.slotCount + 2;
                    persistentInventory.addEquipment(item.ID, amount, 2);
                }
                if (item.Subtype == "bodywear")
                {
                    this.transform.SetParent(inv.slots[inv.slotCount + 3].transform);
                    this.transform.position = inv.slots[inv.slotCount + 3].transform.position;
                    slot = inv.slotCount + 3;
                    persistentInventory.addEquipment(item.ID, amount, 3);
                }
                if (item.Subtype == "lowerwear")
                {
                    this.transform.SetParent(inv.slots[inv.slotCount + 4].transform);
                    this.transform.position = inv.slots[inv.slotCount + 4].transform.position;
                    slot = inv.slotCount + 4;
                    persistentInventory.addEquipment(item.ID, amount, 4);
                }
                if (item.Subtype == "footwear")
                {
                    this.transform.SetParent(inv.slots[inv.slotCount + 5].transform);
                    this.transform.position = inv.slots[inv.slotCount + 5].transform.position;
                    slot = inv.slotCount + 5;
                    persistentInventory.addEquipment(item.ID, amount, 5);
                }
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
    }
}

