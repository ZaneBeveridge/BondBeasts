using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankNode : Node
{

    public override void OnEnter()
    {
        if (!entered && onEnterCutscene != null)
        {
            GM.cutsceneController.PlayCutscene(onEnterCutscene);
        }


        entered = true;
        completed = true;
        GM.playerManager.OnEnterNode("", this); 
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

    public override void Refresh()
    {
        GM.playerManager.OnRefreshNode("");
    }
}
