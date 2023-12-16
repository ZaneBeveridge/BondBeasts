using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretNode : Node
{
    public List<Node> hiddingNodes = new List<Node>();


    public override void SetComplete(bool state)
    {
        completed = state;

        for (int i = 0; i < hiddingNodes.Count; i++)
        {
            if (state)
            {
                hiddingNodes[i].Hidden(false);
            }
            else
            {
                hiddingNodes[i].Hidden(true);
            }
        }
    }

    public override void OnEnter()
    {
        
        GM.playerManager.OnEnterNode("", this);

        if (!completed)
        {
            if (cutscene != null)
            {
                GM.cutsceneController.PlayCutscene(cutscene);
            }

            SetComplete(true);
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
