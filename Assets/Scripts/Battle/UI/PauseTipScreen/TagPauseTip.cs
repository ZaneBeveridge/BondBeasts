using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class TagPauseTip : PauseTip
{
    [Header("Pause Tip References")]
    public int tagNum = 0;

    public Sprite typeProjectileSprite;
    public Sprite typeInstantSprite;
    public Sprite typeGuardSprite;

    public Color greenText;
    public Color redText;
    public Color normalText;

    [Header("Main Area")]

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpMinMaxText;
    public Slider xpSlider;
    public TextMeshProUGUI passiveText;

    public List<TextMeshProUGUI> statTexts = new List<TextMeshProUGUI>();
    public TextMeshProUGUI personalityText;
    public GameObject strangeObject;

    public GameObject equipItemPrefab;

    public List<Transform> equipItemLocation = new List<Transform>();

    [Header("Special")]
    public TextMeshProUGUI specialEffectText;
    public TextMeshProUGUI specialTypeText;
    public TextMeshProUGUI specialCooldownText;
    public Image specialTypeImage;

    [Header("Basic")]
    public TextMeshProUGUI basicEffectText;
    public TextMeshProUGUI basicTypeText;
    public TextMeshProUGUI basicCooldownText;
    public Image basicTypeImage;


    private List<GameObject> itms = new List<GameObject>();
    public override void OpenTip()
    {
        if (bManager.GM.collectionManager.partySlots[tagNum].storedMonsterObject == null) { return; }

        base.OpenTip();

        Monster mon = bManager.GM.collectionManager.partySlots[tagNum].storedMonsterObject.GetComponent<PartySlot>().storedMonster;

        // MAIN PANEL
        nameText.text = mon.name;
        levelText.text = "Level " + mon.level.ToString();
        
        float xpReq = 100;
        for (int j = 0; j < mon.level - 1; j++) { xpReq = xpReq * 1.2f; }
        int xpMax = Mathf.RoundToInt(xpReq);

        int xpSliderAmount = mon.xp;
        if (xpSliderAmount > xpMax)
        {
            xpSliderAmount = xpMax;
        }
        xpSlider.value = xpSliderAmount / xpReq;
        xpMinMaxText.text = mon.xp.ToString() + "/" + xpMax.ToString() + " XP";

        passiveText.text = mon.passiveMove.moveDescription;

        int oomphAmount = 0;
        int edgeAmount = 0;
        int witsAmount = 0;
        for (int i = 0; i < statTexts.Count; i++) // FOR NOW JUST DOES BASE STATS + NATURE / DOESN'T INCLUDE CURRENT BUFFS + ITEMS + PASSIVES BUT CAN IMPLEMENT THIS LATER
        {
            int val = 0;
            if (mon.stats[i].value < 0)
            {
                if (mon.nature.addedStats[i].value < 1) val = Mathf.RoundToInt(-(-mon.stats[i].value * (mon.nature.addedStats[i].value + 1)));
                else val = Mathf.RoundToInt(-(-mon.stats[i].value * (mon.nature.addedStats[i].value - 1)));
            }
            else
            {
                val = Mathf.RoundToInt(mon.stats[i].value * mon.nature.addedStats[i].value);
            }


            if (bManager.friendlyMonsterController.currentSlot == tagNum)
            {
                int itemPassivesAmount = 0;

                switch (i)
                {
                    case 0:
                        itemPassivesAmount = (int)bManager.friendlyMonsterController.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Oomph);
                        break;
                    case 1:
                        itemPassivesAmount = (int)bManager.friendlyMonsterController.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Guts);
                        break;
                    case 2:
                        itemPassivesAmount = (int)bManager.friendlyMonsterController.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Juice);
                        break;
                    case 3:
                        itemPassivesAmount = (int)bManager.friendlyMonsterController.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Edge);
                        break;
                    case 4:
                        itemPassivesAmount = (int)bManager.friendlyMonsterController.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Wits);
                        break;
                    case 5:
                        itemPassivesAmount = (int)bManager.friendlyMonsterController.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Spark);
                        break;
                }

                val = val + bManager.friendlyMonsterController.friendlyBattleBuffManager.slotValues[i] + itemPassivesAmount;
            }

            Color color = normalText;

            if (val > mon.stats[i].value) // Green
            {
                color = greenText;
            }
            else if (val < mon.stats[i].value) // red
            {
                color = redText;
            }

            statTexts[i].color = color;
            statTexts[i].text = val.ToString();

            if (i == 0)
            {
                oomphAmount = val;
            }
            else if (i == 3)
            {
                edgeAmount = val;
            }
            else if (i == 4)
            {
                witsAmount = val;
            }
        }

        

        if (mon.item1.id != 0)
        {
            GameObject item = Instantiate(equipItemPrefab, equipItemLocation[0]);
            item.GetComponent<TeamEquipSlot>().Init(mon.item1, bManager.GM);
            itms.Add(item);
        }

        if (mon.item2.id != 0)
        {
            GameObject item = Instantiate(equipItemPrefab, equipItemLocation[1]);
            item.GetComponent<TeamEquipSlot>().Init(mon.item2, bManager.GM);
            itms.Add(item);
        }

        if (mon.item3.id != 0)
        {
            GameObject item = Instantiate(equipItemPrefab, equipItemLocation[2]);
            item.GetComponent<TeamEquipSlot>().Init(mon.item3, bManager.GM);
            itms.Add(item);
        }

        personalityText.text = mon.nature.natureName;

        if (mon.strange)
        {
            strangeObject.SetActive(true);
        }
        else
        {
            strangeObject.SetActive(false);
        }


        //SPECIAL
        if (mon.specialMove.editableMoveDescription.Contains("[damage]") & mon.specialMove.editableMoveDescription != "")
        {
            FireProjectileEffectSO effect = mon.specialMove.moveActions[0].effect as FireProjectileEffectSO;
            string personlizedString = mon.specialMove.editableMoveDescription.Replace("[damage]", (effect.projectileDamage + (effect.projectileDamage * (0.016 * oomphAmount))).ToString("F2"));

            if (personlizedString.Contains("[damage2]"))
            {
                FireProjectileEffectSO effect2 = mon.specialMove.moveActions[1].effect as FireProjectileEffectSO;
                personlizedString = personlizedString.Replace("[damage2]", (effect2.projectileDamage + (effect2.projectileDamage * (0.016 * oomphAmount))).ToString("F2"));
            }

            specialEffectText.text = personlizedString;
        }
        else
        {
            specialEffectText.text = mon.specialMove.moveDescription;
        }

        specialTypeText.text = mon.specialMove.moveType;

        if (mon.specialMove.moveType == "Guard")
        {
            specialTypeImage.sprite = typeGuardSprite;
        }
        else if (mon.specialMove.moveType == "Instant")
        {
            specialTypeImage.sprite = typeInstantSprite;
        }
        else
        {
            specialTypeImage.sprite = typeProjectileSprite;
        }

        if (witsAmount > 100)
        {
            witsAmount = 100;
        }

        float specialCool = mon.specialMove.baseCooldown - (mon.specialMove.baseCooldown * (0.008f * witsAmount));
        if (specialCool < mon.specialMove.minCooldown)
        {
            specialCool = mon.specialMove.baseCooldown;
        }


        specialCooldownText.text = specialCool.ToString("F1");
        //BASIC

        if (mon.basicMove.editableMoveDescription.Contains("[damage]") & mon.basicMove.editableMoveDescription != "")
        {
            FireProjectileEffectSO effect = mon.basicMove.moveActions[0].effect as FireProjectileEffectSO;
            string personlizedString = mon.basicMove.editableMoveDescription.Replace("[damage]", (effect.projectileDamage + (effect.projectileDamage * (0.016 * oomphAmount))).ToString("F2"));

            if (personlizedString.Contains("[damage2]"))
            {
                FireProjectileEffectSO effect2 = mon.basicMove.moveActions[1].effect as FireProjectileEffectSO;
                personlizedString = personlizedString.Replace("[damage2]", (effect2.projectileDamage + (effect2.projectileDamage * (0.016 * oomphAmount))).ToString("F2"));
            }

            basicEffectText.text = personlizedString;
        }
        else
        {
            basicEffectText.text = mon.basicMove.moveDescription;
        }

        basicTypeText.text = mon.basicMove.moveType;

        if (mon.basicMove.moveType == "Guard")
        {
            basicTypeImage.sprite = typeGuardSprite;
        }
        else if (mon.basicMove.moveType == "Instant")
        {
            basicTypeImage.sprite = typeInstantSprite;
        }
        else
        {
            basicTypeImage.sprite = typeProjectileSprite;
        }

        if (edgeAmount > 100)
        {
            edgeAmount = 100;
        }

        float basicCool = mon.basicMove.baseCooldown - (mon.basicMove.baseCooldown * (0.008f * edgeAmount));
        if (basicCool < mon.basicMove.minCooldown)
        {
            basicCool = mon.basicMove.baseCooldown;
        }


        basicCooldownText.text = basicCool.ToString("F1");
    }
    public override void CloseTip()
    {
        base.CloseTip();

        for (int i = 0; i < itms.Count; i++)
        {
            Destroy(itms[i]);
        }
        itms = new List<GameObject>();
    }

    
}
