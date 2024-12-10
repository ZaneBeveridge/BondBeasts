using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TeamEquipSlotManager : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;

    public int slotNum = 0;

    public Sprite noItemSprite;
    public Sprite lockedSprite;
    public Sprite canGoHereSprite;
    public Sprite hoverSprite;

    public GameObject teamEquipSlotPrefab;
    [HideInInspector] public GameManager GM;
    public int mode = 0; // 0==locked, 1==unlocked

    public PartySlot teamSlot;
    public GameObject currentEquippedSlot;
    private bool inSelectToEquipMode = false;

    private MonsterItemSO currentItem;
    private TeamEquipSlot tempEquipSlot;
    private bool partyI = false;
    public void HighlightSlotForEquip(MonsterItemSO itm, bool partyItemSelected, TeamEquipSlot tEquip)
    {
        if (mode == 1)
        {
            //Debug.Log("testequipManager");
            partyI = partyItemSelected;
            tempEquipSlot = tEquip;
            inSelectToEquipMode = true;
            image.sprite = hoverSprite;
            currentItem = itm;
            text.text = "";
        }
        
    }

    public void ResetSelectToEquip()
    {
        image.sprite = noItemSprite;
        text.text = "NO ITEM EQUIPPED";
        inSelectToEquipMode = false;
    }


    public void UpdateSlot(Monster monster, bool holding, GameManager man)
    {
        GM = man;
        Destroy(currentEquippedSlot);
        currentEquippedSlot = null;
        MonsterItemSO itm = monster.item1;
        switch (slotNum)
        {
            case 1:
                itm = monster.item1;
                break;
            case 2:
                itm = monster.item2;
                break;
            case 3:
                itm = monster.item3;
                break;
        }

        int unlockLevel = slotNum * 10;

        if (monster.level >= unlockLevel)
        {
            mode = 1; //UNLOCKED
            if (itm.id == 0) // blank item
            {
                if (holding)
                {
                    image.sprite = canGoHereSprite;
                    text.text = "";
                }
                else
                {
                    image.sprite = noItemSprite;
                    text.text = "NO ITEM EQUIPPED";
                }
            }
            else
            {
                //INSTANTIATE ITEM HERE
                GameObject itemObject = Instantiate(teamEquipSlotPrefab, this.transform);
                itemObject.GetComponent<TeamEquipSlot>().Init(itm, GM, teamSlot.partySlotManager, this, slotNum);
                currentEquippedSlot = itemObject;

                image.sprite = canGoHereSprite;
                text.text = "";
            }
        }
        else
        {
            mode = 0; // LOCKED
            image.sprite = lockedSprite;
            text.text = "LVL " + unlockLevel.ToString();
        }


        
    }

    public void OnClick() // Remove item from storage and equip it to this empty slot 
    {
        if (inSelectToEquipMode)
        {
            if (partyI)// if selecting a item already equipped
            {
                int thisSlotNum = teamSlot.partySlotManager.slotNum - 1;
                TeamEquipSlot equipSlot = tempEquipSlot;
                GM.collectionManager.RemoveItemFromMonsterInParty(equipSlot.pManager.slotNum - 1, equipSlot.slotNum + 1);
                //manager.collectionManager.AddItemToMonsterInParty(item, equipSlot.pManager.slotNum - 1, equipSlot.slotNum + 1);

                GM.collectionManager.AddItemToMonsterInParty(currentItem, thisSlotNum, slotNum + 1);

                if (GM.itemInspectManagerPopup.currentPanel != null)
                {
                    GM.itemInspectManagerPopup.currentPanel.GetComponent<ItemInspectPopup>().ClosePanel();
                }
            }
            else
            {
                GM.collectionManager.RemoveItemFromStorage(currentItem);

                int thisSlotNum = teamSlot.partySlotManager.slotNum - 1;

                GM.collectionManager.AddItemToMonsterInParty(currentItem, thisSlotNum, slotNum + 1); // add item to new slot

                if (GM.itemInspectManagerPopup.currentPanel != null)
                {
                    GM.itemInspectManagerPopup.currentPanel.GetComponent<ItemInspectPopup>().ClosePanel();
                }
            }
        }
    }

    /*
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        ItemSlot slot = dropped.GetComponent<ItemSlot>();

        if (slot == null) { return; }

        if (slot.itemType == ItemType.Catalyst && mode == 1) // is cata, and mode is unlocked
        {
            if (slot.slotType == ItemSlotType.StorageSlot) // Drag from storage to this empty slot
            {
                int teamSlotNum = teamSlot.partySlotManager.slotNum - 1;


                GM.collectionManager.RemoveItemFromStorage(slot.item);
                GM.collectionManager.AddItemToMonsterInParty(slot.item, teamSlotNum, slotNum + 1);
            }
            else if (slot.slotType == ItemSlotType.EquipSlot) // Drag from equipment slot to this empty slot
            {
                int thisSlotNum = teamSlot.partySlotManager.slotNum - 1;
                TeamEquipSlot equipSlot = slot.GetComponent<TeamEquipSlot>();

                GM.collectionManager.RemoveItemFromMonsterInParty(equipSlot.pManager.slotNum - 1, equipSlot.slotNum + 1); // remove item from held mon slot
                GM.collectionManager.AddItemToMonsterInParty(slot.item, thisSlotNum, slotNum + 1); // add item to new slot
            }
        }

        GM.collectionManager.UpdateCollectionAll();
    }
    */
}
