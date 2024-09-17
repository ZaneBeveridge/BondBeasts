using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    public OverworldUI overworldUI;
    public GameManager GM;
    public RoamerController currentRoamerController;

    public CutsceneSO startingCutscene;
    [Header("Node")]
    public Node homeNode;

    public Node currentNode;
    public Node previousNode;

    private bool readyToAttackRoamer = false;

    //MOVEMENT
    public bool isMoving;
    private Vector3 originalPos, targetPos;
    private float timeToMove = .2f;
    private int distanceMod = 2;


    void Update()
    {
        if (readyToAttackRoamer)
        {
            if (currentRoamerController.IsRoamerOnThisNode(currentNode))
            {
                Roamer roamer = currentRoamerController.GetRoamerOnNode(currentNode);

                if (roamer.roamerType == RoamerType.Alpha)
                {
                    AlphaRoamer alphaRoamer = currentRoamerController.GetRoamerOnNode(currentNode) as AlphaRoamer;


                    List<Monster> mons = new List<Monster>();
                    mons.Add(alphaRoamer.monster);

                    GM.overworldGameobject.SetActive(false);
                    GM.overworldUI.gameObject.SetActive(false);
                    GM.battleManager.InitAlpha(mons, alphaRoamer.background, alphaRoamer.drops, alphaRoamer, alphaRoamer.roamerMaxHealth, alphaRoamer.customItems);

                }
                else if (roamer.roamerType == RoamerType.Healer)
                {
                    HealerRoamer healerRoamer = currentRoamerController.GetRoamerOnNode(currentNode) as HealerRoamer;
                    //Heal

                    int rand = Random.Range(healerRoamer.minHeal, healerRoamer.maxHeal);
                    if (rand + GM.playerHP >= 1000)
                    {
                        GM.playerHP = 1000;
                    }
                    else
                    {
                        GM.playerHP = GM.playerHP + rand;
                    }

                    
                    GM.overworldUI.healthBar.SetHealth(GM.playerHP, false);
                    healerRoamer.Kill();

                }
                else if (roamer.roamerType == RoamerType.Trader)
                {
                    TraderRoamer traderRoamer = currentRoamerController.GetRoamerOnNode(currentNode) as TraderRoamer;
                    // Open Trader Menu
                    traderRoamer.Kill();
                }
            }
            else
            {
                //GM.SaveData(); //this make saving happen when landing on a node but no when landing on a roamer, without save should happen after the nodes been cleared or interacted with.
            }

            
            readyToAttackRoamer = false;
        }
    }

    public void StartNew()
    {
        currentNode = homeNode;
        previousNode = homeNode;
        SetUI("");
        GM.overworldUI.HideUI(true);
        GM.overworldUI.partyButtonParent.SetActive(false);

        GM.cutsceneController.PlayCutscene(startingCutscene);
    }

    public void StartLoad(Node node, Node prevNode)
    {
        //Debug.Log("Current node: " + node.id + ". Prev node: " + prevNode.id);

        
        previousNode = prevNode;
        currentNode = node;

        if (currentNode.IsComplete())
        {
            SetUI(node.doneText);
        }
        else
        {
            SetUI(node.text);
        }
        

        transform.position = currentNode.transform.position;
    }

    public void OnRefreshNode(string text)
    {
        SetUI(text);

        GM.SaveData();
    }

    public void OnEnterNode(string text, Node node)
    {
        previousNode = currentNode;
        currentNode = node;
        SetUI(text);
    }

    private void SetUI(string txt)
    {
        overworldUI.popupManager.Close();
        overworldUI.SetDirections(
            currentNode.northNode, currentNode.nTotalMonstersNeeded, currentNode.nTotalLevelNeeded, currentNode.nLevelCapNeeded, currentNode.nObjectiveNeeded, currentNode.nNodesCompleteNeeded,
            currentNode.eastNode,  currentNode.eTotalMonstersNeeded, currentNode.eTotalLevelNeeded, currentNode.eLevelCapNeeded, currentNode.eObjectiveNeeded, currentNode.eNodesCompleteNeeded,
            currentNode.southNode, currentNode.sTotalMonstersNeeded, currentNode.sTotalLevelNeeded, currentNode.sLevelCapNeeded, currentNode.sObjectiveNeeded, currentNode.sNodesCompleteNeeded,
            currentNode.westNode, currentNode.wTotalMonstersNeeded, currentNode.wTotalLevelNeeded, currentNode.wLevelCapNeeded, currentNode.wObjectiveNeeded, currentNode.wNodesCompleteNeeded
            );
        overworldUI.SetInteractionText(txt);
        overworldUI.SetDialogueButton(currentNode);
        overworldUI.popupManager.Open(currentNode);
    }


    public void Interact()
    {
        currentNode.OnInteract();
    }

    public void DialogueButton()
    {
        currentNode.OnDialogue();
    }

    public void Move(int direction)
    {
        // direction 1 = go north
        // direction 2 = go east
        // direction 3 = go south
        // direction 4 = go west

        if (isMoving) return;

        if (direction == 1)
        {
            int extraMove = currentNode.nMoveAmount;
            currentNode.OnExit(previousNode, currentNode, currentNode.northNode);
            StartCoroutine(MovePlayer(Vector3.up * (distanceMod * extraMove)));
        }

        if (direction == 2)
        {
            int extraMove = currentNode.eMoveAmount;
            currentNode.OnExit(previousNode, currentNode, currentNode.eastNode);
            StartCoroutine(MovePlayer(Vector3.right * (distanceMod * extraMove)));
        }

        if (direction == 3)
        {
            int extraMove = currentNode.sMoveAmount;
            currentNode.OnExit(previousNode, currentNode, currentNode.southNode);
            StartCoroutine(MovePlayer(Vector3.down * (distanceMod * extraMove)));
        }

        if (direction == 4)
        {
            int extraMove = currentNode.wMoveAmount;
            currentNode.OnExit(previousNode, currentNode, currentNode.westNode);
            StartCoroutine(MovePlayer(Vector3.left * (distanceMod * extraMove)));
        }
    }



    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        originalPos = transform.position;
        targetPos = originalPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;

        readyToAttackRoamer = true;
        //GM.playerManager.isMoving = false;

        if (currentNode.nodeType == NodeType.Blank)
        {
            GM.SaveData();
        }
    }
}
