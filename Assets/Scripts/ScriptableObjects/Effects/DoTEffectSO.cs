using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoTEffect", menuName = "SO/Effects/DoT")]
public class DoTEffectSO : EffectSO
{
    [Header("DoT")]
    public bool perma = false;
    public int amount = 0;
    public float time = 0;

    public void Awake()
    {
        effectType = EffectType.DoT;
    }
}
