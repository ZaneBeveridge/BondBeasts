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

    public GameObject upgradeObject;

    public ItemUpgradeManager upgradeManager;

    private MonsterItemSO item;

    private GameManager GM;

    private int partySlot = 0;
    private int equipSlot = 0;
    private bool partyItemSelected = false;
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
    }

    public void Init(MonsterItemSO i, GameManager g, int pSlot, int eSlot)
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

        if (i.canBeUpgraded)
        {
            upgradeObject.SetActive(true);
        }
        else
        {
            upgradeObject.SetActive(false);
        }
    }


    public void Upgrade()
    {
        upgradeManager.Init(item, GM, partyItemSelected, partySlot, equipSlot);
    }

    public void ClosePanel()
    {
        GM.itemInspectManagerPopup.CloseCurrentPanel();
    }
}
