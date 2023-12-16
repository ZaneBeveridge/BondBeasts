using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RefreshCooldownEffect", menuName = "SO/Effects/RefreshCooldown")]
public class RefreshCooldownEffectSO : EffectSO
{
    [Header("Refresh Cooldown")]
    [Range(0f, 100f)]public float chance = 100f;
    public AbilityType whatToRefresh;
    public int amount = 0;

    public void Awake()
    {
        effectType = EffectType.RefreshCooldown;
    }
}

public enum AbilityType
{
    Basic,
    Special,
    Tag
}
