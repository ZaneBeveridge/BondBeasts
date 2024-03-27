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
    }

    public override void OnClick()
    {
        manager.OpenItemInspectTooltip(item, transform);
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
