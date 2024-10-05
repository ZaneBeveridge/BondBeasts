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

    private List<DelayedEffectMove> delayedEffects = new List<DelayedEffectMove>();
    private List<float> delayedEffectsTime = new List<float>();
    public void Update()
    {
        if (delayedEffects.Count > 0)
        {
            for (int i = 0; i < delayedEffects.Count; i++)
            {
                if (delayedEffectsTime[i] > 0)
                {
                    delayedEffectsTime[i] -= Time.deltaTime;
                }
                else if (delayedEffectsTime[i] <= 0)
                {
                    UseEffect(delayedEffects[i].effect, delayedEffects[i].targets);

                    delayedEffects.RemoveAt(i);
                    delayedEffectsTime.RemoveAt(i);
                }
            }
        }
        
    }

    public void UseEffect(EffectSO effect, Targets targets)
    {
        if (effect.effectType == EffectType.DoT)
        {
            DoTEffectSO newEffect = effect as DoTEffectSO;
            DoDoT(newEffect.amount, newEffect.time, targets);
        }
        else if (effect.effectType == EffectType.FireProjectile)
        {
            //Debug.Log("Fired Projectile");
            FireProjectileEffectSO newEffect = effect as FireProjectileEffectSO;
            DoProjectile(newEffect.projectilePrefab, newEffect.projectileDamage, newEffect.projectileSpeed, newEffect.lifetime, newEffect.collideWithAmountOfObjects, newEffect.criticalProjectile, newEffect);
        }
        else if (effect.effectType == EffectType.Heal)
        {
            HealEffectSO newEffect = effect as HealEffectSO;
            DoHeal(newEffect.healAmount, targets);
        }
        else if (effect.effectType == EffectType.Invulnerability)
        {
            InvulnerabilityEffectSO newEffect = effect as InvulnerabilityEffectSO;
            DoInvulnerability(newEffect.invulnerableTime, newEffect.perfectGuardEffect, newEffect.perfectGuardEffectValue, newEffect.projectile, targets);
        }
        else if (effect.effectType == EffectType.RefreshCooldown)
        {
            RefreshCooldownEffectSO newEffect = effect as RefreshCooldownEffectSO;
            DoRefreshCooldown(newEffect.chance, newEffect.whatToRefresh, targets);
        }
        else if (effect.effectType == EffectType.StatMod)
        {
            StatModEffectSO newEffect = effect as StatModEffectSO;
            DoStatMod(newEffect.stat, newEffect.amount, targets);
        }
        else if (effect.effectType == EffectType.Stun)
        {
            StunEffectSO newEffect = effect as StunEffectSO;
            DoStun(newEffect.stunTime, 3, targets);
        }
        else if (effect.effectType == EffectType.CritChance)
        {
            CritChanceEffectSO newEffect = effect as CritChanceEffectSO;
            DoCritChance(newEffect.amount, targets);
        }
        else if (effect.effectType == EffectType.LowGravity)
        {
            LowGravityEffectSO newEffect = effect as LowGravityEffectSO;
            DoLowGrav(newEffect.time, targets);
        }
        else if (effect.effectType == EffectType.SetHealth)
        {
            SetHealthEffectSO newEffect = effect as SetHealthEffectSO;
            DoSetHealth(newEffect.healAmount, targets);
        }
    }

    public void UseMove(MoveSO move) // use basic or special
    {
        foreach (Move m in move.moveActions)
        {
            if (PassConditionTest(m.conditions))
            {
                if (m.delay > 0)
                {
                    delayedEffects.Add(new DelayedEffectMove(m.effect, m.targets));
                    delayedEffectsTime.Add(m.delay);
                }
                else
                {
                    UseEffect(m.effect, m.targets);
                }
            }
        }
    }

    public void DoLowGrav(float amount, Targets targets)
    {
        if (friendlyController) // came from player
        {
            if (targets.enemy)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(amount, 5);
            }

            if (targets.team)
            {
                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(amount, 5);
            }
        }
        else // came from ai
        {
            if (targets.enemy)
            {
                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(amount, 5);
            }

            if (targets.team)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(amount, 5);
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

            if (targets.team)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(amount, 4);
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

            if (targets.team)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(amount, time);
            }
        }


        
    }

    public void DoProjectile(GameObject projectile, int damage, float speed, float lifeTime, int collideWithAmountOfObjects, bool criticalProjectile, FireProjectileEffectSO projEffect)
    {
        if (friendlyController)
        {
            float oomphAmount = 0f;
            float itemPassives = GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Oomph);
            float buffSlots = GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.slotValues[0];

            oomphAmount = GM.battleManager.friendlyMonsterController.oomph + itemPassives + buffSlots;


            //spd = speed + (speed * (0.02f * amt));

            //Debug.Log("Base Damage: " + damage.ToString());

            float tD = damage + (damage * (0.04f * oomphAmount));

            //Debug.Log("Before Crit Damage: " + tD.ToString());  // FIX THIS FOR SOME REASON IF I REMOVE THE COMMENT IT STOP CALCULATING THE RIGHT AMOUNTS

            int realDmg = (int)tD;
            int baseDmg = (int)damage;

            //Debug.Log("Real Damage: " + realDmg.ToString());

            GM.battleManager.friendlyMonsterController.FireProjectile(projectile, speed * 10, realDmg, baseDmg, lifeTime, collideWithAmountOfObjects, criticalProjectile, projEffect);
        }
        else
        {
            float oomphAmount = 0f;
            float itemPassives = GM.battleManager.enemyMonsterController.enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Oomph);
            float buffSlots = GM.battleManager.enemyMonsterController.enemyBattleBuffManager.slotValues[0];

            oomphAmount = GM.battleManager.enemyMonsterController.oomph + itemPassives + buffSlots;

            //spd = speed + (speed * (0.02f * amt));
            float tD = damage + (damage * (0.04f * oomphAmount));

            int realDmg = (int)tD;
            int baseDmg = (int)damage;

            // ENEMY SPEED MODIFIER - CHANGE THIS TO ALTER THE SLIGHT SLOWER SPEED OF ALL ENEMY PROJECTILES COMPARED TO THE PLAYERS 
            float enemyProjectileSpeedModifier = 0.6f; // 0-1   0%-100%


            GM.battleManager.enemyMonsterController.FireProjectile(projectile, (speed * 10) * enemyProjectileSpeedModifier, realDmg, baseDmg, lifeTime, collideWithAmountOfObjects, criticalProjectile, projEffect);
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

            if (targets.team)
            {
                GM.battleManager.enemyMonsterController.Heal(amount);
            }
        }
    }

    public void DoSetHealth(int amount, Targets targets)
    {
        if (friendlyController)
        {
            if (targets.enemy)
            {
                GM.battleManager.enemyMonsterController.SetHealth(amount);
            }

            if (targets.team)
            {
                GM.battleManager.friendlyMonsterController.SetHealth(amount);
            }
        }
        else
        {
            if (targets.enemy)
            {
                GM.battleManager.friendlyMonsterController.SetHealth(amount);
            }

            if (targets.team)
            {
                GM.battleManager.enemyMonsterController.SetHealth(amount);
            }
        }
    }

    public void DoInvulnerability(float time, PerfectGuardEffects effect, float effectValue, FireProjectileEffectSO projectile, Targets targets)
    {
        if (friendlyController)
        {
            if (targets.team)
            {
                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(time, effect, effectValue, projectile, targets);
            }

            if (targets.enemy)
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(time, effect, effectValue, projectile, targets);
            }
            
        }
        else
        {
            GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(time, effect, effectValue, projectile, targets);
        }
    }

    public void DoRefreshCooldown(float chance, AbilityType whatToRefresh, Targets targets)
    {
        if (friendlyController)
        {
            if (targets.enemy)
            {
                if (whatToRefresh == AbilityType.Special)
                {
                    GM.battleManager.enemyMonsterController.specialReady[GM.battleManager.enemyMonsterController.currentSlot] = true;
                    GM.battleManager.enemyMonsterController.specialC[GM.battleManager.enemyMonsterController.currentSlot] = 0f;
                }
            }

            if (targets.team)
            {
                if (whatToRefresh == AbilityType.Special)
                {
                    GM.battleManager.friendlyMonsterController.specialReady[GM.battleManager.friendlyMonsterController.currentSlot] = true;
                    GM.battleManager.friendlyMonsterController.specialC[GM.battleManager.friendlyMonsterController.currentSlot] = 0f;
                }
            }
        }
        else
        {
            if (targets.enemy)
            {
                if (whatToRefresh == AbilityType.Special)
                {
                    GM.battleManager.friendlyMonsterController.specialReady[GM.battleManager.friendlyMonsterController.currentSlot] = true;
                    GM.battleManager.friendlyMonsterController.specialC[GM.battleManager.friendlyMonsterController.currentSlot] = 0f;
                }
            }

            if (targets.team)
            {
                if (whatToRefresh == AbilityType.Special)
                {
                    GM.battleManager.enemyMonsterController.specialReady[GM.battleManager.enemyMonsterController.currentSlot] = true;
                    GM.battleManager.enemyMonsterController.specialC[GM.battleManager.enemyMonsterController.currentSlot] = 0f;
                }
            }
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
                Targets ts = new Targets(false, false);
                GM.battleManager.friendlyMonsterController.GuardOff();
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
                Targets ts = new Targets(false, false);
                GM.battleManager.enemyMonsterController.GuardOff();
            }
            else
            {
                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(time, type);
            }

        }
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
                if (GM.playerHP >= 1000)
                {
                    state = true;
                }
                else
                {
                    state = false;
                }
            }
            else if (conditions.whenBelow150HP)
            {
                if (GM.playerHP < 150)
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
                if (GM.battleManager.enemyMonsterController.enemyHealth >= 1000)
                {
                    state = true;
                }
                else
                {
                    state = false;
                }
            }
            else if (conditions.whenBelow150HP)
            {
                if (GM.battleManager.enemyMonsterController.enemyHealth < 150)
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



public class DelayedEffectMove
{
    public EffectSO effect;
    public Targets targets;

    public DelayedEffectMove(EffectSO e, Targets t)
    {
        effect = e;
        targets = t;
    }
}
