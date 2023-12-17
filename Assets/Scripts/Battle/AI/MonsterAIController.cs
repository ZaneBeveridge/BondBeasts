using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAIController : MonoBehaviour
{
    [Header("Battle Controller")]
    public EnemyMonsterController controller;

    [Header("Action Data")]
    public ActionDataSO actionData;

    //[Header("Tick Rate")]
    //public float tickTime = 1f;

    private float timer;
    private bool active = false;
    private bool isPunkBattle = false;

    void Update()
    {
        if (active)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                Act();
                timer = actionData.tickRate;
            }
        }
    }

    public void Act()
    {
        string move = actionData.GetMove(isPunkBattle);

        if (move == "") return; // return

        //Debug.Log("Doing Action: " + move);

        if (move == "Pause") // return
        {
            return;
        }
        else if (move == "Attack") // if attack on cooldown return, else do attack
        {
            controller.Attack();
        }
        else if (move == "Special") // if special on cooldown return, else do special
        {
            controller.Special();
        }
        else if (move == "HighJump") // if jump on cooldown return, else do high jump
        {
            controller.Jump(0.25f);
            Debug.Log("High");
        }
        else if (move == "MediumJump") // if jump on cooldown return, else do medium jump
        {
            controller.Jump(0.15f);
            Debug.Log("Med");
        }
        else if (move == "LowJump") // if jump on cooldown return, else do low jump
        {
            controller.Jump(0.05f);
            Debug.Log("Low");
        }
        else if (move == "Swap") // if all swaps on cooldown return, else do swap IF in punk fight mode
        {
            // TO IMPLEMENT WITH PUNK BATTLES
            controller.GM.battleManager.SwitchEnemy();
        }

    }

    public void SetActive(bool state)
    {
        if (state == true)
        {
            timer = actionData.tickRate;
            active = true;
        }
        else if (state == false)
        {
            timer = 0f;
            active = false;
        }
    }

    public void SetBattleType(bool isPunk)
    {
        isPunkBattle = isPunk;
    }
}
