using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    // TAKES IN PASSIVE MOVES AND MAKES THEM STAY ACTIVE

    // TAKES IN BASIC AND SPECIAL MOVES EFFECTS AND ACTIVATES THEM / APPLIES THE BUFFS, DEBUFFS

    public GameManager GM;

    public bool friendlyController = false;
    public BattleBuffManager manager;

    public void UseMove(MoveSO move) // use basic or special
    {
        foreach (Move m in move.moveActions)
        {
            if (PassConditionTest(m.conditions))
            {
                if (m.effect.effectType == EffectType.CritAttacks)
                {
                    CritAttacksEffectSO newEffect = m.effect as CritAttacksEffectSO;
                    DoCritAttacks(newEffect.time, 1, m.targets);
                }
                else if (m.effect.effectType == EffectType.DoT)
                {
                    DoTEffectSO newEffect = m.effect as DoTEffectSO;
                    DoDoT(newEffect.amount, newEffect.time, m.targets);
                }
                else if (m.effect.effectType == EffectType.FireProjectile)
                {
                    //Debug.Log("Fired Projectile");
                    FireProjectileEffectSO newEffect = m.effect as FireProjectileEffectSO;
                    DoProjectile(newEffect.projectilePrefab, newEffect.projectileDamage, newEffect.projectileSpeed, newEffect.lifetime, newEffect.collideWithAmountOfObjects, newEffect.criticalProjectile, m.targets);
                }
                else if (m.effect.effectType == EffectType.Heal)
                {
                    HealEffectSO newEffect = m.effect as HealEffectSO;
                    DoHeal(newEffect.healAmount, m.targets);
                }
                else if (m.effect.effectType == EffectType.Invulnerability)
                {
                    InvulnerabilityEffectSO newEffect = m.effect as InvulnerabilityEffectSO;
                    DoInvulnerability(newEffect.invulnerableTime, newEffect.perfectGuardEffect, newEffect.perfectGuardEffectValue, m.targets);
                }
                else if (m.effect.effectType == EffectType.RefreshCooldown)
                {
                    RefreshCooldownEffectSO newEffect = m.effect as RefreshCooldownEffectSO;
                    DoRefreshCooldown(newEffect.chance, newEffect.whatToRefresh, newEffect.amount, m.targets);
                }
                else if (m.effect.effectType == EffectType.StatMod)
                {
                    StatModEffectSO newEffect = m.effect as StatModEffectSO;
                    DoStatMod(newEffect.stat, newEffect.amount, m.targets);
                }
                else if (m.effect.effectType == EffectType.Stun)
                {
                    StunEffectSO newEffect = m.effect as StunEffectSO;
                    DoStun(newEffect.stunTime, 3, m.targets);
                }
                else if (m.effect.effectType == EffectType.TakingCrits)
                {
                    TakingCritsEffectSO newEffect = m.effect as TakingCritsEffectSO;
                    DoTakingCrits(newEffect.time, 2, m.targets);
                }
                else if (m.effect.effectType == EffectType.TimeBomb)
                {
                    TimeBombEffectSO newEffect = m.effect as TimeBombEffectSO;
                    DoTimeBomb(newEffect.damage, newEffect.breaksOnDamage, newEffect.decayTime, newEffect.decayAmount);
                }
                else if (m.effect.effectType == EffectType.CritChance)
                {
                    CritChanceEffectSO newEffect = m.effect as CritChanceEffectSO;
                    DoCritChance(newEffect.amount, m.targets);
                }
            }
        }
    }

    public void DoCritChance(float amount, Targets targets)
    {
        if (friendlyController) // came from player
        {
            if (targets.enemy)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(amount, 4);
            }

            if (targets.self)
            {
                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(amount, 4);
            }

            if (targets.team)
            {
                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(amount, 4);
            }
        }
        else // came from ai
        {
            if (targets.enemy)
            {
                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(amount, 4);
            }

            if (targets.self)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(amount, 4);
            }

            if (targets.team)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(amount, 4);
            }
        }
    }

    public void DoCritAttacks(float time, int type, Targets targets)
    {
        // Give crit attack buff to friend/enemy for amount, time, decay

        if (friendlyController)
        {
            if (targets.enemy)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(time, type);
            }

            if (targets.self)
            {
                manager.AddBuff(time, type);
            }

            if (targets.team)
            {
                manager.AddBuff(time, type);
            }
        }
        else
        {
            if (targets.enemy)
            {
                manager.AddBuff(time, type);
            }

            if (targets.self)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(time, type);
            }

            if (targets.team)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(time, type);
            }
        }
    }

    public void DoDoT(int amount, float time, Targets targets)
    {
        if (friendlyController)
        {
            if (targets.enemy)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(amount, time);
            }

            if (targets.self)
            {
                manager.AddBuff(amount, time);
            }

            if (targets.team)
            {
                manager.AddBuff(amount, time);
            }
        }
        else
        {
            if (targets.enemy)
            {
                manager.AddBuff(amount, time);
            }

            if (targets.self)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(amount, time);
            }

            if (targets.team)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(amount, time);
            }
        }


        
    }

    public void DoProjectile(GameObject projectile, int damage, float speed, float lifeTime, int collideWithAmountOfObjects, bool criticalProjectile, Targets targets)
    {
        if (friendlyController)
        {
            // GM.battleManager.friendlyMonsterController.
            // float gutsAmount = guts + (guts * ((ItemStat(EffectedStat.Guts) + friendlyBattleBuffManager.slotValues[1] + PassiveStat(EffectedStat.Guts)) / 100f));
            float oomphAmount = 0f;
            if (GM.battleManager.friendlyMonsterController.oomph < 0) // less than 0, negatives
            {
                oomphAmount = GM.battleManager.friendlyMonsterController.oomph + (GM.battleManager.friendlyMonsterController.oomph * ((GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Oomph) + GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.slotValues[0]) / -100f));
            }
            else if (GM.battleManager.friendlyMonsterController.oomph >= 0) // 0 or more, positives
            {
                oomphAmount = GM.battleManager.friendlyMonsterController.oomph + (GM.battleManager.friendlyMonsterController.oomph * ((GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Oomph) + GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.slotValues[0]) / 100f));
            }


            //spd = speed + (speed * (0.02f * amt));

            //Debug.Log("Base Damage: " + damage.ToString());

            float tD = damage + (damage * (0.016f * oomphAmount));

            //Debug.Log("Before Crit Damage: " + tD.ToString());

            int realDmg = (int)tD;
            float fltDmg = tD;
            bool state = false;

            if (GM.battleManager.friendlyMonsterController.airCrit)
            {
                realDmg = (int)(fltDmg + (fltDmg * 0.5f));
                state = true;
            }

            //Debug.Log("Real Damage: " + realDmg.ToString());

            GM.battleManager.friendlyMonsterController.FireProjectile(projectile, speed * 10, realDmg, lifeTime, collideWithAmountOfObjects, criticalProjectile, state);
        }
        else
        {
            float oomphAmount = 0f;
            if (GM.battleManager.friendlyMonsterController.oomph < 0) // less than 0, negatives
            {
                oomphAmount = GM.battleManager.enemyMonsterController.oomph + (GM.battleManager.enemyMonsterController.oomph * ((GM.battleManager.enemyMonsterController.enemyBattleBuffManager.slotValues[0]) / -100f));
            }
            else if (GM.battleManager.friendlyMonsterController.oomph >= 0) // 0 or more, positives
            {
                oomphAmount = GM.battleManager.enemyMonsterController.oomph + (GM.battleManager.enemyMonsterController.oomph * ((GM.battleManager.enemyMonsterController.enemyBattleBuffManager.slotValues[0]) / 100f));
            }

            //spd = speed + (speed * (0.02f * amt));

            float tD = damage + (damage * (0.016f * oomphAmount));

            int realDmg = (int)tD;
            float fltDmg = tD;
            bool state = false;
            if (GM.battleManager.enemyMonsterController.airCrit)
            {
                realDmg = (int)(fltDmg + (fltDmg * 0.5f));
                state = true;
            }

            // ENEMY SPEED MODIFIER - CHANGE THIS TO ALTER THE SLIGHT SLOWER SPEED OF ALL ENEMY PROJECTILES COMPARED TO THE PLAYERS 
            float enemyProjectileSpeedModifier = 0.6f; // 0-1   0%-100%


            GM.battleManager.enemyMonsterController.FireProjectile(projectile, (speed * 10) * enemyProjectileSpeedModifier, realDmg, lifeTime, collideWithAmountOfObjects, criticalProjectile, state);
        }
    }

    public void DoHeal(int amount, Targets targets)
    {
        if (friendlyController)
        {
            if (targets.enemy)
            {
                GM.battleManager.enemyMonsterController.Heal(amount);
            }

            if (targets.self)
            {
                GM.battleManager.friendlyMonsterController.Heal(amount);
            }

            if (targets.team)
            {
                GM.battleManager.friendlyMonsterController.Heal(amount);
            }
        }
        else
        {
            if (targets.enemy)
            {
                GM.battleManager.friendlyMonsterController.Heal(amount);
            }

            if (targets.self)
            {
                GM.battleManager.enemyMonsterController.Heal(amount);
            }

            if (targets.team)
            {
                GM.battleManager.enemyMonsterController.Heal(amount);
            }
        }
    }

    public void DoInvulnerability(float time, PerfectGuardEffects effect, float effectValue, Targets targets)
    {
        if (friendlyController)
        {
            if (targets.self)
            {
                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(time, effect, effectValue, targets);
            }

            if (targets.team)
            {
                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(time, effect, effectValue, targets);
            }

            if (targets.enemy)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(time, effect, effectValue, targets);
            }
            
        }
        else
        {
            GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(time, effect, effectValue, targets);
        }
    }

    public void DoRefreshCooldown(float chance, AbilityType whatToRefresh, int amount, Targets targets)
    {
        if (friendlyController)
        {
            //DO REFRESH COOLDOWN HERE
        }
        else
        {
            //DO REFRESH COOLDOWN HERE
        }
    }

    public void DoStatMod(EffectedStat stat, int amount, Targets targets)
    {

        manager.AddBuff(stat, amount, targets);
    }

    public void DoStun(float time, int type, Targets targets)
    {
        if (friendlyController)
        {
            if (GM.battleManager.friendlyMonsterController.guardOn)
            {
                Targets ts = new Targets(false, false, false);
                GM.battleManager.friendlyMonsterController.Guard(false, PerfectGuardEffects.None, 0f, ts);
            }
            else
            {
                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(time, type);
            }
            
        }
        else
        {
            if (GM.battleManager.enemyMonsterController.guardOn)
            {
                Targets ts = new Targets(false, false, false);
                GM.battleManager.enemyMonsterController.Guard(false, PerfectGuardEffects.None, 0f, ts);
            }
            else
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(time, type);
            }

        }
    }

    public void DoTakingCrits(float time, int type, Targets targets)
    {
        if (friendlyController)
        {
            if (targets.enemy)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(time, type);
            }

            if (targets.self)
            {
                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(time, type);
            }

            if (targets.team)
            {
                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(time, type);
            }
        }
        else
        {
            if (targets.enemy)
            {
                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(time, type);
            }

            if (targets.self)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(time, type);
            }

            if (targets.team)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(time, type);
            }
        }
    }

    public void DoTimeBomb(int amount, bool breaksOnDamage, float time, int decay)
    {
        //DISABLED ATM ONLY ACTIVE ON PROJECTILES THEMSELVES
        Debug.Log("A move should not contain this type of move action");
    }
    private bool PassConditionTest(Conditions conditions)
    {
        bool state = false;

        if (friendlyController)
        {
            if (conditions.whenUsed == true)
            {
                state = true;
            }
            else
            {
                state = false;
            }

  

            if (conditions.whenNotInAir && GM.battleManager.friendlyMonsterController.isGrounded) // Not in air and grounded
            {
                state = true;
            }
            else if (conditions.whenInAir && !GM.battleManager.friendlyMonsterController.isGrounded) // In air and not grounded
            {
                state = true;
            }
            else
            {
                state = false;
            }

            if (conditions.whenFullHP)
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
            else if (conditions.whenBelow15HP)
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
        }
        else if (!friendlyController)
        {
            if (conditions.whenUsed == true)
            {
                state = true;
            }
            else
            {
                state = false;
            }

            if (conditions.whenNotInAir && GM.battleManager.enemyMonsterController.isGrounded) // Not in air and grounded
            {
                state = true;
            }
            else if (conditions.whenInAir && !GM.battleManager.enemyMonsterController.isGrounded) // In air and not grounded
            {
                state = true;
            }
            else
            {
                state = false;
            }

            if (conditions.whenFullHP)
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
            else if (conditions.whenBelow15HP)
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

        }

        


        return state;
    }
}
