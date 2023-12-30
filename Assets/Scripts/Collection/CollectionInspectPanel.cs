using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionInspectPanel : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public Color statColor1;
    public Color statColor2;
    public Color statColor3;
    public Color statColor4;
    
    public GameObject panel;

    public Button item1Button;
    public Button item2Button;
    public Button item3Button;

    public Image dynamicImage;
    public Image staticImage;
    public Image variantImage;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI idText;

    public TextMeshProUGUI damageNum;
    public TextMeshProUGUI defenseNum;
    public TextMeshProUGUI regenNum;
    public TextMeshProUGUI attackCooldownNum;
    public TextMeshProUGUI specialCooldownNum;
    public TextMeshProUGUI switchCooldownNum;

    public TextMeshProUGUI damageNumMod;
    public TextMeshProUGUI defenseNumMod;
    public TextMeshProUGUI regenNumMod;
    public TextMeshProUGUI attackCooldownNumMod;
    public TextMeshProUGUI specialCooldownNumMod;
    public TextMeshProUGUI switchCooldownNumMod;

    public List<Slider> sliders = new List<Slider>();
    public List<Image> fills = new List<Image>();

    public TextMeshProUGUI natureNum;
    public TextMeshProUGUI colourNum;
    public TextMeshProUGUI strange;


    public TextMeshProUGUI basicMoveText;
    public TextMeshProUGUI specialMoveText;
    public TextMeshProUGUI passiveMoveText;

    public GameObject statButton;
    public GameObject skillButton;
    public GameObject itemButton;
    public GameObject loreButton;

    public GameObject statPageButton;
    public GameObject skillPageButton;
    public GameObject itemPageButton;
    public GameObject lorePageButton;


    public TextMeshProUGUI cooldownBasic1;
    public TextMeshProUGUI cooldownSpecial1;
    public TextMeshProUGUI cooldownPassive1;

    public TextMeshProUGUI projectileTextBasic;
    public TextMeshProUGUI projectileTextSpecial;

    public TextMeshProUGUI heightText;
    public TextMeshProUGUI weightText;
    public TextMeshProUGUI loreText;
    public TextMeshProUGUI locationsText;

    [Header("Items")]
    public List<GameObject> storageItems = new List<GameObject>();
    public List<ItemSlotManager> itemEquipSlots = new List<ItemSlotManager>();

    public GameObject itemSlotPrefab;

    public ItemEquipMenu itemEquipMenu;

    private GameObject storedObject;
    [HideInInspector]public GameManager g;

    private Monster currentMonster;

    

    // private string typeString = "None";
    //private int slotNumberStorage = 0;


    public void UpdatePanel(Monster monster, GameManager GM)
    {
        if (monster.capLevel == 0)
        {
            item1Button.gameObject.SetActive(true);
            item2Button.gameObject.SetActive(false);
            item3Button.gameObject.SetActive(false);
        }
        else if (monster.capLevel == 1)
        {
            item1Button.gameObject.SetActive(true);
            item2Button.gameObject.SetActive(true);
            item3Button.gameObject.SetActive(false);
        }
        if (monster.capLevel == 2)
        {
            item1Button.gameObject.SetActive(true);
            item2Button.gameObject.SetActive(true);
            item3Button.gameObject.SetActive(true);
        }


        dynamicImage.sprite = monster.dynamicSprite;
        staticImage.sprite = monster.staticSprite;
        variantImage.sprite = monster.variant.variantStillSprite;
        dynamicImage.color = monster.colour.colour;

        nameText.text = monster.name;
        levelText.text = "Level " + monster.level.ToString();

        float xpReq = 100;
        for (int i = 0; i < monster.level - 1; i++)
        {
            xpReq = xpReq * 1.2f;
        }


        xpText.text = monster.xp.ToString() + " / " + Mathf.RoundToInt(xpReq) + " xp";
        idText.text = "ID: " + monster.ID.ID.ToString("000") + "-" + monster.ID.version;

        damageNum.text = monster.stats[0].value.ToString();
        defenseNum.text = monster.stats[1].value.ToString();
        regenNum.text = monster.stats[2].value.ToString();
        attackCooldownNum.text = monster.stats[3].value.ToString();
        specialCooldownNum.text = monster.stats[4].value.ToString();
        switchCooldownNum.text = monster.stats[5].value.ToString();

        List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
        texts.Add(damageNum);
        texts.Add(defenseNum);
        texts.Add(regenNum);
        texts.Add(attackCooldownNum);
        texts.Add(specialCooldownNum);
        texts.Add(switchCooldownNum);

        for (int i = 0; i < monster.nature.addedStats.Count; i++)
        {
            if (monster.nature.addedStats[i].value > 1) // green
            {
                SetModText(i, monster.nature.addedStats[i].value * 100 - 100, "Green");

                //texts[i].text = Mathf.RoundToInt(monster.stats[i].value * monster.nature.addedStats[i].value).ToString();
                texts[i].color = Color.green;
            }
            else if (monster.nature.addedStats[i].value < 1) // red
            {
                SetModText(i, monster.nature.addedStats[i].value * 100 - 100, "Red");
                //texts[i].text = Mathf.RoundToInt(monster.stats[i].value * monster.nature.addedStats[i].value).ToString();
                texts[i].color = Color.red;
            }
            else // white
            {
                SetModText(i, monster.nature.addedStats[i].value * 100 - 100, "Off");
                texts[i].color = Color.white;
            }

            int valu = 0;
            if (monster.stats[i].value < 0)
            {
                if (monster.nature.addedStats[i].value < 1) valu = Mathf.RoundToInt(-(-monster.stats[i].value * (monster.nature.addedStats[i].value + 1)));
                else valu = Mathf.RoundToInt(-(-monster.stats[i].value * (monster.nature.addedStats[i].value - 1)));
            }
            else
            {
                valu = Mathf.RoundToInt(monster.stats[i].value * monster.nature.addedStats[i].value);
            }

            //Debug.Log(valu);
            texts[i].text = valu.ToString();
        }



        for (int i = 0; i < sliders.Count; i++)
        {
            float value = monster.stats[i].value * monster.nature.addedStats[i].value;
            int level = 0;

            while (value > 100)
            {
                level++;
                value = value - 100;
            }

            sliders[i].value = value;
            sliders[i].maxValue = 100;



            if (level == 3) // over 300
            {
                fills[i].color = statColor4;
            }
            else if (level == 2) // 201-300
            {
                fills[i].color = statColor3;
            }
            else if (level == 1)// 101-200
            {
                fills[i].color = statColor2;
            }
            else if (level == 0)// 0-100
            {
                fills[i].color = statColor1;
            }
        }

        natureNum.text = monster.nature.natureName;

        float col = GetHSVColourValue(monster.colour.colour) * 360f;

        int colorNum = Mathf.RoundToInt(col);

        colourNum.text = colorNum.ToString();

        if (monster.strange)
        {
            strange.text = "Strange!";
        }
        else
        {
            strange.text = "";
        }


        basicMoveText.text = monster.basicMove.moveDescription;
        specialMoveText.text = monster.specialMove.moveDescription;
        passiveMoveText.text = monster.passiveMove.moveDescription;

        float valueAmountB = monster.stats[3].value + (monster.stats[3].value * (GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.slotValues[3] / 100));
        float valueRealB = valueAmountB;


        if (valueAmountB > 100)
        {
            valueRealB = 100f;

        }

        float valueAmountS = monster.stats[4].value + (monster.stats[4].value * (GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.slotValues[4] / 100));
        float valueRealS = valueAmountS;


        if ((int)valueAmountS > 100)
        {
            valueRealS = 100f;

        }
        //Debug.Log("ValueFirst Basic:" + valueAmountB + " ValueFirst Special: " + valueAmountS);
        //Debug.Log("ValueReal Basic:" + valueRealB + " ValueReal Special: " + valueRealS);

        float cooldownBasic = Mathf.Round(monster.basicMove.baseCooldown - (monster.basicMove.baseCooldown * (0.008f * valueRealB)));
        float cooldownSpecial = Mathf.Round(monster.specialMove.baseCooldown - (monster.specialMove.baseCooldown * (0.008f * valueRealS)));

        //Debug.Log("Float Basic:" + cooldownBasic + " Float Special: " + cooldownSpecial);

        int printB = (int)cooldownBasic;
        int printS = (int)cooldownSpecial;

        // Debug.Log("Print Basic:" + printB + " Print Special: " + printS);

        cooldownBasic1.text = printB.ToString() + "s";

        cooldownSpecial1.text = printS.ToString() + "s";

        //cooldownPassive1.text = monster.passiveMove.baseCooldown.ToString() + "s";
        //cooldownPassive2.text = monster.passiveMove.baseCooldown.ToString() + "s";

        bool basicProjectile = false;
        bool specialProjectile = false;


        for (int i = 0; i < monster.basicMove.moveActions.Count; i++)
        {
            if (monster.basicMove.moveActions[i].effect.effectType == EffectType.FireProjectile)
            {
                basicProjectile = true;
                break;
            }
        }

        for (int i = 0; i < monster.specialMove.moveActions.Count; i++)
        {
            if (monster.specialMove.moveActions[i].effect.effectType == EffectType.FireProjectile)
            {
                specialProjectile = true;
                break;
            }
        }

        if (basicProjectile)
        {
            projectileTextBasic.gameObject.SetActive(true);
        }
        else
        {
            projectileTextBasic.gameObject.SetActive(false);
        }

        if (specialProjectile)
        {
            projectileTextSpecial.gameObject.SetActive(true);
        }
        else
        {
            projectileTextSpecial.gameObject.SetActive(false);
        }

        for (int i = 0; i < itemEquipSlots.Count; i++)
        {
            if (itemEquipSlots[i].storedItemObject != null)
            {
                Destroy(itemEquipSlots[i].storedItemObject);
                itemEquipSlots[i].storedItemObject = null;
            }
        }

        if (monster.item1.id != 0)
        {
            GameObject itm = Instantiate(itemSlotPrefab, itemEquipSlots[0].transform);
            itm.GetComponent<ItemEquipSlotMoveable>().Init(monster.item1, g, itemEquipSlots[0]);
            itemEquipSlots[0].storedItemObject = itm;
        }
        else
        {
            itemEquipSlots[0].nameText.text = "";
            itemEquipSlots[0].descText.text = "TAP TO EQUIP";
        }

        if (monster.item2.id != 0)
        {
            GameObject itm = Instantiate(itemSlotPrefab, itemEquipSlots[1].transform);
            itm.GetComponent<ItemEquipSlotMoveable>().Init(monster.item2, g, itemEquipSlots[1]);
            itemEquipSlots[1].storedItemObject = itm;
        }
        else
        {
            itemEquipSlots[1].nameText.text = "";
            itemEquipSlots[1].descText.text = "TAP TO EQUIP";
        }

        if (monster.item3.id != 0)
        {
            GameObject itm = Instantiate(itemSlotPrefab, itemEquipSlots[2].transform);
            itm.GetComponent<ItemEquipSlotMoveable>().Init(monster.item3, g, itemEquipSlots[2]);
            itemEquipSlots[2].storedItemObject = itm;
        }
        else
        {
            itemEquipSlots[2].nameText.text = "";
            itemEquipSlots[2].descText.text = "TAP TO EQUIP";
        }

   
        heightText.text = monster.backupData.height;
        weightText.text = monster.backupData.weight;
        loreText.text = monster.backupData.lore;
        locationsText.text = monster.backupData.locations;
    }
    public void OpenPanel(Monster monster, GameObject obj, GameManager GM, string type, int numInStorage)
    {
        currentMonster = monster;
        //typeString = type;
        //slotNumberStorage = slotNum;

        titleText.text = "STATS";
        GM.itemEquipMenu = itemEquipMenu;
        g = GM;
        storedObject = obj;
        panel.SetActive(true);
        statButton.SetActive(false);
        skillButton.SetActive(true);
        itemButton.SetActive(true);
        loreButton.SetActive(true);

        statPageButton.SetActive(true);
        skillPageButton.SetActive(false);
        itemPageButton.SetActive(false);
        lorePageButton.SetActive(false);

        GM.monsterType = type;
        GM.monsterNumInStorage = numInStorage;


        


        UpdatePanel(monster, GM);
    }

    

    private void SetModText(int index, float value, string mode)
    {
        if (index == 0)
        {
            if (mode == "Green") { damageNumMod.color = Color.green; damageNumMod.text = " +" + value.ToString() + "%"; }
            else if (mode == "Red") { damageNumMod.color = Color.red; damageNumMod.text = " " + value.ToString() + "%"; }
            else { damageNumMod.text = ""; }
        }
        else if (index == 1)
        {
            if (mode == "Green") { defenseNumMod.color = Color.green; defenseNumMod.text = " +" + value.ToString() + "%"; }
            else if (mode == "Red") { defenseNumMod.color = Color.red; defenseNumMod.text =  " " + value.ToString() + "%"; }
            else { defenseNumMod.text = ""; }
        }
        else if (index == 2)
        {
            if (mode == "Green") { regenNumMod.color = Color.green; regenNumMod.text = " +" + value.ToString() + "%"; }
            else if (mode == "Red") { regenNumMod.color = Color.red; regenNumMod.text = " " + value.ToString() + "%"; }
            else { regenNumMod.text = ""; }
        }
        else if (index == 3)
        {
            if (mode == "Green") { attackCooldownNumMod.color = Color.green; attackCooldownNumMod.text = " +" + value.ToString() + "%"; }
            else if (mode == "Red") { attackCooldownNumMod.color = Color.red; attackCooldownNumMod.text = " " + value.ToString() + "%"; }
            else { attackCooldownNumMod.text = ""; }
        }
        else if (index == 4)
        {
            if (mode == "Green") { specialCooldownNumMod.color = Color.green; specialCooldownNumMod.text = " +" + value.ToString() + "%"; }
            else if (mode == "Red") { specialCooldownNumMod.color = Color.red; specialCooldownNumMod.text = " " + value.ToString() + "%"; }
            else { specialCooldownNumMod.text = ""; }
        }
        else if (index == 5)
        {
            if (mode == "Green") { switchCooldownNumMod.color = Color.green; switchCooldownNumMod.text = " +" + value.ToString() + "%"; }
            else if (mode == "Red") { switchCooldownNumMod.color = Color.red; switchCooldownNumMod.text = " " + value.ToString() + "%"; }
            else { switchCooldownNumMod.text = ""; }
        }
    }

    public void ClosePanel()
    {
        itemEquipMenu.Hide();
        panel.SetActive(false);
        
    }

    public void DestroyMonster()
    {
        g.collectionManager.ClearMonster(storedObject);
    }

    public void UpdateTitle(string txt)
    {
        titleText.text = txt;
    }


    private float GetHSVColourValue(Color color)
    {
        float value = 0;
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        value = h;
        return value;
    }




    public void OpenSelectItemWindow(int num, bool rightSide)
    {
        List<StoredItem> equippableItems = new List<StoredItem>();

        for (int i = 0; i < g.itemsOwned.Count; i++)
        {
            if (g.itemsOwned[i].item.type == ItemType.Catalyst)
            {
                equippableItems.Add(new StoredItem(g.itemsOwned[i].item, g.itemsOwned[i].amount));
            }
        }

        itemEquipMenu.Show(this, rightSide);
        itemEquipMenu.Refresh(equippableItems, g.monsterType, g.monsterNumInStorage, g);

    }

    public void UpdateInspectPanel()
    {
        List<StoredItem> itms = new List<StoredItem>();

        for (int i = 0; i < g.itemsOwned.Count; i++)
        {
            if (g.itemsOwned[i].item.type == ItemType.Catalyst)
            {
                itms.Add(g.itemsOwned[i]);
            }
        }

        itemEquipMenu.Refresh(itms, g.monsterType, g.monsterNumInStorage, g);

        if (g.monsterType == "Party")
        {
            UpdatePanel(g.collectionManager.partySlots[g.monsterNumInStorage].storedMonsterObject.GetComponent<PartySlot>().storedMonster, g);
        }
        else if (g.monsterType == "Collection")
        {
            UpdatePanel(g.collectionManager.collectionMonsters[g.monsterNumInStorage].GetComponent<CollectionSlot>().storedMonster, g);
        }

        //manager.SaveData();
    }
}
