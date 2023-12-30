using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Monster
{
    public string name;
    public Identifier ID;

    public int level;
    public int capLevel; 
    public int xp;
    public bool symbiotic;

    public MoveSO basicMove;
    public MoveSO specialMove;
    public PassiveSO passiveMove;

    public MonsterItemSO item1;
    public MonsterItemSO item2;
    public MonsterItemSO item3;

    public List<Stat> stats = new List<Stat>();

    public NatureSO nature;
    public bool strange;

    public ColourRoll colour;
    public VariantSO variant;

    public Sprite staticSprite;
    public Sprite dynamicSprite;
    public AnimatorOverrideController animator;

    public MonsterSO backupData;

    public Monster()
    {

    }


    public Monster(string customName, int lvl, MonsterSO data)
    {
        //Debug.Log("Hello1");
        backupData = data;

        if (customName == "")
        {
            name = data.defaultName;
        }
        else
        {
            name = customName;
        }

        level = lvl;
        capLevel = 0;
        
        ID = data.ID;

        stats.Add(new Stat("Oomph", 0));
        stats.Add(new Stat("Guts", 0));
        stats.Add(new Stat("Juice", 0));
        stats.Add(new Stat("Edge", 0));
        stats.Add(new Stat("Wits", 0));
        stats.Add(new Stat("Spark", 0));

        for (int i = 0; i < stats.Count; i++)
        {
            stats[i].value = data.defaultStats[i].value;
        }
        //stats = data.defaultStats;


        nature = data.GetNature();

        strange = data.GetStrange();

        colour = data.GetColour();

        variant = data.GetVariant();

        staticSprite = data.stillSpriteStatic;
        dynamicSprite = data.stillSpriteDynamic;
        animator = data.animator;

        if (strange)
        {
            basicMove = data.strangePoolMoves[Random.Range(0, data.strangePoolMoves.Count - 1)];
        }
        else
        {
            basicMove = data.basicMove;
        }
        

        specialMove = data.specialMove;
        passiveMove = data.passiveMove;

        item1 = data.blankItem;
        item2 = data.blankItem;
        item3 = data.blankItem;

    }

    public Monster(int lvl, MonsterSO data)
    {
        //Debug.Log("Hello2");
        backupData = data;

        name = data.defaultName;

        level = lvl;
        capLevel = 0;

        ID = data.ID;
        

        stats.Add(new Stat("Oomph", 0));
        stats.Add(new Stat("Guts", 0));
        stats.Add(new Stat("Juice", 0));
        stats.Add(new Stat("Edge", 0));
        stats.Add(new Stat("Wits", 0));
        stats.Add(new Stat("Spark", 0));

        for (int i = 0; i < stats.Count; i++)
        {
            stats[i].value = data.defaultStats[i].value;
        }
        //stats = data.defaultStats;


        nature = data.GetNature();

        strange = data.GetStrange();

        colour = data.GetColour();

        variant = data.GetVariant();

        staticSprite = data.stillSpriteStatic;
        dynamicSprite = data.stillSpriteDynamic;
        animator = data.animator;

        if (strange)
        {
            basicMove = data.strangePoolMoves[Random.Range(0, data.strangePoolMoves.Count - 1)];
        }
        else
        {
            basicMove = data.basicMove;
        }


        specialMove = data.specialMove;
        passiveMove = data.passiveMove;


        item1 = data.blankItem;
        item2 = data.blankItem;
        item3 = data.blankItem;

    }

    // 1 item
    public Monster(string _name, int _level, int _capLevel, int _xp, bool _symbiotic, NatureSO _nature, VariantSO _variant, bool _strange, ColourRoll _color, List<Stat> _stats, MonsterSO _data, MoveSO _bMove, MoveSO _sMove, PassiveSO _pMove, List<MonsterItemSO> items)
    {
        //Debug.Log("Hello3");
        name = _name;
        level = _level;
        capLevel = _capLevel;
        xp = _xp;
        symbiotic = _symbiotic;
        nature = _nature;
        variant = _variant;
        strange = _strange;
        colour = _color; // HERE IS THE ISSUE - FIXED
        stats = _stats;
        backupData = _data;

        ID = backupData.ID;

        staticSprite = backupData.stillSpriteStatic;
        dynamicSprite = backupData.stillSpriteDynamic;
        animator = backupData.animator;

        basicMove = _bMove;
        specialMove = _sMove;
        passiveMove = _pMove;

        item1 = items[0];
        item2 = items[1];
        item3 = items[2];

    }


    public void AddStat(int index, int value)
    {
        //Debug.Log("HelloStat: " + index.ToString());
        stats[index].value = stats[index].value + value;
    }
    
}

[System.Serializable]
public class ColourRoll
{
    public int rarity;
    public int number;
    public Color colour;

    //Testing
    public int rolledNum;
    public int totalRoll;

    

    public ColourRoll(int r, int n, Color c, int rn, int t)
    {
        rarity = r;
        number = n;
        colour = c;
        rolledNum = rn;
        totalRoll = t;
    }
}

