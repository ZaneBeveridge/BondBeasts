using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaRoamer : Roamer
{
    [Header("Drops")]
    public List<ItemDrop> drops = new List<ItemDrop>();
    [Header("Background")]
    public Sprite background;
    [Header("Monster")]
    public int roamerMaxHealth = 100;
    public Monster monster;

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

    

    private void AttackPlayer()
    {
        List<Monster> mons = new List<Monster>();
        mons.Add(monster);


        GM.overworldGameobject.SetActive(false);
        GM.overworldUI.gameObject.SetActive(false);
        GM.battleManager.InitAlpha(mons, background, drops, this, roamerMaxHealth);
    }

    public override void Init(Node node, GameManager g, RoamerController c)
    {
        GM = g;
        currentNode = node;
        controller = c;
    }


    private bool CheckForPlayerClose()
    {
       // Debug.Log("Current Player Node: " + GM.playerManager.currentNode.id);
        //Debug.Log("Current Roamer Node: " + currentNode.id);

        if (currentNode.northNode != null)
        {
            if (currentNode.northNode.id == GM.playerManager.currentNode.id)
            {
                //MOVE TO PLAYER IMMEDIATLY
                int extraMove = currentNode.nMoveAmount;
                currentNode = currentNode.northNode;
                StartCoroutine(MoveRoamer(Vector3.up * (distanceMod * extraMove), true));
                Debug.Log("GO UP PLAYER");

                return true;
            }
        }

        if (currentNode.southNode != null)
        {
            if (currentNode.southNode.id == GM.playerManager.currentNode.id)
            {
                //MOVE TO PLAYER IMMEDIATLY
                int extraMove = currentNode.sMoveAmount;
                currentNode = currentNode.southNode;
                StartCoroutine(MoveRoamer(Vector3.down * (distanceMod * extraMove), true));
                Debug.Log("GO DOWN PLAYER");

                return true;
            }
        }

        if (currentNode.eastNode != null)
        {
            if (currentNode.eastNode.id == GM.playerManager.currentNode.id)
            {
                //MOVE TO PLAYER IMMEDIATLY
                int extraMove = currentNode.eMoveAmount;
                currentNode = currentNode.eastNode;
                StartCoroutine(MoveRoamer(Vector3.right * (distanceMod * extraMove), true));
                Debug.Log("GO RIGHT PLAYER");

                return true;
            }
        }

        if (currentNode.westNode != null)
        {
            if (currentNode.westNode.id == GM.playerManager.currentNode.id)
            {
                //MOVE TO PLAYER IMMEDIATLY
                int extraMove = currentNode.wMoveAmount;
                currentNode = currentNode.westNode;
                StartCoroutine(MoveRoamer(Vector3.left * (distanceMod * extraMove), true));
                Debug.Log("GO LEFT PLAYER");

                return true;
            }
        }


        return false;
    }


    public override void DoTurn()
    {
        // direction 1 = go north
        // direction 2 = go south
        // direction 3 = go east
        // direction 4 = go west
        if (moving) return;
        //Debug.Log("BeforeMove");


        if (!CheckForPlayerClose())
        {
            List<int> directionList = new List<int>();


            if (currentNode.northNode != null)
            {
                directionList.Add(1);
            }

            if (currentNode.southNode != null)
            {
                directionList.Add(2);
            }

            if (currentNode.eastNode != null)
            {
                directionList.Add(3);
            }

            if (currentNode.westNode != null)
            {
                directionList.Add(4);
            }


            for (int i = 0; i < directionList.Count; i++)
            {
                //Debug.Log("CanGoDirection: " + directionList[i]);
            }

            int rand = Random.Range(0, directionList.Count);
            //Debug.Log(rand);

            if (directionList[rand] == 1)
            {
                //Debug.Log("Go Up");
                int extraMove = currentNode.nMoveAmount;
                currentNode = currentNode.northNode;
                StartCoroutine(MoveRoamer(Vector3.up * (distanceMod * extraMove), false));
            }

            if (directionList[rand] == 2)
            {
                //Debug.Log("Go Down");
                int extraMove = currentNode.sMoveAmount;
                currentNode = currentNode.southNode;
                StartCoroutine(MoveRoamer(Vector3.down * (distanceMod * extraMove), false));
            }

            if (directionList[rand] == 3)
            {
               // Debug.Log("Go Right");
                int extraMove = currentNode.eMoveAmount;
                currentNode = currentNode.eastNode;
                StartCoroutine(MoveRoamer(Vector3.right * (distanceMod * extraMove), false));
            }

            if (directionList[rand] == 4)
            {
                //Debug.Log("Go Left");
                int extraMove = currentNode.wMoveAmount;
                currentNode = currentNode.westNode;
                StartCoroutine(MoveRoamer(Vector3.left * (distanceMod * extraMove), false));
            }
        }
        
    }




    private IEnumerator MoveRoamer(Vector3 direction, bool isAttack)
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

        if (isAttack)
        {
            AttackPlayer();
        }
    }

}
