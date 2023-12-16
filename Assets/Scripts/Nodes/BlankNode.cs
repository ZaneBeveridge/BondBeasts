using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankNode : Node
{
    public override void SetComplete(bool state)
    {
        completed = state;
    }

    public override void OnEnter()
    {
        completed = true;
        GM.playerManager.OnEnterNode("", this);

        if (cutscene != null)
        {
            GM.cutsceneController.PlayCutscene(cutscene);
        }
    }

    public override void OnExit(Node previousNode, Node currentNode, Node newNode)
    {
        

        if (previousNode == null || previousNode != newNode)
        {
            //UPDATE WORLD HERE
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
        
    }

    public override bool IsComplete()
    {
        return completed;
    }

    public override void Refresh()
    {
        GM.playerManager.OnRefreshNode("");
    }
}
