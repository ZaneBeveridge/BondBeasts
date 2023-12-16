using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemSlotUIEquip : ItemSlotMoveable
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI numberText;

    private ItemEquipMenu menu;

    public DragableItem dragableItem;

    private string typeCollection;
    private int slotInStorage;

    public void Init( MonsterItemSO i, int number, ItemEquipMenu m, string t, int s, GameManager g)
    {
        dragableItem.manager = g;
        manager = g;
        icon.sprite = i.icon;
        nameText.text = i.itemName;
        numberText.text = number.ToString();
        item = i;
        menu = m;
        typeCollection = t;
        slotInStorage = s;

    }
    public override void OnClick()
    {
        manager.OpenItemInspectTooltip(item, transform);
        //menu.SelectItem(item, typeCollection, slotInStorage);
    }
}
