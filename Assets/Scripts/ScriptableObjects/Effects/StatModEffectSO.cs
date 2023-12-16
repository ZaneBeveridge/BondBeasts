using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StatModEffect", menuName = "SO/Effects/StatMod")]
public class StatModEffectSO : EffectSO
{
    [Header("Stat Modifier")]
    public EffectedStat stat;
    public int amount;

    public void Awake()
    {
        effectType = EffectType.StatMod;
    }
}

public enum EffectedStat
{
    Oomph,
    Guts,
    Juice,
    Edge,
    Wits,
    Spark
}
