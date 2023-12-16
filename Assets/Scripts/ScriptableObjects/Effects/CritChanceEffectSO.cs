using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CritChanceEffect", menuName = "SO/Effects/CritChance")]
public class CritChanceEffectSO : EffectSO
{
    [Header("Crit Chance")]
    public int amount = 1;

    public void Awake()
    {
        effectType = EffectType.CritChance;
    }
}
