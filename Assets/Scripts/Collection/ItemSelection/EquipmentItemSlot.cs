using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class EquipmentItemSlot : ItemSlot
{
    public Image mainImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;
    public int slotNum;
    public Image cover;

    public void Init(StoredItem itm, GameManager man)
    {
        slotType = ItemSlotType.StorageSlot;
        itemType = itm.item.type;
        slotNum = itm.storedID;
        manager = man;

        item = itm.item;

        if (itm.amount <= 0)
        {
            cover.color = new Color(255f,255f,255f,0.5f);
            button.interactable = false;
            if (nameText != null)
            {
                nameText.text = "";
            }
        }
        else
        {
            cover.color = new Color(255f, 255f, 255f, 0f);
            button.interactable = true;
            if (nameText != null)
            {
                nameText.text = itm.item.itemName;
            }
            
        }

        icon.sprite = itm.item.icon;
        amountText.text = "x" + itm.amount.ToString();
        amount = itm.amount;
        
    }


    public override void OnClick()
    {
        if (amount > 0)
        {
            manager.OpenItemInspectTooltip(item);
        }
    }
    /*
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        ItemSlot slot = dropped.GetComponent<ItemSlot>();

        manager.collectionManager.EndDrag(this.gameObject);

        if (slot.itemType == ItemType.Catalyst)
        {
            int slotFrom = slot.GetComponent<EquipmentItemSlot>().slotNum;


            
        }


        manager.collectionManager.UpdateCollectionAll();
    }
    */

}
