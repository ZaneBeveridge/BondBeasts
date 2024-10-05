using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetHealthEffect", menuName = "SO/Effects/SetHealth")]
public class SetHealthEffectSO : EffectSO
{
    [Header("Set Health")]
    public int healAmount;

    public void Awake()
    {
        effectType = EffectType.Heal;
    }
}
