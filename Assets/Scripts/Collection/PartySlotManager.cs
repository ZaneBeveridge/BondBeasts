using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartySlotManager : MonoBehaviour, IDropHandler
{
    public int slotNum;

    public GameObject partySlotPrefab;

    public CollectionManager manager;

    public GameObject storedMonsterObject;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) { return; }

        GameObject dropped = eventData.pointerDrag;
        

        Slot slot = dropped.GetComponent<Slot>();
        if (slot == null) { return; }

        Monster monster = slot.storedMonster;

        

        if (storedMonsterObject != null) //SWAP
        {
            if (slot.type == SlotType.Collection) // SWAP BETWEEN COLLECTION AND PARTY
            {
                GameObject mon = Instantiate(partySlotPrefab, transform);
                mon.GetComponent<PartySlot>().Init(monster, manager, this);
                

                manager.ClearMonster(dropped);

                manager.SpawnMonsterInCollection(storedMonsterObject.GetComponent<PartySlot>().storedMonster);

                Destroy(storedMonsterObject);
                storedMonsterObject = mon;
            }
            else if (slot.type == SlotType.Party) // SWAP BETWEEN PARTY AND PARTY
            {
                PartySlotManager previousPartyManager = dropped.GetComponent<PartySlot>().partySlotManager;

                GameObject mon = Instantiate(partySlotPrefab, transform);
                mon.GetComponent<PartySlot>().Init(monster, manager, this);

                GameObject previousMon = Instantiate(partySlotPrefab, previousPartyManager.gameObject.transform);
                previousMon.GetComponent<PartySlot>().Init(storedMonsterObject.GetComponent<PartySlot>().storedMonster, manager, previousPartyManager);


                previousPartyManager.storedMonsterObject = previousMon;

                Destroy(storedMonsterObject);
                storedMonsterObject = mon;

                Destroy(dropped);

            }
        }
        else //PLACE
        {
            if (slot.type == SlotType.Collection) // PLACE FROM COLLECTION TO PARTY
            {
                GameObject mon = Instantiate(partySlotPrefab, transform);
                mon.GetComponent<PartySlot>().Init(monster, manager, this);
                storedMonsterObject = mon;

                manager.ClearMonster(dropped);
            }
            else if (slot.type == SlotType.Party) // PLACE FROM PARTY TO PARTY
            {
                GameObject mon = Instantiate(partySlotPrefab, transform);
                mon.GetComponent<PartySlot>().Init(monster, manager, this);
                storedMonsterObject = mon;

                int fromSlotNum = dropped.GetComponent<PartySlot>().partySlotManager.slotNum;

                manager.ClearMonsterFromParty(dropped, fromSlotNum);
            }
        }





        manager.UpdatePartyLevel();




    }

 
}
