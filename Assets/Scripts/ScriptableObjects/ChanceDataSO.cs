using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChanceData", menuName = "SO/Colour/ChanceData", order = 2)]
public class ChanceDataSO : ScriptableObject
{
    public List<int> chances = new List<int>();
}
