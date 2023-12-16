using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "SO/Effects/Heal")]
public class HealEffectSO : EffectSO
{
    [Header("Heal")]
    public int healAmount;

    public void Awake()
    {
        effectType = EffectType.Heal;
    }
}
