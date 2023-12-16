using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Passive", menuName = "SO/Passive")]
public class PassiveSO : ScriptableObject
{
    [Header("Basic")]
    public int id;
    public string moveName;
    public string moveDescription;

    public bool shared = false;

    [Header("Passive")]
    public List<Passive> passiveActions = new List<Passive>();




    public bool AlwaysActive(int index)
    {
        bool state = false;

        if (passiveActions[index].conditions.always)
        {
            state = true;
        }

        return state;
    }
}


[System.Serializable]
public class Passive
{
    public string name;
    public EffectSO effect;
    public Targets targets;
    public PassiveConditions conditions;
}

[System.Serializable]
public class PassiveConditions
{
    public bool always;
    public bool whenBeingHit; // trigger
    public bool whenTagIn; // trigger
    public bool whenTagOut; // trigger
    public bool whenCrit; // trigger
    public bool whenEnemyStunned; // trigger
    public bool whenUseBasic; // trigger
    public bool whenUseSpecial; // trigger
    public bool whenEnemyHitBasic; // trigger
    public bool whenEnemyHitSpecial; // trigger
    

    public bool whileEnemyFullHP; // property change
    public bool whileEnemyBelow15HP; // property change
    public bool whileInAir; // property change
    public bool whileNotInAir; // property change
    public bool whileFullHP; // property change
    public bool whileBelow15HP; // property change
}


