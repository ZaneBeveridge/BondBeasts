using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class StorageSlotManager : MonoBehaviour, IDropHandler
{
    public TextMeshProUGUI text;
    public Image image;

    public Color selectedColour;
    public Color unselectedColour;

    public Color textSelectedColour;
    public Color textUnselectedColour;

    public StorageSlotType type;

    public int storageSlotID = 0;

    [HideInInspector]public CollectionManager manager;

    


    public void Init(int num, int storageID, CollectionManager man)
    {
        storageSlotID = storageID;
        text.text = num.ToString();
        manager = man;
    }

    public void Select(bool state)
    {
        if (state)
        {
            image.color = selectedColour;
            text.color = textSelectedColour;
        }
        else
        {
            image.color = unselectedColour;
            text.color = textUnselectedColour;
        }
    }

    public void Press()
    {
        if (type == StorageSlotType.Bag)
        {
            manager.PressButtonBag(storageSlotID);
        }
        else if (type == StorageSlotType.Folder)
        {
            manager.PressButtonFolder(storageSlotID);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        int trueBagID = storageSlotID + (manager.selectedFolderTemp * manager.currentAmountOfCollectionSlots);

        BagSpace bagCheck = manager.CheckSpaceInBag(trueBagID);

        if (bagCheck.state == false) { return; }

        if (dropped.TryGetComponent<Slot>(out Slot slot)) // collection slot
        {
            if (slot.type == SlotType.Party && type == StorageSlotType.Bag && manager.bagMode == 0) // dragged from party slot into bag slot
            {
                PartySlot pSlot = dropped.GetComponent<PartySlot>();
                manager.ClearMonsterFromParty(dropped, pSlot.partySlotManager.slotNum - 1); // remove dragged mon from party

                manager.SpawnMonsterInCollectionWithBag(slot.storedMonster, trueBagID); // place new mon in dropped bag / folder
            }
            else if (slot.type == SlotType.Collection && type == StorageSlotType.Bag && manager.bagMode == 0)
            {
                manager.ClearMonster(slot.storedMonster); // remove dragged mon from collection

                manager.SpawnMonsterInCollectionWithBag(slot.storedMonster, trueBagID); // place new mon in dropped bag / folder
            }

        }
        else if (dropped.TryGetComponent<ItemSlot>(out ItemSlot itemSlot)) //itemSlot
        {
            if (type == StorageSlotType.Bag && manager.bagMode == 1 || manager.bagMode == 2) // dragged from item mat slot into bag slot
            {
                manager.RemoveItemFromStorage(itemSlot.item, itemSlot.amount);

                manager.AddItemToStorageWithBag(itemSlot.item, itemSlot.amount, trueBagID);
            }
        }


        manager.UpdateCollectionAll();

    }
}

public enum StorageSlotType
{
    Bag,
    Folder
}
