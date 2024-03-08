using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CritAttacksEffect", menuName = "SO/Effects/CritAttacks")]
public class CritAttacksEffectSO : EffectSO
{
    [Header("Crit Attacks")]
    public bool perma = false;
    public float time = 0;

    public void Awake()
    {
        //effectType = EffectType.CritAttacks;
    }
}
