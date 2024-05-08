using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BasicPauseTip : PauseTip
{
    [Header("Basic Move Tip References")]

    public Sprite typeProjectileSprite;
    public Sprite typeInstantSprite;
    public Sprite typeGuardSprite;

    public TextMeshProUGUI basicDescriptionText;
    public TextMeshProUGUI basicCooldownText;
    public TextMeshProUGUI basicTypeText;
    public Image basicTypeIcon;

    public override void OpenTip()
    {
        base.OpenTip();

        Monster mon = bManager.friendlyMonsterController.friendlyMonster;
        //basicNameText.text = bManager.friendlyMonsterController.friendlyMonster.basicMove.moveName;

        float edgeAmount = 0f;
        edgeAmount = bManager.friendlyMonsterController.edge + bManager.friendlyMonsterController.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Edge) + bManager.friendlyMonsterController.friendlyBattleBuffManager.slotValues[3];

        float oomphAmount = 0f;
        oomphAmount = bManager.friendlyMonsterController.oomph + bManager.friendlyMonsterController.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Oomph) + bManager.friendlyMonsterController.friendlyBattleBuffManager.slotValues[0];


        if (mon.basicMove.editableMoveDescription.Contains("[damage]") & mon.basicMove.editableMoveDescription != "")
        {
            FireProjectileEffectSO effect = mon.basicMove.moveActions[0].effect as FireProjectileEffectSO;
            string personlizedString = mon.basicMove.editableMoveDescription.Replace("[damage]", (effect.projectileDamage + (effect.projectileDamage * (0.016 * oomphAmount))).ToString("F2"));

            if (personlizedString.Contains("[damage2]"))
            {
                FireProjectileEffectSO effect2 = mon.basicMove.moveActions[1].effect as FireProjectileEffectSO;
                personlizedString = personlizedString.Replace("[damage2]", (effect2.projectileDamage + (effect2.projectileDamage * (0.016 * oomphAmount))).ToString("F2"));
            }

            basicDescriptionText.text = personlizedString;
        }
        else
        {
            basicDescriptionText.text = mon.basicMove.moveDescription;
        }

        basicTypeText.text = mon.basicMove.moveType;

        if (mon.basicMove.moveType == "Guard")
        {
            basicTypeIcon.sprite = typeGuardSprite;
        }
        else if (mon.basicMove.moveType == "Instant")
        {
            basicTypeIcon.sprite = typeInstantSprite;
        }
        else
        {
            basicTypeIcon.sprite = typeProjectileSprite;
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
}
