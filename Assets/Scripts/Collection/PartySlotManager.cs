using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySlotManager : MonoBehaviour, IDropHandler
{
    public int slotNum;

    public GameObject partySlotPrefab;

    public CollectionManager manager;

    public GameObject storedMonsterObject;

    public Image backing; 
    public void SetDroppableArt(bool state)
    {
        if (state)
        {
            backing.color = new Color(255, 255, 255, 255);
        }
        else
        {
            backing.color = new Color(255, 255, 255, 0);
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) { return; }

        GameObject dropped = eventData.pointerDrag;
        Slot slot = dropped.GetComponent<Slot>();
        Dragable dragable = dropped.GetComponent<Dragable>();
        if (slot == null) { return; }
        if (dragable == null || dragable.active == false) { return; }

        Monster monster = slot.storedMonster;

        manager.EndDrag(dropped);

        if (storedMonsterObject != null) //SWAP
        {
            if (slot.type == SlotType.Collection) // SWAP BETWEEN COLLECTION AND PARTY
            {
                Monster monsterFromThisObject = storedMonsterObject.GetComponent<PartySlot>().storedMonster;

                manager.ClearMonster(monster); // remove held collection mon
                manager.ClearMonsterFromParty(storedMonsterObject, slotNum - 1); // remove this mon in party
                

                manager.SpawnMonsterInCollectionWithID(monsterFromThisObject, monster.storedID); // add this mon to collection
                manager.SpawnMonsterInParty(monster, slotNum - 1); // add held mon to party

            }
            else if (slot.type == SlotType.Party) // SWAP BETWEEN PARTY AND PARTY
            {
                int fromSlotNum = dropped.GetComponent<PartySlot>().partySlotManager.slotNum - 1;
                //PartySlotManager previousPartyManager = dropped.GetComponent<PartySlot>().partySlotManager;
                Monster monsterFromThisObject = storedMonsterObject.GetComponent<PartySlot>().storedMonster;

                manager.ClearMonsterFromParty(dropped, fromSlotNum); // remove held mon from party
                manager.ClearMonsterFromParty(storedMonsterObject, slotNum - 1); //remove this mon from party


                manager.SpawnMonsterInParty(monster, slotNum - 1); // add held mon back to party in new slot
                manager.SpawnMonsterInParty(monsterFromThisObject, fromSlotNum); // add this mon back to party in new slot
                

            }
        }
        else //PLACE
        {
            if (slot.type == SlotType.Collection) // PLACE FROM COLLECTION TO PARTY
            {
                manager.ClearMonster(monster);

                manager.SpawnMonsterInParty(monster, slotNum - 1);

                
            }
            else if (slot.type == SlotType.Party) // PLACE FROM PARTY TO PARTY
            {
                int fromSlotNum = dropped.GetComponent<PartySlot>().partySlotManager.slotNum - 1;

                manager.ClearMonsterFromParty(dropped, fromSlotNum);

                manager.SpawnMonsterInParty(monster, slotNum - 1);
            }
        }



        manager.UpdateCollectionBeasts(manager.currentBag);

        manager.UpdatePartyLevel();




    }

 
}
