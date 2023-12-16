using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nature", menuName = "SO/Nature", order = 2)]
public class NatureSO : ScriptableObject
{
    [Header("Info")]
    public string natureName;
    public int id;

    [Header("Stats")]
    public List<PercentStat> addedStats = new List<PercentStat>();


    private void Awake()
    {
        if (addedStats.Count <= 0)
        {
            addedStats.Add(new PercentStat("Oomph", 1f));
            addedStats.Add(new PercentStat("Guts", 1f));
            addedStats.Add(new PercentStat("Juice", 1f));
            addedStats.Add(new PercentStat("Edge", 1f));
            addedStats.Add(new PercentStat("Wits", 1f));
            addedStats.Add(new PercentStat("Spark", 1f));
        }
        
    }
}
[System.Serializable]
public class PercentStat
{
    public string name;
    [Range(0.50f, 1.50f)] public float value = 1f;
    public PercentStat(string n, float v)
    {
        name = n;
        value = v;
    }
}

