using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBuffManager : MonoBehaviour
{
    [Header("Buff Management")]
    public GameObject buffSlotPrefab;
    public Transform locationParent;

    public GameManager GM;



    public List<BuffSlot> slots = new List<BuffSlot>();

    public List<int> slotValues = new List<int>();

    public PerfectGuardEffects perfectGuardEffect = PerfectGuardEffects.None;
    public float perfectGuardValue = 0;
    public Targets perfectGuardTargets;

    public bool isFriendly = false;

    private float tickTimer = 0f;
    private bool tickActive = true;

    private List<MonsterItemSO> items = new List<MonsterItemSO>();
    private PassiveSO currentMonPassive;
    private List<PassiveSO> sharedPassives = new List<PassiveSO>();

    private bool passivesOn = false;
    void Update()
    {
        if (passivesOn)
        {
            for (int i = 0; i < sharedPassives.Count; i++)
            {
                // CHECK ALL PASSIVES HERE

            }
        }

        if (tickActive)
        {
            if (tickTimer > 0)
            {
                tickTimer -= Time.deltaTime;
            }
            else if (tickTimer <= 0)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    if (slots[i].active)
                    {
                        if (slots[i].slotType == 6) // dot
                        {
                            if (isFriendly)
                            {
                                GM.battleManager.friendlyMonsterController.TakeDamage(slotValues[6], true, false);
                            }
                            else
                            {
                                GM.battleManager.enemyMonsterController.TakeDamage(slotValues[6], true, false);
                            }
                        }

                        

                    }
                }


                tickTimer = 1f;
            }
        }
        


        
    }


    

    public void StartPassives(List<PassiveSO> ps)
    {

    }


    public float GetStatsFromItemsPassives(EffectedStat effectedStat) // only gets always stats, for calc
    {
        float val = 0;

        if (currentMonPassive != null)
        {
            for (int i = 0; i < currentMonPassive.passiveActions.Count; i++)
            {
                if (currentMonPassive.passiveActions[i].effect.effectType == EffectType.StatMod && currentMonPassive.passiveActions[i].conditions.always)
                {
                    StatModEffectSO effect = currentMonPassive.passiveActions[i].effect as StatModEffectSO;

                    if (effect.stat == effectedStat)
                    {
                        val += effect.amount;
                    }
                }
            }
        }
        

        if (sharedPassives.Count > 0)
        {
            for (int i = 0; i < sharedPassives.Count; i++)
            {
                for (int j = 0; j < sharedPassives[i].passiveActions.Count; j++)
                {
                    if (sharedPassives[i].passiveActions[j].effect.effectType == EffectType.StatMod && sharedPassives[i].passiveActions[j].conditions.always)
                    {
                        StatModEffectSO effect = sharedPassives[i].passiveActions[j].effect as StatModEffectSO;

                        if (effect.stat == effectedStat)
                        {
                            val += effect.amount;
                        }
                    }
                }
            }
        }
        

        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < items[i].itemEffects.Count; j++)
                {
                    if (items[i].itemEffects[j].effect.effectType == EffectType.StatMod && items[i].itemEffects[j].conditions.always)
                    {
                        StatModEffectSO effect = items[i].itemEffects[j].effect as StatModEffectSO;

                        if (effect.stat == effectedStat)
                        {
                            val += effect.amount;
                        }
                    }
                }
            }
        }
        

        return val;
    }
    
    


    public void AddBuff(float time, int type) // doing crits buff / for taking crits
    {
        if (type == 1) // CritAttacks
        {
            SpawnBuffObject(9, time);
        }
        else if (type == 2) // TakingCrits
        {
            SpawnBuffObject(10, time);
        }
        else if (type == 3) // stun
        {
            SpawnBuffObject(7, time);
        }
        else if (type == 4) // CritChance
        {
            SpawnBuffObject(11, (int)time);
        }
        else if (type == 5) // Low Grav
        {
            SpawnBuffObject(12, time);
        }
    }



    public void AddBuff(int amount, float time) // dot
    {
        SpawnBuffObject(6, amount, time);
    }

    public void AddBuff(float time, PerfectGuardEffects effect, float effectValue, Targets team) // for invulnurability and perfect guard effects
    {
        perfectGuardEffect = effect;
        perfectGuardValue = effectValue;
        perfectGuardTargets = team;
        SpawnBuffObject(8, time);
    }

    public void AddBuff(EffectedStat stat, int amount, Targets targets) // for buffing, debuffing, inversing etc any stat type
    {
        if (stat == EffectedStat.Oomph)
        {
            SpawnBuffObject(0, amount);
        }
        else if (stat == EffectedStat.Guts)
        {
            SpawnBuffObject(1, amount);
        }
        else if (stat == EffectedStat.Juice)
        {
            SpawnBuffObject(2, amount);
        }
        else if (stat == EffectedStat.Edge)
        {
            SpawnBuffObject(3, amount);
        }
        else if (stat == EffectedStat.Wits)
        {
            SpawnBuffObject(4, amount);
        }
        else if (stat == EffectedStat.Spark)
        {
            SpawnBuffObject(5, amount);
        }
    }

    public void RemoveBuff(EffectedStat stat, int amount, Targets targets)
    {
        if (stat == EffectedStat.Oomph)
        {
            RemoveBuffObject(0, amount);
        }
        else if (stat == EffectedStat.Guts)
        {
            RemoveBuffObject(1, amount);
        }
        else if (stat == EffectedStat.Juice)
        {
            RemoveBuffObject(2, amount);
        }
        else if (stat == EffectedStat.Edge)
        {
            RemoveBuffObject(3, amount);
        }
        else if (stat == EffectedStat.Wits)
        {
            RemoveBuffObject(4, amount);
        }
        else if (stat == EffectedStat.Spark)
        {
            RemoveBuffObject(5, amount);
        }
    }



    private void SpawnBuffObject(int type, int amount, float time) // dot
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].slotType == type)
            {
                slotValues[type] += amount;
                slots[i].Merge(amount, time);
                return;
            }
        }

        slotValues[type] += amount;
        GameObject obj = Instantiate(buffSlotPrefab, locationParent);
        obj.GetComponent<BuffSlot>().Init(type, amount, time, this);
        slots.Add(obj.GetComponent<BuffSlot>());
    }

    private void SpawnBuffObject(int type, float time) // crit attacks, taking crits, stun, guard, low grav
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].slotType == type)
            {
                slots[i].Merge(time);
                return;
            }
        }

        GameObject obj = Instantiate(buffSlotPrefab, locationParent);
        obj.GetComponent<BuffSlot>().Init(type, time, this);
        slots.Add(obj.GetComponent<BuffSlot>());
    }

    private void SpawnBuffObject(int type, int amount) // stats
    {
        //int extra = (int)(GM.battleManager.friendlyMonsterController.ItemStat(st) + GM.battleManager.friendlyMonsterController.PassiveStat(st));


        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].slotType == type)
            {
                slotValues[type] += amount;
                slots[i].MergeAmount(slotValues[type]);
                return;
            }
        }

        slotValues[type] += amount;
        GameObject obj = Instantiate(buffSlotPrefab, locationParent);
        obj.GetComponent<BuffSlot>().Init(type, amount, this);
        slots.Add(obj.GetComponent<BuffSlot>());
    }

    private void RemoveBuffObject(int type, int amount) // stats
    {

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].slotType == type)
            {
                slotValues[type] -= amount;
                slots[i].MergeAmount(slotValues[type]);
                return;
            }
        }

        slotValues[type] -= amount;
        GameObject obj = Instantiate(buffSlotPrefab, locationParent);
        obj.GetComponent<BuffSlot>().Init(type, amount, this);
        slots.Add(obj.GetComponent<BuffSlot>());
    }



    public void ClearSlotsAfterBattle()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            //Debug.Log("hellllo " + i);
            Destroy(slots[i].gameObject);
        }

        slots = new List<BuffSlot>();

        for (int i = 0; i < slotValues.Count; i++)
        {
            slotValues[i] = 0;
        }

        perfectGuardEffect = PerfectGuardEffects.None;
        perfectGuardValue = 0;
        
        Targets ts = new Targets(false, false, false);
        perfectGuardTargets = ts;

        if (isFriendly)
        {
            GM.battleManager.friendlyMonsterController.SetLowGravity(8f, 15f);
            GM.battleManager.friendlyMonsterController.Stun(false, 0f);
            GM.battleManager.friendlyMonsterController.Guard(false, PerfectGuardEffects.None, 0f, ts);
            GM.battleManager.friendlyMonsterController.CritAttacks(false);
            GM.battleManager.friendlyMonsterController.TakingCrits(false);
        }
        else
        {
            GM.battleManager.enemyMonsterController.SetLowGravity(8f, 15f);
            GM.battleManager.enemyMonsterController.Stun(false, 0f);
            GM.battleManager.enemyMonsterController.Guard(false, PerfectGuardEffects.None, 0f, ts);
            GM.battleManager.enemyMonsterController.CritAttacks(false);
            GM.battleManager.enemyMonsterController.TakingCrits(false);
        }

    }
}

public enum BuffSlotType
{
    Oomph,
    Guts,
    Juice,
    Edge,
    Wits,
    Spark,
    DoT,
    Stun,
    Guard,
    CritAttacks,
    TakingCrits,
    CritChance,
}

