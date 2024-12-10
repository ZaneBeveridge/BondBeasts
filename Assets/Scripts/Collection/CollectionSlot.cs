using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CollectionSlot : Slot, IDropHandler
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;

    public Image backingImage;

    public Sprite symbioticSprite;
    public Sprite parasiticSprite;

    public int slotNum;

    public List<PartyItemSlotMini> miniItemSlots = new List<PartyItemSlotMini>();
    public void Init(Monster monster, CollectionManager man, int num)
    {
        type = SlotType.Collection;
        slotNum = num;
        manager = man;

        storedMonster = monster;

        dynamicImage.sprite = monster.dynamicSprite;
        dynamicImage.color = monster.colour.colour;

        staticImage.sprite = monster.staticSprite;
        variantImage.sprite = monster.variant.variantStillSprite;

        nameText.text = monster.name;
        levelText.text = monster.level.ToString();

        if(monster.symbiotic)
        {
            backingImage.sprite = symbioticSprite;
        }
        else
        {
            backingImage.sprite = parasiticSprite;
        }
        UpdateItems();
    }

    public void UpdateItem()
    {
        dynamicImage.sprite = storedMonster.dynamicSprite;
        dynamicImage.color = storedMonster.colour.colour;

        staticImage.sprite = storedMonster.staticSprite;
        variantImage.sprite = storedMonster.variant.variantStillSprite;

        nameText.text = storedMonster.name;
        levelText.text = storedMonster.level.ToString();
        UpdateItems();
    }



    public void UpdateItems()
    {
        if (storedMonster.item1.id == 0) // locked or empty
        {
            int unlockLevel = 0 * 10;
            if (storedMonster.level >= unlockLevel)
            {
                miniItemSlots[0].Init(2);
            }
            else
            {
                miniItemSlots[0].Init(1);
            }
        }
        else // full
        {
            miniItemSlots[0].Init(storedMonster.item1.icon);
        }

        if (storedMonster.item2.id == 0) // locked or empty
        {
            int unlockLevel = 1 * 10;
            if (storedMonster.level >= unlockLevel)
            {
                miniItemSlots[1].Init(2);
            }
            else
            {
                miniItemSlots[1].Init(1);
            }
        }
        else // full
        {
            miniItemSlots[1].Init(storedMonster.item2.icon);
        }

        if (storedMonster.item3.id == 0) // locked or empty
        {
            int unlockLevel = 2 * 10;
            if (storedMonster.level >= unlockLevel)
            {
                miniItemSlots[2].Init(2);
            }
            else
            {
                miniItemSlots[2].Init(1);
            }
        }
        else // full
        {
            miniItemSlots[2].Init(storedMonster.item3.icon);
        }
    }
    public override void OnClick()
    {
        manager.OpenCollectionInspect(storedMonster, this.gameObject, "Collection", slotNum);
    }

    public void OnDrop(PointerEventData eventData) // drop from party to collection slot, swaps collection slot with party
    {
        GameObject dropped = eventData.pointerDrag;
        Slot slot = dropped.GetComponent<Slot>();

        manager.EndDrag(this.gameObject);

        if (slot.type == SlotType.Party)
        {
            int storedMonID = storedMonster.storedID;

            PartySlot pSlot = dropped.GetComponent<PartySlot>();
            manager.ClearMonsterFromParty(dropped, pSlot.partySlotManager.slotNum - 1); // remove dragged mon from party
            manager.SpawnMonsterInParty(storedMonster, pSlot.partySlotManager.slotNum - 1); // place new mon from dropped slot in party

            

            manager.ClearMonster(storedMonster); // remove mon from dropped slot
            manager.SpawnMonsterInCollectionWithID(slot.storedMonster, storedMonID); // place new mon in dropped slot
        }
        else if (slot.type == SlotType.Collection)
        {
            int storedMonID = storedMonster.storedID;
            CollectionSlot cSlot = dropped.GetComponent<CollectionSlot>();


            manager.ClearMonster(slot.storedMonster); // remove mon from dragged slot
            manager.SpawnMonsterInCollectionWithID(storedMonster, cSlot.slotNum);

            manager.ClearMonster(storedMonster); // remove mon from dropped slot
            manager.SpawnMonsterInCollectionWithID(slot.storedMonster, storedMonID); // place new mon in dropped slot
        }

        manager.UpdateCollectionAll();
    }
}
