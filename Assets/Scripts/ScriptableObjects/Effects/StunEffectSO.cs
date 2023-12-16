using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StunEffect", menuName = "SO/Effects/Stun")]
public class StunEffectSO : EffectSO
{
    [Header("Stun")]
    public float stunTime;

    public void Awake()
    {
        effectType = EffectType.Stun;
    }
}
