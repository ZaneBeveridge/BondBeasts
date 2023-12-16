using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapIncreaseNode : Node
{
    public float avgTeamLevelRequired = 0f;
    public int capAmount = 0;

    public List<ObjectiveGate> unlockGates = new List<ObjectiveGate>();

    public CutsceneSO notEnoughLevelScene;
    public CutsceneSO enoughLevelScene;
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
    }

    public override void OnEnter()
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
        if (GM.avgTeamLevel >= avgTeamLevelRequired)
        {
            if (enoughLevelScene != null)
            {
                GM.cutsceneController.PlayCutscene(enoughLevelScene);
            }


            GM.levelCap = capAmount;
            SetComplete(true);
            Refresh();
            GM.SaveData();
        }
        else
        {
            if (notEnoughLevelScene != null)
            {
                GM.cutsceneController.PlayCutscene(notEnoughLevelScene);
            }
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
