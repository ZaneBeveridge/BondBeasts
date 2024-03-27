using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropNode : Node
{
    public List<StoredItem> items = new List<StoredItem>();
    public override void SetComplete(bool state)
    {
        completed = state;
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
        for (int i = 0; i < items.Count; i++)
        {
            bool merge = false;
            int mergeId = 0;

            if (GM.collectionManager.itemsOwned.Count > 0)
            {
                for (int j = 0; j < GM.collectionManager.itemsOwned.Count; j++)
                {
                    if (GM.collectionManager.itemsOwned[j].item == items[i].item)
                    {
                        mergeId = j;
                        merge = true;
                        break;
                    }
                }
            }

            if (merge)
            {
                GM.collectionManager.itemsOwned[mergeId].amount += items[i].amount;
            }
            else
            {
                GM.collectionManager.AddItemToStorage(items[i].item, items[i].amount);
            }
        }

        SetComplete(true);
        Refresh();
        GM.SaveData();
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
