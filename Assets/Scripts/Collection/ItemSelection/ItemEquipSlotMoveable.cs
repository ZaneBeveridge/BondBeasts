using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemEquipSlotMoveable : ItemSlotMoveable
{
    [HideInInspector]public ItemSlotManager itemSlotManager;

    public DragableItem dragableItem;



    void Update()
    {
        if (dragableItem.dragging && itemSlotManager != null)
        {
            itemSlotManager.backImage.sprite = itemSlotManager.noItem;
            itemSlotManager.nameText.text = "";
            itemSlotManager.descText.text = "";
        }
        else if (!dragableItem.dragging && itemSlotManager != null)
        {
            itemSlotManager.backImage.sprite = itemSlotManager.withItem;
            itemSlotManager.nameText.text = item.itemName;
            itemSlotManager.descText.text = item.desc;
        }
    }

    //public int slotNum;
    public void Init(MonsterItemSO i, GameManager man, ItemSlotManager itemMan)
    {
        //slotNum = num;
        dragableItem.manager = man;
        type = ItemSlotType.EquipSlot;
        manager = man;
        itemSlotManager = itemMan;

        
        if (i.id == 0)
        {
            itemMan.backImage.sprite = itemMan.noItem;
        }
        else
        {
            itemMan.backImage.sprite = itemMan.withItem;
        }
        

        item = i;
        icon.sprite = item.icon;
        itemSlotManager.nameText.text = item.itemName;
        itemSlotManager.descText.text = item.desc;
    }

    public void UpdateItem()
    {
        icon.sprite = item.icon;
        itemSlotManager.nameText.text = item.name;
        itemSlotManager.descText.text = item.desc;

        
        if (item.id == 0)
        {
            itemSlotManager.backImage.sprite = itemSlotManager.noItem;
        }
        else
        {
            itemSlotManager.backImage.sprite = itemSlotManager.withItem;
        }
        
    }

    public override void OnClick()
    {
        itemSlotManager.panel.OpenSelectItemWindow(itemSlotManager.slotNum, itemSlotManager.rightS);
    }


}
