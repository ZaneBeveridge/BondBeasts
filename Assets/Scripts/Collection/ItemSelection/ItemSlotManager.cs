using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
public class ItemSlotManager : MonoBehaviour, IDropHandler
{
    public bool rightS;
    public int slotNum;
    public GameObject slotPrefab;

    public Image backImage;
    public Sprite noItem;
    public Sprite withItem;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;

    public GameObject storedItemObject;
    public CollectionInspectPanel panel;
    public AllSlotsManagerItems allManager;
    [HideInInspector]public GameManager manager;
    public void Awake()
    {
        manager = panel.g;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) { return; }

        GameObject dropped = eventData.pointerDrag;

        manager.itemInspectManagerPopup.CloseCurrentPanel();
        ItemSlotMoveable slot = dropped.GetComponent<ItemSlotMoveable>();
        if (slot == null) { return; }

        MonsterItemSO item = slot.item;


        if (storedItemObject != null) //SWAP
        {
            if (slot.type == ItemSlotType.StorageSlot) // SWAP BETWEEN ITEM STORAGE AND EQUIP
            {
                if (allManager.CheckIfAlreadyEquipped(item)) { return; }

                GameObject itm = Instantiate(slotPrefab, transform);
                itm.GetComponent<ItemEquipSlotMoveable>().Init(item, manager, this);

                manager.RemoveItemFromMonster(slotNum);
                manager.AddItemToMonster(item, slotNum);
                
                manager.RemoveItemFromStorage(item);
                manager.AddItemToStorage(storedItemObject.GetComponent<ItemEquipSlotMoveable>().item);
                //REMOVE OLD OBJECT

                Destroy(storedItemObject);
                Destroy(dropped);
                //SET NEW OBJECT
                storedItemObject = itm;
                
            }
            else if (slot.type == ItemSlotType.EquipSlot) // SWAP BETWEEN EQUIP AND EQUIP
            {
                //GETS DROPPED ITEMS ITEM SLOT MANAGER FOR REFERENCE
                ItemSlotManager previousPartyManager = dropped.GetComponent<ItemEquipSlotMoveable>().itemSlotManager;

                //SPAWNS ITEM AT THIS LOCATION BASED ON DROPPED ITEM INFO
                GameObject itm = Instantiate(slotPrefab, transform);
                itm.GetComponent<ItemEquipSlotMoveable>().Init(item, manager, this);

                //SPAWNS ITEM AT DROPPED ITEMS MANAGER WITH THE INFO FROM THIS MANAGERS ITEM
                GameObject previousitm = Instantiate(slotPrefab, previousPartyManager.gameObject.transform);
                previousitm.GetComponent<ItemEquipSlotMoveable>().Init(storedItemObject.GetComponent<ItemEquipSlotMoveable>().item, manager, previousPartyManager);

                //SET LAST ITEM MANAGER ITEM 
                previousPartyManager.storedItemObject = previousitm;

                manager.RemoveItemFromMonster(slotNum);
                manager.AddItemToMonster(item, slotNum);

                manager.RemoveItemFromMonster(previousPartyManager.slotNum);
                manager.AddItemToMonster(storedItemObject.GetComponent<ItemEquipSlotMoveable>().item, previousPartyManager.slotNum);
                //REMOVE OLD OBJECT
                Destroy(storedItemObject); 
                Destroy(dropped);

                storedItemObject = itm;
            }
        }
        else //PLACE
        {
            if (slot.type == ItemSlotType.StorageSlot) // PLACE FROM ITEM STORAGE TO EQUIP
            {
                if (allManager.CheckIfAlreadyEquipped(item)) { return; }
                GameObject itm = Instantiate(slotPrefab, transform);
                itm.GetComponent<ItemEquipSlotMoveable>().Init(item, manager, this);
                storedItemObject = itm;

                
                manager.AddItemToMonster(item, slotNum);
                manager.RemoveItemFromStorage(item);
            }
            else if (slot.type == ItemSlotType.EquipSlot) // PLACE FROM EQUIP TO EQUIP
            {
                GameObject itm = Instantiate(slotPrefab, transform);
                itm.GetComponent<ItemEquipSlotMoveable>().Init(item, manager, this);
                storedItemObject = itm;

                dropped.GetComponent<ItemEquipSlotMoveable>().itemSlotManager.backImage.sprite = dropped.GetComponent<ItemEquipSlotMoveable>().itemSlotManager.noItem;

                int fromSlotNum = dropped.GetComponent<ItemEquipSlotMoveable>().itemSlotManager.slotNum;

                manager.AddItemToMonster(item, slotNum);
                manager.RemoveItemFromMonster(fromSlotNum);

                
            }
        }


        panel.UpdateInspectPanel();

    }


    

    public void OnButtonClick()
    {
        panel.OpenSelectItemWindow(slotNum, rightS);
    }
}
