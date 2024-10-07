using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class EquipmentItemSlot : ItemSlot
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;

    public int slotNum;

    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.25f;
    }

    public void Init(StoredItem itm, GameManager man)
    {
        slotType = ItemSlotType.StorageSlot;
        itemType = itm.item.type;
        slotNum = itm.storedID;
        manager = man;

        item = itm.item;

        if (nameText != null)
        {
            nameText.text = itm.item.itemName;
        }

        
        amountText.text = itm.amount.ToString();
        amount = itm.amount;
        icon.sprite = itm.item.icon;
    }


    public override void OnClick()
    {
        manager.OpenItemInspectTooltip(item);
    }
    /*
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        ItemSlot slot = dropped.GetComponent<ItemSlot>();

        manager.collectionManager.EndDrag(this.gameObject);

        if (slot.itemType == ItemType.Catalyst)
        {
            int slotFrom = slot.GetComponent<EquipmentItemSlot>().slotNum;


            manager.collectionManager.RemoveItemFromStorage(slot.item);
            manager.collectionManager.AddItemToStorageWithID(slot.item, slot.amount, slotNum);

            manager.collectionManager.RemoveItemFromStorage(item);
            manager.collectionManager.AddItemToStorageWithID(item, amount, slotFrom);
        }


        manager.collectionManager.UpdateCollectionAll();
    }
    */

}
