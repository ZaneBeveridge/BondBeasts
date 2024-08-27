using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpecialPauseTip : PauseTip
{
    [Header("Special Move Tip References")]

    public Sprite typeProjectileSprite;
    public Sprite typeInstantSprite;
    public Sprite typeGuardSprite;

    public TextMeshProUGUI specialDescriptionText;
    public TextMeshProUGUI specialCooldownText;
    public TextMeshProUGUI specialTypeText;
    public Image specialTypeIcon;

    public override void OpenTip()
    {
        base.OpenTip();

        Monster mon = bManager.friendlyMonsterController.friendlyMonster;

        float witsAmount = 0f;
        witsAmount = bManager.friendlyMonsterController.wits + bManager.friendlyMonsterController.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Wits) + bManager.friendlyMonsterController.friendlyBattleBuffManager.slotValues[4];

        float oomphAmount = 0f;
        oomphAmount = bManager.friendlyMonsterController.oomph + bManager.friendlyMonsterController.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Oomph) + bManager.friendlyMonsterController.friendlyBattleBuffManager.slotValues[0];


        if (mon.specialMove.editableMoveDescription.Contains("[damage]") & mon.specialMove.editableMoveDescription != "")
        {
            FireProjectileEffectSO effect = mon.specialMove.moveActions[0].effect as FireProjectileEffectSO;
            string personlizedString = mon.specialMove.editableMoveDescription.Replace("[damage]", (effect.projectileDamage + (effect.projectileDamage * (0.04f * oomphAmount))).ToString());

            if (personlizedString.Contains("[damage2]"))
            {
                FireProjectileEffectSO effect2 = mon.specialMove.moveActions[1].effect as FireProjectileEffectSO;
                personlizedString = personlizedString.Replace("[damage2]", (effect2.projectileDamage + (effect2.projectileDamage * (0.04f * oomphAmount))).ToString());
            }

            specialDescriptionText.text = personlizedString;
        }
        else
        {
            specialDescriptionText.text = mon.specialMove.moveDescription;
        }

        specialTypeText.text = mon.specialMove.moveType;

        if (mon.specialMove.moveType == "Guard")
        {
            specialTypeIcon.sprite = typeGuardSprite;
        }
        else if (mon.specialMove.moveType == "Instant")
        {
            specialTypeIcon.sprite = typeInstantSprite;
        }
        else
        {
            specialTypeIcon.sprite = typeProjectileSprite;
        }

        if (witsAmount > 100)
        {
            witsAmount = 100;
        }

        float specialCool = mon.specialMove.baseCooldown - (mon.specialMove.baseCooldown * (0.04f * witsAmount));
        if (specialCool < mon.specialMove.minCooldown)
        {
            specialCool = mon.specialMove.baseCooldown;
        }


        specialCooldownText.text = specialCool.ToString("F1");
    }
}
