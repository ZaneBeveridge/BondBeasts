using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeBombEffect", menuName = "SO/Effects/TimeBomb")]
public class TimeBombEffectSO : EffectSO
{
    [Header("TimeBomb")]
    public int damage;
    public bool breaksOnDamage;
    public bool stacks;
    public float decayTime;
    public int decayAmount = 1;

    public void Awake()
    {
        effectType = EffectType.TimeBomb;
    }
}
