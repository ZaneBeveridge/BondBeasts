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
        manager.OpenItemInspectTooltip(item, transform, simple);
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
