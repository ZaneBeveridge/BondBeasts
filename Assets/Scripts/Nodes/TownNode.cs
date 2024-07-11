using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownNode : Node
{
    public override void OnEnter() // Entering node
    {
        GM.playerManager.OnEnterNode(text, this);
        if (!entered & onEnterCutscene != null)
        {
            GM.overworldUI.partyButtonParent.SetActive(true);
            GM.cutsceneController.PlayCutscenePreStarter(onEnterCutscene);
            completed = true;
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
        if (GM.playerHP < 50f)
        {
            GM.playerHP = 50f;
            GM.popupManager.FullyHealed();
            GM.overworldUI.healthBar.SetHealth(50f, false);
        }

        
        GM.playerHomeNodeID = id;
        //GM.collectionManager.OpenInterface();
    }


    public override void Refresh()
    {
        GM.playerManager.OnRefreshNode(text);
    }
}
