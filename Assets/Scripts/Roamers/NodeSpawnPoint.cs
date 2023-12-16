using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpawnPoint : MonoBehaviour
{
    public GameManager GM;
    public Node myNode;

    // Start is called before the first frame update

    private void Awake()
    {
        myNode = GetComponent<Node>();
    }

    public bool CanSpawnRoamer()
    {
        bool state = true;


        float minDist = 7f;
        float dist = Vector3.Distance(GM.playerManager.transform.position, transform.position);

        if (dist < minDist)
        {
            state = false;
            return state;
        }


        if (GM.playerManager.currentNode.id == myNode.id)
        {
            state = false;
            return state;
        }

        for (int i = 0; i < GM.playerManager.currentRoamerController.currentRoamers.Count; i++)
        {
            if (GM.playerManager.currentRoamerController.currentRoamers[i].GetComponent<Roamer>().currentNode.id == myNode.id)
            {
                state = false;
                return state;
            }
        }

        return state;
    }
}
