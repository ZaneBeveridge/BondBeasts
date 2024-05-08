using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class OverworldUI : MonoBehaviour
{
    [Header("Interaction Button")]
    public Button interactionButton;
    public TextMeshProUGUI interactionText;
    [Header("Direction Butttons")]
    public Button northButton;
    public Button eastButton;
    public Button southButton;
    public Button westButton;

    [Header("Menu")]
    public GameObject menuUI;
    public GameManager GM;
    public PostBattleLootManager postBattleLootManager;

    public BattleHealthBar healthBar;
    public NodeInfoPopupManager popupManager;
    void Start()
    {
        healthBar.SetMaxHealth(100f);
        healthBar.SetHealth(GM.playerHP, false);
    }


    public void SetInteractionText(string text) // Sets the text on the interact button
    {
        if (text == "")
        {
            interactionText.text = text;
            interactionButton.gameObject.SetActive(false);
        }
        else
        {
            interactionButton.gameObject.SetActive(true);
            interactionText.text = text;
        }
        
    }
    //, Node eastNode, int eLevelNeeded, int eObjNeeded, Node southNode, int sLevelNeeded, int sObjNeeded, Node westNode, int wLevelNeeded, int wObjNeeded


    public void SetDirections(
        Node northNode, int nMonstersNeeded, int nLevelNeeded, int nCapNeeded, int nObjNeeded, List<int> nNodesNeeded,
        Node eastNode, int eMonstersNeeded, int eLevelNeeded, int eCapNeeded, int eObjNeeded, List<int> eNodesNeeded,
        Node southNode, int sMonstersNeeded, int sLevelNeeded, int sCapNeeded, int sObjNeeded, List<int> sNodesNeeded,
        Node westNode, int wMonstersNeeded, int wLevelNeeded, int wCapNeeded, int wObjNeeded, List<int> wNodesNeeded) // Sets which directional buttons are active based on the nodes connected nodes
    {
        northButton.gameObject.SetActive(false);
        southButton.gameObject.SetActive(false);
        eastButton.gameObject.SetActive(false);
        westButton.gameObject.SetActive(false);

        if (northNode != null)
        {
            // if pass level + objective requirements set to true else false
            if (GM.CanPassToNode(nMonstersNeeded, nLevelNeeded, nCapNeeded, nObjNeeded, nNodesNeeded))
            {
                if (GM.PassageClear(GM.playerManager.currentNode) || northNode == GM.playerManager.previousNode)
                {
                    northButton.gameObject.SetActive(true);
                    if (northNode.nodeType == NodeType.Secret && !GM.nodesCompleted[northNode.id])
                    {
                        northButton.gameObject.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                    }
                    else
                    {
                        northButton.gameObject.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
                    }
                    
                }
            }
        }

        if (southNode != null)
        {
            // if pass level + objective requirements set to true else false
            if (GM.CanPassToNode(sMonstersNeeded, sLevelNeeded, sCapNeeded, sObjNeeded, sNodesNeeded))
            {
                if (GM.PassageClear(GM.playerManager.currentNode) || southNode == GM.playerManager.previousNode)
                {
                    southButton.gameObject.SetActive(true);
                    if (southNode.nodeType == NodeType.Secret && !GM.nodesCompleted[southNode.id])
                    {
                        southButton.gameObject.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                    }
                    else
                    {
                        southButton.gameObject.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
                    }
                }
            }
        }

        if (eastNode != null)
        {
            // if pass level + objective requirements set to true else false
            //Debug.Log("objective: " + eastNode.eObjectiveNeeded);
            if (GM.CanPassToNode(eMonstersNeeded, eLevelNeeded, eCapNeeded, eObjNeeded, eNodesNeeded))
            {
                if (GM.PassageClear(GM.playerManager.currentNode) || eastNode == GM.playerManager.previousNode)
                {
                    eastButton.gameObject.SetActive(true);
                    if (eastNode.nodeType == NodeType.Secret && !GM.nodesCompleted[eastNode.id])
                    {
                        eastButton.gameObject.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                    }
                    else
                    {
                        eastButton.gameObject.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
                    }
                }
                
            }
        }

        if (westNode != null)
        {
            // if pass level + objective requirements set to true else false
            if (GM.CanPassToNode(wMonstersNeeded, wLevelNeeded, wCapNeeded, wObjNeeded, wNodesNeeded))
            {
                if (GM.PassageClear(GM.playerManager.currentNode) || westNode == GM.playerManager.previousNode)
                {
                    westButton.gameObject.SetActive(true);
                    if (westNode.nodeType == NodeType.Secret && !GM.nodesCompleted[westNode.id])
                    {
                        westButton.gameObject.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                    }
                    else
                    {
                        westButton.gameObject.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
                    }
                }
            }
        }

    }


    public void SetMenuOpen(bool state)
    {
        GM.overworldUI.gameObject.SetActive(!state);
        menuUI.SetActive(state);
    }
    
}
