using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeInfoPopupManager : MonoBehaviour
{

    public GameObject mainObject;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subTitleText;
    public TextMeshProUGUI infoText;


    public Color battleColor;
    public Color punkColor;
    public Color survivalColor;
    public Color capColor;
    public Color itemColor;
    public Color townColor;

    public Color completeColor;
    public Color incompleteColor;


    public GameManager GM;
    public void Open(Node node)
    {
        if (node.nodeType == NodeType.Blank || node.nodeType == NodeType.Secret) { return; }


        mainObject.SetActive(true);
        UpdateVisuals(node);
    }

    public void Close()
    {
        mainObject.SetActive(false);
    }


    private void UpdateVisuals(Node node)
    {
        titleText.text = node.gameObject.name;

        if (node.nodeType == NodeType.Battle)
        {
            titleText.color = battleColor;

            if (node.completed)
            {
                subTitleText.color = completeColor;
                subTitleText.text = "COMPLETE";

                BattleNode bNode = node as BattleNode;

                string text = "Seen beasts:\n";

                for (int i = 0; i < bNode.monsterPool.Count; i++)
                {
                    for (int j = 0; j < GM.playerData.mData.Count; j++)
                    {
                        if (GM.playerData.mData[j] == bNode.monsterPool[i].monster.ID.ID)
                        {
                            text += bNode.monsterPool[i].monster.defaultName + ", ";
                            break;
                        }
                    }
                }
                infoText.text = text;
            }
            else
            {
                subTitleText.color = incompleteColor;
                subTitleText.text = "INCOMPLETE";
                infoText.text = "Complete to pass this area and discover beasts seen here.";
            }

            
        }
        else if (node.nodeType == NodeType.Punk)
        {
            titleText.color = punkColor;

            if (node.completed)
            {
                subTitleText.color = completeColor;
                subTitleText.text = "COMPLETE";

                PunkNode bNode = node as PunkNode;

                string text = "Punk's Beasts:\n";

                for (int i = 0; i < bNode.monsters.Count; i++)
                {
                    text += bNode.monsters[i].name + "\n";
                }
                infoText.text = text;
            }
            else
            {
                subTitleText.color = incompleteColor;
                subTitleText.text = "INCOMPLETE";
                infoText.text = "Challenge this punk to a battle to unlock more of the map.";
            }

        }
        else if (node.nodeType == NodeType.Survival)
        {
            titleText.color = survivalColor;

            SurvivalNode bNode = node as SurvivalNode;

            if (node.completed)
            {
                subTitleText.color = completeColor;
                subTitleText.text = "COMPLETE";
                
                infoText.text = "Highscore: \n" + GM.survivalBest[bNode.survivalID] + " Rounds";
            }
            else
            {
                subTitleText.color = incompleteColor;
                subTitleText.text = "INCOMPLETE";
                infoText.text = "Score To Pass: " + bNode.scoreNeededToPass + "\nBeat the score to pass this area.";
            }
        }
        else if (node.nodeType == NodeType.CapIncrease)
        {
            titleText.color = capColor;

            CapIncreaseNode bNode = node as CapIncreaseNode;

            if (node.completed)
            {
                subTitleText.color = completeColor;
                subTitleText.text = "COMPLETE";

                infoText.text = "Level cap increased to:\n" + bNode.capAmount.ToString();
            }
            else
            {
                subTitleText.color = incompleteColor;
                subTitleText.text = "INCOMPLETE";
                infoText.text = "Average team level needed to increase level cap:\n" + bNode.avgTeamLevelRequired.ToString();
            }
        }
        else if (node.nodeType == NodeType.ItemDrop)
        {
            titleText.color = itemColor;

            ItemDropNode bNode = node as ItemDropNode;

            if (node.completed)
            {
                mainObject.SetActive(false);

                subTitleText.color = completeColor;
                subTitleText.text = "";

                infoText.text = "";
            }
            else
            {
                subTitleText.color = itemColor;
                subTitleText.text = "ITEM PICKUP";
                infoText.text = "Item on ground:\n" + bNode.items[0];
            }
        }
        else if (node.nodeType == NodeType.Town)
        {
            titleText.color = townColor;

            ItemDropNode bNode = node as ItemDropNode;

            if (node.completed)
            {
                subTitleText.color = completeColor;
                subTitleText.text = "";

                infoText.text = "Visit the town to manage your beasts, party, items, and evoltions.";
            }
            else
            {
                subTitleText.color = itemColor;
                subTitleText.text = "";
                infoText.text = "Visit the town to manage your beasts, party, items, and evoltions.";
            }
        }
    }
 
}




