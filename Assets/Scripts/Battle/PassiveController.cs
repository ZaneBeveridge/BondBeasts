using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveController : MonoBehaviour
{
    public BattleBuffManager manager;
    public GameManager GM;
    public MoveController moveController;

    private PassiveSO currentMonPassive;
    private List<PassiveSO> sharedPassives = new List<PassiveSO>();

    public bool passivesOn = false;

    private List<Passive> activePropertyEffects = new List<Passive>();
    private List<bool> activePropertyEffectsOn = new List<bool>();
    void Update()
    {
        if (passivesOn)
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
    public void StartPassives(PassiveSO cMonPassive, List<PassiveSO> sPassives) // Start of passives each time a monster switches
    {
        ClearLastMonsterEffectsForAlways();
        ClearActivePropertyEffects();


        currentMonPassive = cMonPassive;
        sharedPassives = sPassives;

        for (int i = 0; i < sPassives.Count; i++)
        {
            if (sharedPassives[i].id != currentMonPassive.id)
            {
                foreach (Passive itemE in sPassives[i].passiveActions)
                {
                    if (itemE.conditions.whileInAir || itemE.conditions.whileNotInAir || itemE.conditions.whileFullHP || itemE.conditions.whileBelow15HP || itemE.conditions.whileEnemyFullHP || itemE.conditions.whileEnemyBelow15HP)
                    {
                        activePropertyEffects.Add(itemE);
                        activePropertyEffectsOn.Add(false);
                    }
                }
            }
        }

        foreach (Passive itemA in cMonPassive.passiveActions)
        {
            if (itemA.conditions.whileInAir || itemA.conditions.whileNotInAir || itemA.conditions.whileFullHP || itemA.conditions.whileBelow15HP || itemA.conditions.whileEnemyFullHP || itemA.conditions.whileEnemyBelow15HP)
            {
                activePropertyEffects.Add(itemA);
                activePropertyEffectsOn.Add(false);
            }
        }


        CheckAllEffectsForAlways();

        passivesOn = true;
    }

    private void ClearActivePropertyEffects()
    {
        for (int i = 0; i < activePropertyEffects.Count; i++)
        {
            if (activePropertyEffectsOn[i])
            {
                ForceActivatePassiveProperty(activePropertyEffects[i]); // check to turn off
            }
        }

        activePropertyEffects = new List<Passive>();
        activePropertyEffectsOn = new List<bool>();
    }

    private void ClearLastMonsterEffectsForAlways()
    {
        if (currentMonPassive != null)
        {
            ActivatePassive(currentMonPassive, 0);
        }

        for (int i = 0; i < sharedPassives.Count; i++)
        {
            if (sharedPassives[i].id != currentMonPassive.id)
            {
                ActivatePassive(sharedPassives[i], 0);
            }
            
        }
    }

    private void CheckAllEffectsForAlways()
    {
        if (currentMonPassive != null)
        {
            ActivatePassive(currentMonPassive, 1);
        }

        for (int i = 0; i < sharedPassives.Count; i++)
        {
            if (sharedPassives[i].id != currentMonPassive.id)
            {
                ActivatePassive(sharedPassives[i], 1);
            }
            
        }
    }

    public void PassiveTrigger(TriggerType triggerType)
    {
        if (currentMonPassive != null)
        {
            ActivatePassiveTrigger(currentMonPassive, triggerType);
        }

        for (int i = 0; i < sharedPassives.Count; i++)
        {
            if (sharedPassives[i].id != currentMonPassive.id)
            {
                ActivatePassiveTrigger(sharedPassives[i], triggerType);
            }
            
        }
    }

    private void ActivatePassive(PassiveSO passive, int id) // for start 0 == negative, 1 == positive
    {
        foreach (Passive m in passive.passiveActions)
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

    private void ActivateItemProperty(Passive item, int activate, int id) // for start 0 == negative, 1 == positive
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

    private void ForceActivatePassiveProperty(Passive item) // for start 0 == negative, 1 == positive
    {
        if (item.effect.effectType == EffectType.StatMod)
        {
            StatModEffectSO newEffect = item.effect as StatModEffectSO;
            manager.RemoveBuff(newEffect.stat, newEffect.amount, item.targets);

        }
    }

    private void ActivatePassiveTrigger(PassiveSO passive, TriggerType triggerType) // for start 0 == negative, 1 == positive
    {
        foreach (Passive m in passive.passiveActions)
        {
            if (PassConditionTest(m.conditions) && PassTriggerTest(m.conditions, triggerType))
            {
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
                else if (m.effect.effectType == EffectType.CritChance)
                {
                    CritChanceEffectSO newEffect = m.effect as CritChanceEffectSO;
                    moveController.DoCritChance(newEffect.amount, m.targets);
                }



            }
        }

    }

    private bool PassTriggerTest(PassiveConditions conditions, TriggerType tType)
    {
        bool state = false;
        switch (tType)
        {
            case TriggerType.beingHit:
                if (conditions.whenBeingHit)
                {
                    state = true;
                    return state;
                }
                break;
            case TriggerType.tagOut:
                if (conditions.whenTagOut)
                {
                    state = true;
                    return state;
                }
                break;
            case TriggerType.tagIn:
                if (conditions.whenTagIn)
                {
                    state = true;
                    return state;
                }
                break;
            case TriggerType.crit:
                if (conditions.whenCrit)
                {
                    state = true;
                    return state;
                }
                break;
            case TriggerType.enemyStunned:
                if (conditions.whenEnemyStunned)
                {
                    state = true;
                    return state;
                }
                break;
            case TriggerType.useBasic:
                if (conditions.whenUseBasic)
                {
                    state = true;
                    return state;
                }
                break;
            case TriggerType.useSpecial:
                if (conditions.whenUseSpecial)
                {
                    state = true;
                    return state;
                }
                break;
            case TriggerType.enemyHitBasic:
                if (conditions.whenEnemyHitBasic)
                {
                    state = true;
                    return state;
                }
                break;
            case TriggerType.enemyHitSpecial:
                if (conditions.whenEnemyHitSpecial)
                {
                    state = true;
                    return state;
                }
                break;
        }

        return state;
    }

    private bool PassConditionTestAlways(PassiveConditions conditions)
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
    
    private bool PassConditionTest(PassiveConditions conditions)
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

    public void ClearPassives()
    {
        passivesOn = false;
        currentMonPassive = null;
        sharedPassives = new List<PassiveSO>();
    }
}
