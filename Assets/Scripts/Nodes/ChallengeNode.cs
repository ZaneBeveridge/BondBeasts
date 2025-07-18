using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeNode : Node
{

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
        SetComplete(true);
        Refresh();
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
