using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LowGravityEffect", menuName = "SO/Effects/LowGravity")]
public class LowGravityEffectSO : EffectSO
{
    [Header("Gravity Time")]
    public float time;

    public void Awake()
    {
        effectType = EffectType.LowGravity;
    }
}
