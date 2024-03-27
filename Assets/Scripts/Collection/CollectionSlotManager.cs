using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectionSlotManager : MonoBehaviour, IDropHandler
{
    public int slotID;


    public CollectionManager manager;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Slot slot = dropped.GetComponent<Slot>();

        if (slot == null) { return; }

        manager.EndDrag(dropped);

        if (slot.type == SlotType.Party)
        {
            int realSlot = slotID + ((manager.currentBag * 10) + 3);


            manager.SpawnMonsterInCollectionWithID(slot.storedMonster, realSlot);

            manager.ClearMonsterFromParty(dropped, dropped.GetComponent<PartySlot>().partySlotManager.slotNum - 1);
        }
        else if (slot.type == SlotType.Collection)
        {
            int realSlot = slotID + ((manager.currentBag * 10) + 3);

            manager.ClearMonster(slot.storedMonster);

            manager.SpawnMonsterInCollectionWithID(slot.storedMonster, realSlot);

            
        }

        manager.UpdateCollectionBeasts(manager.currentBag);
        manager.UpdatePartyLevel();
    }
}
