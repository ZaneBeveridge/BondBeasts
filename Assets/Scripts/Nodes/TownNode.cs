using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownNode : Node
{
    public override void SetComplete(bool state)
    {
        completed = true;
    }
    public override void OnEnter() // Entering node
    {
        GM.playerManager.OnEnterNode(text, this);

        if (cutscene != null)
        {
            GM.cutsceneController.PlayCutscene(cutscene);
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
        }
    }

    public override void OnInteract()
    {
        if (GM.playerHP < 50f)
        {
            GM.playerHP = 50f;
            GM.popupManager.FullyHealed();
            GM.overworldUI.healthBar.SetHealth(50f, false);
        }

        
        GM.playerHomeNodeID = id;
        //GM.collectionManager.OpenInterface();
    }

    public override bool IsComplete()
    {
        return completed;
    }

    public override void Refresh()
    {
        GM.playerManager.OnRefreshNode(text);
    }
}
