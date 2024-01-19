using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleNode : Node
{
    [Header("Monster Spawn")]
    public Sprite backgroundSprite;
    public List<MonsterSpawn> monsterPool = new List<MonsterSpawn>();
    [Range(1,5)]public int minSpawn = 1;
    [Range(1,5)]public int maxSpawn = 1;

    private List<MonsterSpawn> mons = new List<MonsterSpawn>();
    //public MonsterSO monster;
    //public int level = 1;


    public List<ObjectiveGate> unlockGates = new List<ObjectiveGate>();

    [Header("Default Settings")]
    public Sprite incompleteSprite;
    public Sprite completeSprite;
    public override void SetComplete(bool state)
    {
        completed = state;

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
            if (cutscene != null)
            {
                GM.cutsceneController.PlayCutscene(cutscene);
            }

            GM.playerManager.OnEnterNode(text, this);
        }
        else
        {
            GM.playerManager.OnEnterNode(doneText, this);
        }
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
            //GM.playerManager.isMoving = false;
        }
    }

    public override void OnInteract() 
    {
        if (GM.playerHP <= 0) { return; }

        int spawnNum = Random.Range(minSpawn, maxSpawn);

        mons = new List<MonsterSpawn>();

        for (int i = 0; i < spawnNum; i++)
        {
            float total = 0;

            for (int j = 0; j < monsterPool.Count; j++)
            {
                total += monsterPool[j].weight;
            }

            float random = Random.Range(0f, total);



            float addUp = 0;
            for (int j = 0; j < monsterPool.Count; j++)
            {
                addUp = addUp + monsterPool[j].weight;

                if (random <= addUp)
                {
                    mons.Add(monsterPool[j]);
                    break;
                }
            }

        }


        if (mons != null)
        {
            GM.overworldGameobject.SetActive(false);
            GM.overworldUI.gameObject.SetActive(false);

            GM.battleManager.InitBattle(mons, nodeType, backgroundSprite);
        }

    }

    public override bool IsComplete()
    {
        return completed;
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
public class MonsterSpawn
{
    public MonsterSO monster;
    public int minLevel, maxLevel;
    public float weight;
}



