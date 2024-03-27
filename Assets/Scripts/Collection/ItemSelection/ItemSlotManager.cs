using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
public class ItemSlotManager : MonoBehaviour, IDropHandler
{
    public int slotID;

    public CollectionManager manager;

    public ItemType slotItemType;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        ItemSlot slot = dropped.GetComponent<ItemSlot>();

        if (slot == null) { return; }

        manager.EndDrag(dropped);

        if (slot.itemType != slotItemType) { return; } // if slot being dropped item type is not the same as type (mat/cata) of this slot manager then return

        if (slot.slotType == ItemSlotType.EquipSlot) // if dragging from equipSlot to storage slot
        {
            int realSlot = slotID + (manager.currentBag * manager.currentAmountOfCollectionSlots);
            TeamEquipSlot equipSlot = slot.GetComponent<TeamEquipSlot>();

            manager.AddItemToStorageWithID(slot.item, slot.amount, realSlot);
            manager.RemoveItemFromMonsterInParty(equipSlot.pManager.slotNum - 1, equipSlot.tManager.slotNum + 1);

            //manager.RemoveItemFromEquipMon(); WORK HERE
        }
        else if (slot.slotType == ItemSlotType.StorageSlot) // if dragging from storage slot to another storage slot
        {
            int realSlot = slotID + (manager.currentBag * manager.currentAmountOfCollectionSlots);

            manager.RemoveItemFromStorage(slot.item, slot.amount);

            manager.AddItemToStorageWithID(slot.item, slot.amount, realSlot);


        }

        manager.UpdateCollectionAll();
    }
}
