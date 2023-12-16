using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TakingCritsEffect", menuName = "SO/Effects/TakingCrits")]
public class TakingCritsEffectSO : EffectSO
{
    [Header("Taking Crits")]
    public bool perma = false;
    public float time = 1;

    public void Awake()
    {
        effectType = EffectType.TakingCrits;
    }
}
