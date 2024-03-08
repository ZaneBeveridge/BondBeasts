using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterItem", menuName = "SO/MonsterItem")]
public class MonsterItemSO : ScriptableObject
{
    [Header("General")]
    public int id;
    public int groupedId;
    public string itemName;
    public string desc;
    public Sprite icon;
    public ItemType type;
    public int maxStack = 1000;


    [Header("Upgrading")]

    public bool canBeUpgraded;
    public MonsterItemRecipe recipe;


    [Header("Effects")]
    public List<ItemEffect> itemEffects = new List<ItemEffect>();


    public bool AlwaysActive(int index)
    {
        bool state = false;

        if (itemEffects[index].conditions.always)
        {
            state = true;
        }

        return state;
    }

}


public enum ItemType
{
    Currency, // Glitter, used to upgrade and as curreny
    Material, // Materials used to craft items and consumables
    Catalyst // can be Equipped as well as crafted into higher items
}
[System.Serializable]
public class ItemConditions
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

[System.Serializable]
public class ItemEffect
{
    public string name;
    public EffectSO effect;
    public float delay = 0f;
    public Targets targets;
    public ItemConditions conditions;
}

[System.Serializable]
public class MonsterItemRecipe
{
    public MonsterItemSOAmount usedCatalyst;
    public MonsterItemSOAmount glitter;
    public List<MonsterItemSOAmount> extraMats = new List<MonsterItemSOAmount>();
    public MonsterItemSOAmount createdCatalyst;
    
}

[System.Serializable]
public class MonsterItemSOAmount
{
    public MonsterItemSO item;
    public int amount;
}