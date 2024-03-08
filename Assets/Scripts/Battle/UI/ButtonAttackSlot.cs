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
        float value2 = 0f;
        float value3 = 0f;

        if (isBasic)
        {
            value1 = controller.edge + controller.friendlyBattleBuffManager.slotValues[3] + controller.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Edge);
        }
        else
        {
            value1 = controller.wits + controller.friendlyBattleBuffManager.slotValues[4] + controller.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Wits);
        }


        if (value1 > 100f)
        {
            if (value1 > 200f) // 201-300
            {
                value3 = value1 - 200f;
                value2 = 100f;
                value1 = 100f;

                if (isBasic)
                {
                    if (!controller.basicReady[controller.currentSlot] && !controller.basicReady2[controller.currentSlot] && !controller.basicReady3[controller.currentSlot])
                    {
                        attackButton.color = new Color(0.25f, 0.25f, 0.25f);

                        float f = GM.battleManager.friendlyMonsterController.basicC3[GM.battleManager.friendlyMonsterController.currentSlot];
                        f = Mathf.Round(f * 10.0f) * 0.1f;
                        num.text = f.ToString() + "s";
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
                    if (!controller.specialReady[controller.currentSlot] && !controller.specialReady2[controller.currentSlot] && !controller.specialReady3[controller.currentSlot])
                    {
                        attackButton.color = new Color(0.25f, 0.25f, 0.25f);

                        float f = GM.battleManager.friendlyMonsterController.specialC3[GM.battleManager.friendlyMonsterController.currentSlot];
                        f = Mathf.Round(f * 10.0f) * 0.1f;
                        num.text = f.ToString() + "s";
                        trueReady = true;
                    }
                    else
                    {
                        attackButton.color = new Color(1f, 1f, 1f);
                        num.text = "";
                        Ready();
                    }
                }

                
            }
            else // 101-200
            {
                value2 = value1 - 100f;
                value1 = 100f;

                if (isBasic)
                {
                    if (!controller.basicReady[controller.currentSlot] && !controller.basicReady2[controller.currentSlot])
                    {
                        float f = GM.battleManager.friendlyMonsterController.basicC2[GM.battleManager.friendlyMonsterController.currentSlot];
                        attackButton.color = new Color(0.25f, 0.25f, 0.25f);
                        f = Mathf.Round(f * 10.0f) * 0.1f;
                        num.text = f.ToString() + "s";
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
                    if (!controller.specialReady[controller.currentSlot] && !controller.specialReady2[controller.currentSlot])
                    {
                        float f = GM.battleManager.friendlyMonsterController.specialC2[GM.battleManager.friendlyMonsterController.currentSlot];
                        attackButton.color = new Color(0.25f, 0.25f, 0.25f);
                        f = Mathf.Round(f * 10.0f) * 0.1f;
                        num.text = f.ToString() + "s";
                        trueReady = true;
                    }
                    else
                    {
                        attackButton.color = new Color(1f, 1f, 1f);
                        num.text = "";
                        Ready();
                    }
                }

                
            }
        }
        else // 0-100
        {
            if (isBasic)
            {
                if (!controller.basicReady[controller.currentSlot])
                {
                    float f = GM.battleManager.friendlyMonsterController.basicC[GM.battleManager.friendlyMonsterController.currentSlot];
                    attackButton.color = new Color(0.25f, 0.25f, 0.25f);
                    f = Mathf.Round(f * 10.0f) * 0.1f;
                    num.text = f.ToString() + "s";
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
                    f = Mathf.Round(f * 10.0f) * 0.1f;
                    num.text = f.ToString() + "s";
                    trueReady = true;
                }
                else
                {
                    attackButton.color = new Color(1f, 1f, 1f);
                    num.text = "";
                    Ready();
                }
            }
            

        }


        float val1 = 0f;
        float val2 = 1f;

        if (isBasic)
        {
            if (!controller.basicReady[controller.currentSlot])
            {
                val1 = controller.basicC[controller.currentSlot];
                val2 = controller.friendlyMonster.basicMove.baseCooldown - (controller.friendlyMonster.basicMove.baseCooldown * (0.008f * value1));
            }
            else
            {
                if (!controller.basicReady2[controller.currentSlot] && value2 > 0)
                {
                    val1 = controller.basicC2[controller.currentSlot];
                    val2 = controller.friendlyMonster.basicMove.baseCooldown - (controller.friendlyMonster.basicMove.baseCooldown * (0.008f * value2));
                }
                else
                {
                    if (!controller.basicReady3[controller.currentSlot] && value3 > 0)
                    {
                        val1 = controller.basicC3[controller.currentSlot];
                        val2 = controller.friendlyMonster.basicMove.baseCooldown - (controller.friendlyMonster.basicMove.baseCooldown * (0.008f * value3));
                    }
                }
            }
        }
        else
        {
            if (!controller.specialReady[controller.currentSlot])
            {
                val1 = controller.specialC[controller.currentSlot];
                val2 = controller.friendlyMonster.specialMove.baseCooldown - (controller.friendlyMonster.specialMove.baseCooldown * (0.008f * value1));
            }
            else
            {
                if (!controller.specialReady2[controller.currentSlot] && value2 > 0)
                {
                    val1 = controller.specialC2[controller.currentSlot];
                    val2 = controller.friendlyMonster.specialMove.baseCooldown - (controller.friendlyMonster.specialMove.baseCooldown * (0.008f * value2));
                }
                else
                {
                    if (!controller.specialReady3[controller.currentSlot] && value3 > 0)
                    {
                        val1 = controller.specialC3[controller.currentSlot];
                        val2 = controller.friendlyMonster.specialMove.baseCooldown - (controller.friendlyMonster.specialMove.baseCooldown * (0.008f * value3));
                    }
                }
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
