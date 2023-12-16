using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    [Header("TYPE")]
    public EffectType effectType;
}

public enum EffectType
{
    FireProjectile,
    StatMod,
    Invulnerability,
    Heal,
    Stun,
    TimeBomb,
    CritAttacks,
    TakingCrits,
    DoT,
    RefreshCooldown,
    CritChance
}
