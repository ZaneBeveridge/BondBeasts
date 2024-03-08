using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffSlot : MonoBehaviour
{
    public Image typeImage;
    public TextMeshProUGUI numText;

    public Color positiveColour;
    public Color negativeColour;

    public int slotType;
    public int timerAmount = 0;

    public List<Sprite> sprites = new List<Sprite>();

    public Animator anim;

    private float timer = 0;
    private bool timerOn = false;

    private BattleBuffManager manager;

    [HideInInspector] public bool active = false;

    void Update()
    {
        if (slotType == 6)
        {
            if (timerAmount > 0)
            {
                numText.text = "+" + timerAmount.ToString();
            }
            else if (timerAmount < 0)
            {
                numText.text = "-" + timerAmount.ToString();
            }
        }
       

        if (timerOn)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (timer <= 0)
            {
                TimerEnd();
            }
        }
    }

    private void DoBuffs(int slotNum, bool isFriend)
    {
        if (slotNum == 7)// stun
        {
            if (isFriend)
            {
                manager.GM.battleManager.friendlyMonsterController.Stun(true, timer);
            }
            else
            {
                manager.GM.battleManager.enemyMonsterController.Stun(true, timer);
            }

        }
        else if(slotNum == 8) // guard
        {
            if (isFriend)
            {
                if (manager.perfectGuardEffect == PerfectGuardEffects.None)
                {
                    manager.GM.battleManager.friendlyMonsterController.Guard(manager.perfectGuardEffect, manager.perfectGuardValue, manager.perfectGuardProjectile, manager.perfectGuardTargets, 0f);
                }
                else
                {
                    manager.GM.battleManager.friendlyMonsterController.Guard( manager.perfectGuardEffect, manager.perfectGuardValue, manager.perfectGuardProjectile, manager.perfectGuardTargets, 0.25f);
                }
                
            }
            else
            {
                if (manager.perfectGuardEffect == PerfectGuardEffects.None)
                {
                    manager.GM.battleManager.enemyMonsterController.Guard(manager.perfectGuardEffect, manager.perfectGuardValue, manager.perfectGuardProjectile, manager.perfectGuardTargets, 0f);
                }
                else
                {
                    manager.GM.battleManager.enemyMonsterController.Guard(manager.perfectGuardEffect, manager.perfectGuardValue, manager.perfectGuardProjectile, manager.perfectGuardTargets, 0.25f);
                }
                
            }
        }
        else if (slotNum == 12) // low grav
        {
            if (isFriend)
            {
                manager.GM.battleManager.friendlyMonsterController.SetLowGravity(1f, 8f);
            }
            else
            {
                manager.GM.battleManager.enemyMonsterController.SetLowGravity(1f, 8f);
            }
        }





    }

    public void Init(int type, int amount, float time, BattleBuffManager man) // dot
    {
        typeImage.gameObject.SetActive(false);
        numText.gameObject.SetActive(false);

        manager = man;
        slotType = type;
        timer = time;
        timerAmount = amount;

        DoBuffs(type, man.isFriendly);

        active = true;
        timerOn = true;

        UpdateText();
    }

    public void Init(int type, float time, BattleBuffManager man)// crit attacks, taking crits, critchance, stun, guard, low grav
    {
        typeImage.gameObject.SetActive(false);
        numText.gameObject.SetActive(false);


        if (type == 11)// critchance
        {
            timerAmount = (int)time;
            timer = 0f;


            

            manager = man;

            DoBuffs(type, man.isFriendly);
            slotType = type;
        }
        else
        {
            manager = man;
            slotType = type;
            timer = time;

            DoBuffs(type, man.isFriendly);

            timerOn = true;
        }

        UpdateText();
    }

    public void Init(int type, int amount, BattleBuffManager man) // stats
    {
        typeImage.gameObject.SetActive(true);
        numText.gameObject.SetActive(true);

        typeImage.sprite = sprites[type];

        if (amount < 0)
        {
            anim.SetTrigger("Neg");
        }
        else if (amount > 0)
        {
            anim.SetTrigger("Pos");
        }

        timerAmount = amount;


        UpdateText();

        manager = man;

        DoBuffs(type, man.isFriendly);
        slotType = type;
        
    }


    public void UpdateExtraStats() // Removes all extra stats, if total is now +0% DELETE THIS BUFF, else just remove extra stats and refresh new values
    {
        if (timerAmount == 0) // no normalStatsLeft
        {
            TimerEnd();
        }
        else // stats left
        {
            UpdateText();
        }    
    }

    public void Merge(int amount, float time)
    {
        timer += time;
        timerAmount += amount;


        UpdateText();
    }

    public void Merge(float time)
    {
        timer += time;

        UpdateText();
    }

    public void MergeAmount(int amount, int addedAmount) // for stats
    {
        if (addedAmount < 0)
        {
            anim.SetTrigger("Neg");
        }
        else if (addedAmount > 0)
        {
            anim.SetTrigger("Pos");
        }



        timerAmount = amount;

        if (timerAmount == 0)
        {
            TimerEnd();
        }

        UpdateText();
    }

    private void UpdateText()
    {
        if (timerAmount >= 0)
        {
            numText.text = "+" + timerAmount.ToString();
            numText.color = positiveColour;
        }
        else
        {
            numText.text = timerAmount.ToString();
            numText.color = negativeColour;
        }
    }    

    public void TimerEnd()
    {
        Targets ts = new Targets(false, false);

        if (slotType == 8)
        {
            manager.perfectGuardEffect = PerfectGuardEffects.None;
            manager.perfectGuardValue = 0;
            manager.perfectGuardProjectile = null;

            manager.perfectGuardTargets = ts;
        }

        if (slotType == 7)
        {
            if (manager.isFriendly)
            {
                manager.GM.battleManager.friendlyMonsterController.Stun(false, 0f);
            }
            else
            {
                manager.GM.battleManager.enemyMonsterController.Stun(false, 0f);
            }
        }

        if (slotType == 8)
        {
            if (manager.isFriendly)
            {
                manager.GM.battleManager.friendlyMonsterController.GuardOff();
            }
            else
            {
                manager.GM.battleManager.enemyMonsterController.GuardOff();
            }
        }

        if (slotType == 11) // crit chance
        {
            if (manager.isFriendly)
            {
                manager.GM.battleManager.friendlyMonsterController.SetCritChance(0);
            }
            else
            {
                manager.GM.battleManager.enemyMonsterController.SetCritChance(0);
            }
        }

        if (slotType == 12) // low grav
        {
            if (manager.isFriendly)
            {
                if (manager.GM.battleManager.friendlyMonsterController.friendlyMonster.backupData.isHeavyJumper)
                {
                    manager.GM.battleManager.friendlyMonsterController.SetLowGravity(8f, 10f);
                }
                else
                {
                    manager.GM.battleManager.friendlyMonsterController.SetLowGravity(8f, 15f);
                }

                
            }
            else
            {
                if (manager.GM.battleManager.enemyMonsterController.currentMonster.backupData.isHeavyJumper)
                {
                    manager.GM.battleManager.enemyMonsterController.SetLowGravity(8f, 10f);
                }
                else
                {
                    manager.GM.battleManager.enemyMonsterController.SetLowGravity(8f, 15f);
                }
                
            }
        }


        //Debug.Log("I am slot type: " + slotType + ", and my value is: " + timerAmount);

        active = false;
        manager.slotValues[slotType] = 0;
        manager.slots.Remove(this);
        
        Destroy(this.gameObject);
    }
}
