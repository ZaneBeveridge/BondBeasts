using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    public FriendlyMonsterController controller;

    public TextMeshProUGUI passiveText;

    public TextMeshProUGUI specialText;
    public TextMeshProUGUI specialTextCooldown;

    public TextMeshProUGUI basicText;
    public TextMeshProUGUI basicTextCooldown;

    public List<PauseTip> tips = new List<PauseTip>();


    public void UpdateText()
    {
        passiveText.text = controller.friendlyMonster.passiveMove.moveDescription;

        specialText.text = controller.friendlyMonster.specialMove.moveDescription;


        float valueAmountB = controller.edge + (controller.edge * (controller.friendlyBattleBuffManager.slotValues[3] / 100));
        float valueRealB = valueAmountB;


        if (valueAmountB > 100)
        {
            valueRealB = 100f;

        }

        float valueAmountS = controller.wits + (controller.wits * (controller.friendlyBattleBuffManager.slotValues[4] / 100));
        float valueRealS = valueAmountS;


        if ((int)valueAmountS > 100)
        {
            valueRealS = 100f;

        }
        

        float cooldownBasic = Mathf.Round(controller.friendlyMonster.basicMove.baseCooldown - (controller.friendlyMonster.basicMove.baseCooldown * (0.04f * valueRealB)));
        float cooldownSpecial = Mathf.Round(controller.friendlyMonster.specialMove.baseCooldown - (controller.friendlyMonster.specialMove.baseCooldown * (0.04f * valueRealS)));

        

        int printB = (int)cooldownBasic;
        int printS = (int)cooldownSpecial;

       


        specialTextCooldown.text = printB.ToString() + "s";

        basicText.text = controller.friendlyMonster.basicMove.moveDescription;
        basicTextCooldown.text = printS.ToString() + "s";
    }


    public void CloseAllTips()
    {
        for (int i = 0; i < tips.Count; i++)
        {
            tips[i].CloseTip();
        }
    }

    
}
