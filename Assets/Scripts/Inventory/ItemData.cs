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
                persistentInventory.removeItem(item.ID, slot);
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
            if(item.Type == "consumable")
            {
                switch (item.ID)
                {
                    case 106:
                        if (!GameManager.instance.questManager.questLog.ContainsKey("Quest006"))
                        {
                            GameManager.instance.questManager.CompleteObjective("Quest005ConsumeMegaPils");
                            GameManager.instance.questManager.AddQuestToLog("Quest006");
                        }
                        break;
                }
                PlayerPrefsManager.SetPlayerHealth((XPManager.xpmanager.Health() + PersistentInventoryScript.instance.itemHealth) + (int)((float)item.Heal / 100 * PlayerPrefsManager.GetPlayerHealth()));
                if (PlayerPrefsManager.GetPlayerHealth() > (XPManager.xpmanager.Health() + PersistentInventoryScript.instance.itemHealth))
                {
                    PlayerPrefsManager.SetPlayerHealth(XPManager.xpmanager.Health() + PersistentInventoryScript.instance.itemHealth);
                }
                inv.RemoveItem(item, this);
                persistentInventory.removeItem(item.ID, slot);
            }

        }
    }
}

