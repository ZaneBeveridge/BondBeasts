using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInspectPopup : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI loreText;

    public GameObject unequipButton;

    public GameObject upgradeObject;

    public ItemUpgradeManager upgradeManager;

    private MonsterItemSO item;

    private GameManager GM;


    private int partySlot = 0;
    private int equipSlot = 0;
    public bool partyItemSelected = false;

    private TeamEquipSlot cSlot;
    public void Init(MonsterItemSO i, GameManager g)
    {
        GM = g;
        item = i;
        icon.sprite = i.icon;
        nameText.text = i.itemName;
        descText.text = i.desc;
        loreText.text = i.lore;
        partyItemSelected = false;

        if (i.canBeUpgraded)
        {
            upgradeObject.SetActive(true);
        }
        else
        {
            upgradeObject.SetActive(false);
        }
        unequipButton.SetActive(false);

        UpdatePartySlotsEquipSlots(i, null);
    }

    public void Init(MonsterItemSO i, GameManager g, int pSlot, int eSlot, TeamEquipSlot clickedSlot) // equipped item
    {
        GM = g;
        item = i;
        icon.sprite = i.icon;
        nameText.text = i.itemName;
        descText.text = i.desc;
        loreText.text = i.lore;
        partySlot = pSlot;
        equipSlot = eSlot;
        partyItemSelected = true;
        cSlot = clickedSlot;
        if (i.canBeUpgraded)
        {
            upgradeObject.SetActive(true);
        }
        else
        {
            upgradeObject.SetActive(false);
        }

        unequipButton.SetActive(true);

        UpdatePartySlotsEquipSlots(i, clickedSlot);
    }

    

    private void UpdatePartySlotsEquipSlots(MonsterItemSO it, TeamEquipSlot clickedSlot)
    {

        for (int i = 0; i < GM.collectionManager.partySlots.Count; i++)
        {

            if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                PartySlot partySlot = GM.collectionManager.partySlots[i].storedMonsterObject.GetComponent<PartySlot>();
                for (int j = 0; j < partySlot.equipSlots.Count; j++)
                {
                    if (partySlot.equipSlots[j].currentEquippedSlot != null) // equip slot has item in it 
                    {
                        if (partySlot.equipSlots[j].currentEquippedSlot.GetComponent<TeamEquipSlot>() != clickedSlot)
                        {
                            partySlot.equipSlots[j].currentEquippedSlot.GetComponent<TeamEquipSlot>().HighlightSlotForEquip(it, partyItemSelected, clickedSlot);
                        }
                        
                    }
                    else // no item in slot
                    {
                        partySlot.equipSlots[j].HighlightSlotForEquip(it, partyItemSelected, clickedSlot);
                    }
                }
            }
        }
    }
    public void Unequip()
    {
        //Unequip here
        GM.collectionManager.RemoveItemFromMonsterInParty(cSlot.pManager.slotNum - 1, cSlot.slotNum + 1);

        GM.collectionManager.AddItemToStorage(item, 1);
        ClosePanel();
    }

    public void Upgrade()
    {
        upgradeManager.Init(item, GM, partyItemSelected, partySlot, equipSlot);
    }

    public void ClosePanel()
    {
        ResetSelections();
        GM.collectionManager.UpdateCollectionAll();
        GM.itemInspectManagerPopup.CloseCurrentPanel();
    }

    public void ResetSelections()
    {
       
        for (int i = 0; i < GM.collectionManager.partySlots.Count; i++)
        {
            if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                PartySlot partySlot = GM.collectionManager.partySlots[i].storedMonsterObject.GetComponent<PartySlot>();
                for (int j = 0; j < partySlot.equipSlots.Count; j++)
                {
                    if (partySlot.equipSlots[j].currentEquippedSlot != null) // equip slot has item in it 
                    {
                        partySlot.equipSlots[j].currentEquippedSlot.GetComponent<TeamEquipSlot>().ResetSelectToEquip();

                    }
                    else // no item in slot
                    {
                        partySlot.equipSlots[j].ResetSelectToEquip();
                    }
                }
            }
        }
    }
}
