using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TeamEquipSlot : ItemSlot, IDropHandler
{
    public TextMeshProUGUI nameText;

    [HideInInspector]public PartySlotManager pManager;
    [HideInInspector]public TeamEquipSlotManager tManager;

    public int slotNum;

    public bool simple = false;



    public void Init(MonsterItemSO itm, GameManager GM, PartySlotManager p, TeamEquipSlotManager t, int sNum)
    {
        slotType = ItemSlotType.EquipSlot;
        itemType = itm.type;
        manager = GM;
        pManager = p;
        tManager = t;
        item = itm;

        nameText.text = itm.itemName;
        amount = 1;
        icon.sprite = itm.icon;

        slotNum = sNum;

        simple = false;
    }
    public void Init(MonsterItemSO itm, GameManager GM)
    {
        slotType = ItemSlotType.EquipSlot;
        itemType = itm.type;
        item = itm;
        manager = GM;
        nameText.text = itm.itemName;
        amount = 1;
        icon.sprite = itm.icon;

        simple = true;
    }

    public override void OnClick()
    {
        manager.OpenItemInspectTooltipEquipped(item, pManager.slotNum - 1, slotNum);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // THIS ALL WORKS BUT LEAVING COMMENTED OUT BECAUSE I WANT TO RE DO THE EQUIPING SYSTEM, IT SUCKS


        /*
        GameObject draggingItem = eventData.pointerDrag;
        ItemSlot draggingItemSlot = draggingItem.GetComponent<ItemSlot>();

        if (draggingItemSlot == null) { return; }

        if (draggingItemSlot.itemType == ItemType.Catalyst) // is cata, and mode is unlocked
        {
            if (draggingItemSlot.slotType == ItemSlotType.StorageSlot) // Drag from storage to this other item to swap them, swap equiped item to storage, and storage item to equip
            {
                int thisSlotNum = pManager.slotNum - 1;
                EquipmentItemSlot draggedEquipSlot = draggingItemSlot.GetComponent<EquipmentItemSlot>();
                //Remove dragged Item From Storage
                manager.collectionManager.RemoveItemFromStorage(draggingItemSlot.item, 1);
                //Add Item on equip slot to storage
                manager.collectionManager.AddItemToStorageWithID(item, 1, draggedEquipSlot.slotNum);
                //Remove equip item from beast
                manager.collectionManager.RemoveItemFromMonsterInParty(pManager.slotNum - 1, slotNum + 1); // remove item from held mon slot
                //Add New equip item to beast from dragged item
                manager.collectionManager.AddItemToMonsterInParty(draggingItemSlot.item, thisSlotNum, slotNum + 1);
            }
            else if (draggingItemSlot.slotType == ItemSlotType.EquipSlot) // Drag from equipment slot to this other equipment slot, swap the equipment on each beast
            {
                int thisSlotNum = pManager.slotNum - 1;
                EquipmentItemSlot draggedEquipSlot = draggingItemSlot.GetComponent<EquipmentItemSlot>();
                TeamEquipSlot equipSlot = draggingItemSlot.GetComponent<TeamEquipSlot>();
                //Remove dragged Item From dragged Equip slot
                manager.collectionManager.RemoveItemFromMonsterInParty(equipSlot.pManager.slotNum - 1, equipSlot.slotNum + 1);
                //Add Item on dragged Equip Slot
                manager.collectionManager.AddItemToMonsterInParty(item, equipSlot.pManager.slotNum - 1, equipSlot.slotNum + 1);
                //Remove equip item from beast
                manager.collectionManager.RemoveItemFromMonsterInParty(pManager.slotNum - 1, slotNum + 1); // remove item from held mon slot
                //Add New equip item to beast from dragged item
                manager.collectionManager.AddItemToMonsterInParty(draggingItemSlot.item, thisSlotNum, slotNum + 1);
            }
        }

        manager.collectionManager.UpdateCollectionAll();
        */


        throw new System.NotImplementedException();
    }
}
