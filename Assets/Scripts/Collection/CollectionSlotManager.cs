using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectionSlotManager : MonoBehaviour, IDropHandler
{

    public CollectionManager manager;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Slot slot = dropped.GetComponent<Slot>();

        if (slot == null) { return; }

        if (slot.type == SlotType.Party)
        {
            manager.SpawnMonsterInCollection(slot.storedMonster);

            manager.ClearMonsterFromParty(dropped, dropped.GetComponent<PartySlot>().partySlotManager.slotNum);
        }

        manager.UpdatePartyLevel();
    }
}
