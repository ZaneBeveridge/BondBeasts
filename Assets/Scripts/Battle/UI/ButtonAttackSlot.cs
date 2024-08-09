using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonAttackSlot : MonoBehaviour
{
    [Range(0f, 1f)]public float opacity;

    public GameManager GM;
    public FriendlyMonsterController controller;

    public TextMeshProUGUI num;

    public Image attackButton;

    public Transform fillObject1;

    public GameObject flashMask;
    public Image flaskMaskImage;
    public Animator anim;

    

    public bool isBasic = true;

    private bool readyDone = false;
    private bool trueReady = false;
    private float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (readyDone)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
            else if (timer <= 0f)
            {
                flashMask.SetActive(false);
                timer = 0f;
                readyDone = false;
            }
        }



        float value1 = 0f;

        if (isBasic)
        {
            value1 = controller.edge + controller.friendlyBattleBuffManager.slotValues[3] + controller.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Edge);
        }
        else
        {
            value1 = controller.wits + controller.friendlyBattleBuffManager.slotValues[4] + controller.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Wits);
        }

        if (isBasic)
        {
            if (!controller.basicReady[controller.currentSlot])
            {
                float f = GM.battleManager.friendlyMonsterController.basicC[GM.battleManager.friendlyMonsterController.currentSlot];
                attackButton.color = new Color(0.25f, 0.25f, 0.25f);
                num.text = f.ToString("F1") + "s";
                trueReady = true;
            }
            else
            {
                attackButton.color = new Color(1f, 1f, 1f);
                num.text = "";
                Ready();
            }
        }
        else
        {
            if (!controller.specialReady[controller.currentSlot])
            {
                float f = GM.battleManager.friendlyMonsterController.specialC[GM.battleManager.friendlyMonsterController.currentSlot];
                attackButton.color = new Color(0.25f, 0.25f, 0.25f);
                num.text = f.ToString("F1") + "s";
                trueReady = true;
            }
            else
            {
                attackButton.color = new Color(1f, 1f, 1f);
                num.text = "";
                Ready();
            }
        }


        float val1 = 0f;
        float val2 = 1f;

        if (isBasic)
        {
            if (!controller.basicReady[controller.currentSlot])
            {
                val1 = controller.basicC[controller.currentSlot];
                float baseCD = controller.friendlyMonster.basicMove.baseCooldown;
                val2 = 1f / ((1f / baseCD) * value1 * 0.04f + (1f / baseCD));
            }
        }
        else
        {
            if (!controller.specialReady[controller.currentSlot])
            {
                val1 = controller.specialC[controller.currentSlot];

                float baseCD = controller.friendlyMonster.specialMove.baseCooldown;
                val2 = 1f / ((1f / baseCD) * value1 * 0.04f + (1f / baseCD));
            }
        }


        if (val2 <= 0)
        {
            val2 = 1;
        }
        

        fillObject1.localScale = new Vector3(1f, val1 / val2, 1f);


    }

    public void Ready()
    {
        if (!readyDone && trueReady)
        {
            timer = 0.15f;
            readyDone = true;
            trueReady = false;
            flashMask.SetActive(true);
            flaskMaskImage.color = new Color(255f, 255f, 255f, opacity);
            anim.SetTrigger("Ready");
        }
    }
}
