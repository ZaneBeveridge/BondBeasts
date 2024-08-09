using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "SO/Move", order = 5)]
public class MoveSO : ScriptableObject
{
    [Header("Basic")]
    public int id;
    public string moveName;
    public string moveDescription;
    public string editableMoveDescription;
    public string moveType;
    public MoveSet moveSet;
    //public Sprite iconSprite;
    public float baseCooldown = 1f;
    public float minCooldown = 0f;

    [Header("Move")]
    public List<Move> moveActions = new List<Move>();
    

   

    public FireProjectileEffectSO GetProjectile(bool inAir, int hp)
    {
        foreach (Move move in moveActions)
        {
            if (move.effect.effectType == EffectType.FireProjectile)
            {
                if (move.conditions.whenUsed && move.conditions.whenInAir == inAir)
                {
                    if (move.conditions.whenFullHP)
                    {
                        if (hp >= 1000)
                        {
                            return move.effect as FireProjectileEffectSO;
                        }
                    }
                    else if (move.conditions.whenBelow15HP)
                    {
                        if (hp < 150)
                        {
                            return move.effect as FireProjectileEffectSO;
                        }
                    }
                    else
                    {
                        return move.effect as FireProjectileEffectSO;
                    }
                    
                }
                
            }
        }

        return null;

    }

}






[System.Serializable]
public class Move
{
    public string name;
    public EffectSO effect;
    public float delay = 0f;
    public Targets targets;
    public Conditions conditions;
}

[System.Serializable]
public class Conditions
{
    public bool whenUsed;
    public bool whenNotInAir = true;
    public bool whenInAir;
    public bool whenTagIn;
    public bool whenTagOut;
    public bool whenFullHP;
    public bool whenBelow15HP;
    //public InequalityCondition whenHP;
}

[System.Serializable]
public class Targets
{
    public bool team;
    public bool enemy;

    public Targets(bool t, bool e)
    {
        team = t;
        enemy = e;
    }
}

/*
[System.Serializable]
public class InequalityCondition
{
    public bool active;
    public Inequality HPInequality;
    public float value;
}


public enum Inequality
{
    Equal,
    LessThan,
    GreaterThan,
    LessThanOrEqual,
    GreaterThanOrEqual
}
*/

public enum MoveSet
{
    Basic,
    Special
}





