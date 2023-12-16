using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour
{
    public int id;
    public string text;
    public string doneText;
    public NodeType nodeType;
    public bool accessableByRoamers = true;
    public NodeSpawnPoint nodeSpawnPoint;

    public SpriteRenderer rend;

    [Header("Connected Nodes - NESW")]

    [Header("NORTH")]
    public Node northNode;
    public int nTotalMonstersNeeded = 0;
    public int nTotalLevelNeeded = 0; // if 0 is open to all levels
    public int nLevelCapNeeded = 0;
    public int nObjectiveNeeded = 0; // if 0, no objective is on 0 so is open always
    public List<int> nNodesCompleteNeeded = new List<int>();
    public int nMoveAmount = 1;
    [Header("SOUTH")]
    public Node southNode;
    public int sTotalMonstersNeeded = 0;
    public int sTotalLevelNeeded = 0; // if 0 is open to all levels
    public int sLevelCapNeeded = 0;
    public int sObjectiveNeeded = 0; // if 0, no objective is on 0 so is open always
    public List<int> sNodesCompleteNeeded = new List<int>();
    public int sMoveAmount = 1;
    [Header("EAST")]
    public Node eastNode;
    public int eTotalMonstersNeeded = 0;
    public int eTotalLevelNeeded = 0; // if 0 is open to all levels
    public int eLevelCapNeeded = 0;
    public int eObjectiveNeeded = 0; // if 0, no objective is on 0 so is open always
    public List<int> eNodesCompleteNeeded = new List<int>();
    public int eMoveAmount = 1;
    [Header("WEST")]
    public Node westNode;
    public int wTotalMonstersNeeded = 0;
    public int wTotalLevelNeeded = 0; // if 0 is open to all levels
    public int wLevelCapNeeded = 0;
    public int wObjectiveNeeded = 0; // if 0, no objective is on 0 so is open always
    public List<int> wNodesCompleteNeeded = new List<int>();
    public int wMoveAmount = 1;

    [Header("Cutscenes")]
    public CutsceneSO cutscene;
    [Header("Completion")]


    public bool completed = false;
    public int completedObjectiveUnlock = 0; //0 is nothing 1 and above are different objectives that get activated when completing
    [Header("Manager")]

    public GameManager GM;


    public abstract void OnEnter(); // Is called from the last node on after passing through the exit method, sets UI to active and sets the directions and interaction buttons to that of the node

    public abstract void OnExit(Node previousNode, Node currentNode, Node newNode); // Is called when the player presses in a direction and checks for the movement of roaming enemies on the map
    public abstract void OnInteract(); // Initiates after pressing the interaction button and starts an interation based on what node the player is on

    public abstract void SetComplete(bool state);
    public abstract bool IsComplete();

    public abstract void Refresh();

    public void Hidden(bool state)
    {
        rend.enabled = !state;
    }

}


public enum NodeType
{
    Blank,
    Battle,
    Town,
    Survival,
    Punk,
    Challenge,
    Roamer,
    CapIncrease,
    ItemDrop,
    Secret
}


