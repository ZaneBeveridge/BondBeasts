using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public BattleBuffManager manager;
    public GameManager GM;
    public MoveController moveController;

    private List<MonsterItemSO> items = new List<MonsterItemSO>();

    public bool itemsOn = false;

    private List<ItemEffect> activePropertyEffects = new List<ItemEffect>();
    private List<bool> activePropertyEffectsOn = new List<bool>();

    void Update()
    {
        if (itemsOn)
        {
            for (int i = 0; i < activePropertyEffects.Count; i++)
            {
                // CHECK ALL PASSIVES HERE

                if (!activePropertyEffectsOn[i]) // if off
                {
                    ActivateItemProperty(activePropertyEffects[i], 1, i); // check to turn on, if pass condition check do so
                    
                }
                else if (activePropertyEffectsOn[i]) // if on
                {
                    ActivateItemProperty(activePropertyEffects[i], 0, i); // check to turn off, if does not pass condition check do so
                }


            }
        }
    }

    
    public void StartItems(List<MonsterItemSO> it) // Start of passives each time a monster switches
    {
        ClearLastMonsterEffectsForAlways();
        ClearActivePropertyEffects();

        items = it;

        

        for (int i = 0; i < items.Count; i++)
        {
            foreach (ItemEffect itemE in items[i].itemEffects)
            {
                if (itemE.conditions.whileInAir || itemE.conditions.whileNotInAir || itemE.conditions.whileFullHP || itemE.conditions.whileBelow15HP || itemE.conditions.whileEnemyFullHP || itemE.conditions.whileEnemyBelow15HP)
                {
                    activePropertyEffects.Add(itemE);
                    activePropertyEffectsOn.Add(false);
                }
            }
        }


        CheckAllEffectsForAlways();

        itemsOn = true;
    }

    private void ClearActivePropertyEffects()
    {
        for (int i = 0; i < activePropertyEffects.Count; i++)
        {
            if (activePropertyEffectsOn[i])
            {
                ForceActivateItemProperty(activePropertyEffects[i]); // check to turn off
            }
        }

        activePropertyEffects = new List<ItemEffect>();
        activePropertyEffectsOn = new List<bool>();
    }

    private void ClearLastMonsterEffectsForAlways()
    {
        for (int i = 0; i < items.Count; i++)
        {
            ActivateItem(items[i], 0);
        }
    }

    private void CheckAllEffectsForAlways()
    {
        for (int i = 0; i < items.Count; i++)
        {
            ActivateItem(items[i], 1);
        }
    }

    public void ItemTrigger(TriggerType triggerType)
    {
        for (int i = 0; i < items.Count; i++)
        {
            //Debug.Log(items[i].itemName);
            ActivateItemTrigger(items[i], triggerType);
        }
    }
    private void ActivateItem(MonsterItemSO item, int id) // for start 0 == negative, 1 == positive
    {
        foreach (ItemEffect m in item.itemEffects)
        {
            if (PassConditionTestAlways(m.conditions))
            {
                if (m.effect.effectType == EffectType.StatMod)
                {
                    StatModEffectSO newEffect = m.effect as StatModEffectSO;

                    if (id == 0)
                    {
                        manager.RemoveBuff(newEffect.stat, newEffect.amount, m.targets);
                    }
                    else if (id == 1)
                    {
                        manager.AddBuff(newEffect.stat, newEffect.amount, m.targets);
                    }

                }
            }
        }

    }

    private void ActivateItemProperty(ItemEffect item, int activate, int id) // for start 0 == negative, 1 == positive
    {
        if (activate == 1) // turn on , was off
        {
            if (PassConditionTest(item.conditions))
            {
                if (item.effect.effectType == EffectType.StatMod)
                {
                    StatModEffectSO newEffect = item.effect as StatModEffectSO;
                    manager.AddBuff(newEffect.stat, newEffect.amount, item.targets);
                }

                activePropertyEffectsOn[id] = true;
            }
        }
        else if (activate == 0) // turn off, was on
        {
            if (!PassConditionTest(item.conditions))
            {
                if (item.effect.effectType == EffectType.StatMod)
                {
                    StatModEffectSO newEffect = item.effect as StatModEffectSO;
                    manager.RemoveBuff(newEffect.stat, newEffect.amount, item.targets);

                }

                activePropertyEffectsOn[id] = false;
            }
        }
    }

    private void ForceActivateItemProperty(ItemEffect item) // for start 0 == negative, 1 == positive
    {
        if (item.effect.effectType == EffectType.StatMod)
        {
            StatModEffectSO newEffect = item.effect as StatModEffectSO;
            manager.RemoveBuff(newEffect.stat, newEffect.amount, item.targets);

        }
    }

    private void ActivateItemTrigger(MonsterItemSO item, TriggerType triggerType) // for start 0 == negative, 1 == positive
    {
        foreach (ItemEffect m in item.itemEffects)
        {
            //Debug.Log(m.conditions.whenTagIn);
            //Debug.Log(PassConditionTest(m.conditions));
            //Debug.Log(PassTriggerTest(m.conditions, triggerType));
            if (PassConditionTest(m.conditions) && PassTriggerTest(m.conditions, triggerType))
            {
                //Debug.Log("test");

                if (m.effect.effectType == EffectType.StatMod)
                {
                    StatModEffectSO newEffect = m.effect as StatModEffectSO;
                    manager.AddBuff(newEffect.stat, newEffect.amount, m.targets);
                }
                else if (m.effect.effectType == EffectType.Heal)
                {
                    HealEffectSO newEffect = m.effect as HealEffectSO;
                    moveController.DoHeal(newEffect.healAmount, m.targets);
                }
                else if (m.effect.effectType == EffectType.FireProjectile)
                {
                    FireProjectileEffectSO newEffect = m.effect as FireProjectileEffectSO;
                    moveController.DoProjectile(newEffect.projectilePrefab, newEffect.projectileDamage, newEffect.projectileSpeed, newEffect.lifetime, newEffect.collideWithAmountOfObjects, newEffect.criticalProjectile, m.targets);
                }
                else if (m.effect.effectType == EffectType.Invulnerability)
                {
                    InvulnerabilityEffectSO newEffect = m.effect as InvulnerabilityEffectSO;
                    moveController.DoInvulnerability(newEffect.invulnerableTime, newEffect.perfectGuardEffect, newEffect.perfectGuardEffectValue, m.targets);
                }
                else if (m.effect.effectType == EffectType.RefreshCooldown)
                {
                    RefreshCooldownEffectSO newEffect = m.effect as RefreshCooldownEffectSO;
                    moveController.DoRefreshCooldown(newEffect.chance, newEffect.whatToRefresh, newEffect.amount, m.targets);
                }
                else if (m.effect.effectType == EffectType.Stun)
                {
                    StunEffectSO newEffect = m.effect as StunEffectSO;
                    moveController.DoStun(newEffect.stunTime, 3, m.targets);
                }
                else if (m.effect.effectType == EffectType.TakingCrits)
                {
                    TakingCritsEffectSO newEffect = m.effect as TakingCritsEffectSO;
                    moveController.DoTakingCrits(newEffect.time, 2, m.targets);
                }
                else if (m.effect.effectType == EffectType.CritAttacks)
                {
                    CritAttacksEffectSO newEffect = m.effect as CritAttacksEffectSO;
                    moveController.DoCritAttacks(newEffect.time, 1, m.targets);
                }
                else if (m.effect.effectType == EffectType.DoT)
                {
                    DoTEffectSO newEffect = m.effect as DoTEffectSO;
                    moveController.DoDoT(newEffect.amount, newEffect.time, m.targets);
                }



            }
        }

    }

    private bool PassTriggerTest(ItemConditions conditions, TriggerType tType)
    {
        bool state = true;

        //Debug.Log(conditions.whenTagIn);
        //Debug.Log(tType);

        if (tType == TriggerType.beingHit)
        {
            if (conditions.whenBeingHit) { state = true; }
            else { state = false; }
        }
        else if (tType == TriggerType.tagOut)
        {
            if (conditions.whenTagOut) { state = true; }
            else { state = false; }
        }
        else if (tType == TriggerType.tagIn)
        {
            if (conditions.whenTagIn) { state = true; }
            else { state = false; }
        }
        else if (tType == TriggerType.crit)
        {
            if (conditions.whenCrit) { state = true; }
            else { state = false; }
        }
        else if (tType == TriggerType.enemyStunned)
        {
            if (conditions.whenEnemyStunned) { state = true; }
            else { state = false; }
        }
        else if (tType == TriggerType.useBasic)
        {
            if (conditions.whenUseBasic) { state = true; }
            else { state = false; }
        }
        else if (tType == TriggerType.useSpecial)
        {
            if (conditions.whenUseSpecial) { state = true; }
            else { state = false; }
        }
        else if (tType == TriggerType.enemyHitBasic)
        {
            if (conditions.whenEnemyHitBasic) { state = true; }
            else { state = false; }
        }
        else if (tType == TriggerType.enemyHitSpecial)
        {
            if (conditions.whenEnemyHitSpecial) { state = true; }
            else { state = false; }
        }





        return state;
    }

    private bool PassConditionTestAlways(ItemConditions conditions)
    {
        bool state = false;

        if (conditions.always == true)
        {
            state = true;
            return state;
        }
        else
        {
            state = false;
        }

        

        return state;
    }

    private bool PassConditionTest(ItemConditions conditions)
    {
        bool state = true;

        if (conditions.whileNotInAir)
        {
            if (GM.battleManager.friendlyMonsterController.isGrounded) // Not in air and grounded
            {
                state = true;
            }
            else
            {
                state = false;
            }
        }

        if (conditions.whileInAir)
        {
            if (!GM.battleManager.friendlyMonsterController.isGrounded) // Not in air and grounded
            {
                state = true;
            }
            else
            {
                state = false;
            }
        }

        if (conditions.whileFullHP)
        {
            if (GM.playerHP >= 100)
            {
                state = true;
            }
            else
            {
                state = false;
            }
        }

        if (conditions.whileBelow15HP)
        {
            if (GM.playerHP < 15)
            {
                state = true;
            }
            else
            {
                state = false;
            }
        }

        if (conditions.whileEnemyFullHP)
        {
            if (GM.battleManager.enemyMonsterController.enemyHealth >= 100)
            {
                state = true;
            }
            else
            {
                state = false;
            }
        }

        if (conditions.whileEnemyBelow15HP)
        {
            if (GM.battleManager.enemyMonsterController.enemyHealth < 15)
            {
                state = true;
            }
            else
            {
                state = false;
            }
        }

        return state;

    }

    public void ClearItems()
    {
        itemsOn = false;
        items = new List<MonsterItemSO>();
    }
}
