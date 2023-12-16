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

    public int slotNum;
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


    }

    public void UpdateItem()
    {
        dynamicImage.sprite = storedMonster.dynamicSprite;
        dynamicImage.color = storedMonster.colour.colour;

        staticImage.sprite = storedMonster.staticSprite;
        variantImage.sprite = storedMonster.variant.variantStillSprite;

        nameText.text = storedMonster.name;
        levelText.text = storedMonster.level.ToString();

    }

    public override void OnClick()
    {
        manager.OpenCollectionInspect(storedMonster, this.gameObject, "Collection", slotNum);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Slot slot = dropped.GetComponent<Slot>();

        if (slot.type == SlotType.Party)
        {
            PartySlot pSlot = dropped.GetComponent<PartySlot>();
            GameObject mon = Instantiate(pSlot.partySlotManager.partySlotPrefab, pSlot.partySlotManager.gameObject.transform);
            mon.GetComponent<PartySlot>().Init(storedMonster, manager, pSlot.partySlotManager);
            pSlot.partySlotManager.storedMonsterObject = mon;

            Destroy(dropped);

            manager.SpawnMonsterInCollection(slot.storedMonster);
            



            manager.ClearMonster(this.gameObject);

        }
    }
}
