using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunkNode : Node
{
    [Header("Monster Spawn")]
    public Sprite backgroundSprite;
    public string punkName;
    public int punkMaxHealth = 1000;
    public List<MonsterStatic> monsters = new List<MonsterStatic>();

    public List<ItemDrop> drops = new List<ItemDrop>();
    public List<MonsterItemSO> customItems = new List<MonsterItemSO>();

    public List<ObjectiveGate> unlockGates = new List<ObjectiveGate>();

    [Header("Default Settings")]
    public Sprite incompleteSprite;
    public Sprite completeSprite;
    public override void SetComplete(bool state)
    {
        base.SetComplete(state);

        for (int i = 0; i < unlockGates.Count; i++)
        {
            if (completed)
            {
                unlockGates[i].Open();
            }
            else
            {
                unlockGates[i].Close();
            }
        }

        if (completed)
        {
            GetComponent<SpriteRenderer>().sprite = completeSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = incompleteSprite;
        }

        if (completedObjectiveUnlock > 0 && completed)
        {
            GM.objectivesComplete[completedObjectiveUnlock - 1] = true;
        }
    }
    public override void OnEnter() // Entering node
    {
        if (!completed)
        {
            GM.playerManager.OnEnterNode(text, this);
        }
        else
        {
            GM.playerManager.OnEnterNode(doneText, this);
        }

        if (!entered & onEnterCutscene != null)
        {
            GM.cutsceneController.PlayCutscene(onEnterCutscene);
        }
        entered = true;
    }

    public override void OnExit(Node previousNode, Node currentNode, Node newNode)
    {
        if (previousNode == null || previousNode != newNode)
        {
            newNode.OnEnter();
            GM.playerManager.currentRoamerController.TurnCount();
        }
        else
        {
            newNode.OnEnter();
        }
    }

    public override void OnInteract()
    {
        if (GM.playerHP <= 0) { return; }


        GM.overworldGameobject.SetActive(false);
        GM.overworldUI.gameObject.SetActive(false);


        

        List<Monster> mons = new List<Monster>();

        for (int i = 0; i < monsters.Count; i++)
        {
            List<MonsterItemSO> itms = new List<MonsterItemSO>();
            itms.Add(monsters[i].item1);
            itms.Add(monsters[i].item2);
            itms.Add(monsters[i].item3);

            mons.Add(new Monster(
                monsters[i].name,
                monsters[i].level,
                monsters[i].capLevel,
                monsters[i].xp,
                0,
                monsters[i].symbiotic,
                monsters[i].nature,
                monsters[i].variant,
                monsters[i].strange,
                monsters[i].colour,
                monsters[i].stats,
                monsters[i].backupData,
                monsters[i].basicMove,
                monsters[i].specialMove,
                monsters[i].passiveMove,
                itms
                ));
        }

        GM.battleManager.InitPunk(mons, nodeType, backgroundSprite, drops, punkMaxHealth, customItems);

    }

    public override void Refresh()
    {
        if (!completed)
        {
            GM.playerManager.OnRefreshNode(text);
        }
        else
        {
            GM.playerManager.OnRefreshNode(doneText);
        }
    }

}

[System.Serializable]
public class MonsterStatic
{
    public string name;
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
    public VariantSO variant;

    public ColourRoll colour;
    
    public MonsterSO backupData;
}
