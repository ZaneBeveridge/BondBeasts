using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public BattleBuffManager manager;
    public GameManager GM;
    public MoveController moveController;
    public Transform itemConditionParticleSpawnLocation;

    private List<MonsterItemSO> items = new List<MonsterItemSO>();
    private List<ItemUsesInBattle> itemUses = new List<ItemUsesInBattle>();

    public bool itemsOn = false;

    private List<ItemEffect> activePropertyEffects = new List<ItemEffect>();
    private List<bool> activePropertyEffectsOn = new List<bool>();


    private List<DelayedEffectItem> delayedEffects = new List<DelayedEffectItem>();
    private List<float> delayedEffectsTime = new List<float>();

    private int currentSelectedMonsterID = 0;

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
                        UseEffect(delayedEffects[i].effect, delayedEffects[i].targets, delayedEffects[i].particle);

                        delayedEffects.RemoveAt(i);
                        delayedEffectsTime.RemoveAt(i);
                    }
                }
            }


        }
    }

    
    public void StartItems(List<MonsterItemSO> it, int currentSlot) // Start of passives each time a monster switches
    {
        currentSelectedMonsterID = currentSlot;
        ClearLastMonsterEffectsForAlways();
        ClearActivePropertyEffects();

        items = it;

        bool isSpace = false;

        if (itemUses.Count > 0)
        {
            for (int i = 0; i < itemUses.Count; i++)
            {
                if (itemUses[i].monstersID == currentSlot)
                {
                    break;
                }

                isSpace = true;
            }

            if (isSpace)
            {
                itemUses.Add(new ItemUsesInBattle(it, currentSlot));
            }
            
        }
        else
        {
            itemUses.Add(new ItemUsesInBattle(it, currentSlot));
        }

        

        


        for (int i = 0; i < items.Count; i++)
        {
            foreach (ItemEffect itemE in items[i].itemEffects)
            {
                if (itemE.conditions.whileInAir || itemE.conditions.whileNotInAir || itemE.conditions.whileFullHP || itemE.conditions.whileBelow150HP || itemE.conditions.whileEnemyFullHP || itemE.conditions.whileEnemyBelow150HP)
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
            ActivateItemTrigger(items[i], triggerType, i);
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
                /*
                if (item.effect.effectType == EffectType.Heal)
                {
                    HealEffectSO newEffect = item.effect as HealEffectSO;
                    moveController.DoHeal(newEffect.healAmount, item.targets);
                }
                */
                if (item.particleToTrigger != null)
                {
                    Instantiate(item.particleToTrigger, itemConditionParticleSpawnLocation.position, Quaternion.identity, itemConditionParticleSpawnLocation);
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

    private void UseEffect(EffectSO effect, Targets targets, GameObject particle)
    {
        if (particle != null)
        {
            Instantiate(particle, itemConditionParticleSpawnLocation.position, Quaternion.identity, itemConditionParticleSpawnLocation);
        }

        if (effect.effectType == EffectType.StatMod)
        {
            StatModEffectSO newEffect = effect as StatModEffectSO;
            manager.AddBuff(newEffect.stat, newEffect.amount, targets);
        }
        else if (effect.effectType == EffectType.Heal)
        {
            HealEffectSO newEffect = effect as HealEffectSO;
            moveController.DoHeal(newEffect.healAmount, targets);
        }
        else if (effect.effectType == EffectType.FireProjectile)
        {
            FireProjectileEffectSO newEffect = effect as FireProjectileEffectSO;
            moveController.DoProjectile(newEffect.projectilePrefab, newEffect.projectileDamage, newEffect.projectileSpeed, newEffect.lifetime, newEffect.collideWithAmountOfObjects, newEffect.criticalProjectile, newEffect);
        }
        else if (effect.effectType == EffectType.Invulnerability)
        {
            InvulnerabilityEffectSO newEffect = effect as InvulnerabilityEffectSO;
            moveController.DoInvulnerability(newEffect.invulnerableTime, newEffect.perfectGuardEffect, newEffect.perfectGuardEffectValue, newEffect.projectile, targets);
        }
        else if (effect.effectType == EffectType.RefreshCooldown)
        {
            RefreshCooldownEffectSO newEffect = effect as RefreshCooldownEffectSO;
            moveController.DoRefreshCooldown(newEffect.chance, newEffect.whatToRefresh, targets);
        }
        else if (effect.effectType == EffectType.Stun)
        {
            StunEffectSO newEffect = effect as StunEffectSO;
            moveController.DoStun(newEffect.stunTime, 3, targets);
        }
        else if (effect.effectType == EffectType.DoT)
        {
            DoTEffectSO newEffect = effect as DoTEffectSO;
            moveController.DoDoT(newEffect.amount, newEffect.time, targets);
        }
        else if (effect.effectType == EffectType.CritChance)
        {
            CritChanceEffectSO newEffect = effect as CritChanceEffectSO;
            moveController.DoCritChance(newEffect.amount, targets);
        }
        else if (effect.effectType == EffectType.LowGravity)
        {
            LowGravityEffectSO newEffect = effect as LowGravityEffectSO;
            moveController.DoLowGrav(newEffect.time, targets);
        }
        else if (effect.effectType == EffectType.SetHealth)
        {
            SetHealthEffectSO newEffect = effect as SetHealthEffectSO;
            moveController.DoSetHealth(newEffect.healAmount, targets);
        }
    }

    private void ActivateItemTrigger(MonsterItemSO item, TriggerType triggerType, int itemID) // for start 0 == negative, 1 == positive
    {
        for (int i = 0; i < item.itemEffects.Count; i++)
        {
            if (!PassConditionTestAlways(item.itemEffects[i].conditions))
            {
                if (PassConditionTest(item.itemEffects[i].conditions) && PassTriggerTest(item.itemEffects[i].conditions, triggerType))
                {
                    //Debug.Log("Trigger:" + triggerType.ToString());

                    if (item.itemEffects[i].maxUsesPerBattle > 0)
                    {
                        for (int j = 0; j < itemUses.Count; j++)
                        {
                            if (itemUses[j].monstersID == currentSelectedMonsterID)
                            {
                                if (itemUses[j].ListOfList[itemID].effectUses[i] > 0)
                                {
                                    itemUses[j].ListOfList[itemID].effectUses[i]--;
                                    if (item.itemEffects[i].delay > 0)
                                    {
                                        delayedEffects.Add(new DelayedEffectItem(item.itemEffects[i].effect, item.itemEffects[i].targets, item.itemEffects[i].particleToTrigger));
                                        delayedEffectsTime.Add(item.itemEffects[i].delay);
                                    }
                                    else
                                    {
                                        UseEffect(item.itemEffects[i].effect, item.itemEffects[i].targets, item.itemEffects[i].particleToTrigger);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (item.itemEffects[i].delay > 0)
                        {
                            delayedEffects.Add(new DelayedEffectItem(item.itemEffects[i].effect, item.itemEffects[i].targets, item.itemEffects[i].particleToTrigger));
                            delayedEffectsTime.Add(item.itemEffects[i].delay);
                        }
                        else
                        {
                            UseEffect(item.itemEffects[i].effect, item.itemEffects[i].targets, item.itemEffects[i].particleToTrigger);
                        }
                    }
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
            if (GM.playerHP >= 1000)
            {
                state = true;
            }
            else
            {
                state = false;
            }
        }

        if (conditions.whileBelow150HP)
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

        if (conditions.whileEnemyFullHP)
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

        if (conditions.whileEnemyBelow150HP)
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

        return state;

    }

    public void ClearItems()
    {
        itemsOn = false;
        items = new List<MonsterItemSO>();
        itemUses = new List<ItemUsesInBattle>();
    }

}

[System.Serializable]
public class ItemUsesInBattle
{
    public int monstersID;
    public List<ListOfEffectUses> ListOfList = new List<ListOfEffectUses>();

    public ItemUsesInBattle(List<MonsterItemSO> its, int id)
    {
        monstersID = id;

        for (int i = 0; i < its.Count; i++)
        {
            ListOfList.Add(new ListOfEffectUses(its[i]));
        }
    }
}

[System.Serializable]
public class ListOfEffectUses
{
    public List<int> effectUses = new List<int>();

    public ListOfEffectUses(MonsterItemSO item)
    {
        for (int i = 0; i < item.itemEffects.Count; i++)
        {
            effectUses.Add(item.itemEffects[i].maxUsesPerBattle);
        }
    }
}


public class DelayedEffectItem
{
    public EffectSO effect;
    public Targets targets;
    public GameObject particle;

    public DelayedEffectItem(EffectSO e, Targets t, GameObject p)
    {
        effect = e;
        targets = t;
        particle = p;
    }
}


