using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "SO/Monster", order = 1)]
public class MonsterSO : ScriptableObject
{
    [Header("Info")]
    public string defaultName;
    public Identifier ID;

    [Header("Visuals")]
    public Sprite stillSpriteDynamic;
    public Sprite stillSpriteStatic;
    public AnimatorOverrideController animator;

    [Header("Moves")]
    public bool isHeavyJumper = false;
    public MoveSO basicMove;
    public MoveSO specialMove;
    public PassiveSO passiveMove;
    public List<MoveSO> strangePoolMoves = new List<MoveSO>();

    [Header("Item Drops")]
    public MonsterItemSO blankItem;

    
    public List<ItemDrop> itemDrops = new List<ItemDrop>();

    [Header("Variants")]
    public List<Variant> possibleVariants = new List<Variant>();

    [Header("Colour")]
    public ColourWheel startColour;
    [Range(1, 18)]public int amountOfColours = 1;
    public ColourDataSO colourData;
    public ChanceDataSO chanceData;

    [Header("Background")]
    public string height;
    public string weight;
    public string lore;
    public string locations;

    [Header("Strange")]
    [Range(0, 100)] public float strangeChance = 0.2f;

    [Header("Statistics")]
    public List<Stat> defaultStats = new List<Stat>();

    [Header("References")]
    public List<NatureSO> natures = new List<NatureSO>();

    [Header("Transformations")]

    public MonsterForm ascendForm;
    public MonsterForm descendForm;
    public List<MonsterFormWithCata> cataForms = new List<MonsterFormWithCata>();


    private void Awake()
    {
        if (defaultStats.Count <= 0)
        {
            defaultStats.Add(new Stat("Oomph", 0));
            defaultStats.Add(new Stat("Guts", 0));
            defaultStats.Add(new Stat("Juice", 0));
            defaultStats.Add(new Stat("Edge", 0));
            defaultStats.Add(new Stat("Wits", 0));
            defaultStats.Add(new Stat("Spark", 0));
        }
        
    }


    public int GetStat(int index)
    {
        return defaultStats[index].value;
    }

    


    public NatureSO GetNature()
    {
        return natures[Random.Range(0, natures.Count - 1)];
    }

    public bool GetStrange()
    {
        float roll = Random.Range(0f, 100f);

        //Debug.Log("Strange Roll: " + roll);

        if (roll <= strangeChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public ColourRoll GetColour()
    {
        // GET START AND END INDEX

        int startIndex = 0;
        for (int i = 0; i < colourData.datas.Count; i++)
        {
            if (colourData.datas[i].colourWheel == startColour)
            {
                startIndex = i;
                break;
            }
        }

        // CREAT LIST OF WEIGHTS WITH COLOUR DATA

        List<ColourDataWeight> weights = new List<ColourDataWeight>();
        for (int i = 0; i < amountOfColours; i++)
        {
            int ind = startIndex + i;

            if (ind >= colourData.datas.Count)
            {
                ind = (startIndex + i) - colourData.datas.Count;
            }

            ColourDataWeight weight = new ColourDataWeight(colourData.datas[ind], chanceData.chances[i]);
            weights.Add(weight);
        }

        // ADD UP TOTAL WEIGHTS
        int total = 0;

        for (int i = 0; i < weights.Count; i++)
        {
            total = total + weights[i].weight;
        }

        //GET RANDOM NUMBER BETWEEN 1 AND TOTAL WEIGHT
        int randomNum = Random.Range(1, total);
        //Debug.Log(randomNum + " out of " + total);

        // GO THROUGH EACH WEIGHT AND SEE IT IT LINES UP WITH ROLLED WEIGHT
        int rarity = 0;
        int chosenIndex = 0;
        int addUp = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            rarity++;
            addUp = addUp + weights[i].weight;
            if (randomNum <= addUp)
            {
                chosenIndex = i;
                break;
            }
        }

        // GET COLOURS

        ColourData data = weights[chosenIndex].data;

        float startColourH = GetHSVColourValue(0, data.startColour);
        float endColourH = GetHSVColourValue(0, data.endColour);

        float colourS = GetHSVColourValue(1, data.startColour);

        float colourV = GetHSVColourValue(2, data.startColour);


        float randomHColour = Random.Range(startColourH, endColourH);


        float intNum = randomHColour * 360f;

        //SEND IT!

        ColourRoll roll = new ColourRoll(rarity, Mathf.RoundToInt(intNum), Color.HSVToRGB(randomHColour, colourS, colourV), randomNum, total);

        return roll;
    }

    private float GetHSVColourValue(int output, Color color)
    {
        float value = 0;
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);

        if (output == 0)
        {
            value = h;
        }
        else if (output == 1)
        {
            value = s;
        }
        else if (output == 2)
        {
            value = v;
        }

        return value;
    }


    public VariantSO GetVariant()
    {
        float total = 0;

        for (int i = 0; i < possibleVariants.Count; i++)
        {
            total += possibleVariants[i].weight;
        }

        float random = Random.Range(1, total);

        int variantIndex = 0;
        float addUpVariants = 0;
        for (int i = 0; i < possibleVariants.Count; i++)
        {
            addUpVariants = addUpVariants + possibleVariants[i].weight;

            if (random <= addUpVariants)
            {
                variantIndex = i;
                break;
            }
        }

        //Debug.Log(random);

        return possibleVariants[variantIndex].variant;
    }


}

[System.Serializable]
public class Stat
{
    public string name;
    [Range(-300, 300)] public int value;
    public Stat(string n, int v)
    {
        name = n;
        value = v;
    }
}

[System.Serializable]
public class Identifier
{
    public int ID;
    public int version;
}

[System.Serializable]
public class Variant
{
    public VariantSO variant;
    public float weight;
}


[System.Serializable]
public class ColourDataWeight
{
    public ColourData data;
    public int weight;

    public ColourDataWeight(ColourData d, int w)
    {
        data = d;
        weight = w;
    }
}


[System.Serializable]
public class ItemDrop
{
    public MonsterItemSO item;
    [Range(0f, 100f)] public float chance;
    public int minDrops, maxDrops;
}

[System.Serializable]
public class MonsterForm
{
    public MonsterSO form;
    public List<MonsterItemSOAmount> requiredItems = new List<MonsterItemSOAmount>();

}



[System.Serializable]
public class MonsterFormWithCata
{
    public MonsterSO form;
    public List<MonsterItemSOAmount> requiredItems = new List<MonsterItemSOAmount>();
    public MonsterItemSO requiredCatalyst;
}
