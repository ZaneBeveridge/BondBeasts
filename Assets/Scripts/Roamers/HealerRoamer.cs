using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerRoamer : Roamer
{
    public int minHeal, maxHeal;

    private bool isMoving;
    private Vector3 originalPos, targetPos;
    private float timeToMove = .2f;
    private int distanceMod = 2;

    private RoamerController controller;

    private bool dying = false;

    private bool moving = false;



    public void Update()
    {
        if (dying & !moving)
        {
            controller.currentRoamers.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void Kill()
    {
        dying = true;
    }


    public override void Init(Node node, GameManager g, RoamerController c)
    {
        GM = g;
        currentNode = node;
        controller = c;
    }


    public override void DoTurn()
    {
        // direction 1 = go north
        // direction 2 = go south
        // direction 3 = go east
        // direction 4 = go west
        if (moving) return;
        //Debug.Log("BeforeMove");


        List<int> directionList = new List<int>();


        if (currentNode.northNode != null)
        {
            if (currentNode.northNode != GM.playerManager.currentNode)
            {
                directionList.Add(1);
            }
            
        }

        if (currentNode.southNode != null)
        {
            if (currentNode.southNode != GM.playerManager.currentNode)
            {
                directionList.Add(2);
            }
            
        }

        if (currentNode.eastNode != null)
        {

            if (currentNode.eastNode != GM.playerManager.currentNode)
            {
                directionList.Add(3);
            }
            
        }

        if (currentNode.westNode != null)
        {

            if (currentNode.westNode != GM.playerManager.currentNode)
            {
                directionList.Add(4);
            }
            
        }

        int rand = Random.Range(0, directionList.Count);
        //Debug.Log(rand);

        if (directionList[rand] == 1)
        {
            //Debug.Log("Go Up");
            int extraMove = currentNode.nMoveAmount;
            currentNode = currentNode.northNode;
            StartCoroutine(MoveRoamer(Vector3.up * (distanceMod * extraMove)));
        }

        if (directionList[rand] == 2)
        {
            //Debug.Log("Go Down");
            int extraMove = currentNode.sMoveAmount;
            currentNode = currentNode.southNode;
            StartCoroutine(MoveRoamer(Vector3.down * (distanceMod * extraMove)));
        }

        if (directionList[rand] == 3)
        {
            //Debug.Log("Go Right");
            int extraMove = currentNode.eMoveAmount;
            currentNode = currentNode.eastNode;
            StartCoroutine(MoveRoamer(Vector3.right * (distanceMod * extraMove)));
        }

        if (directionList[rand] == 4)
        {
            //Debug.Log("Go Left");
            int extraMove = currentNode.wMoveAmount;
            currentNode = currentNode.westNode;
            StartCoroutine(MoveRoamer(Vector3.left * (distanceMod * extraMove)));
        }

    }




    private IEnumerator MoveRoamer(Vector3 direction)
    {
        moving = true;

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

        moving = false;
    }
}
