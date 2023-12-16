using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpawnPoint : MonoBehaviour
{
    public GameManager GM;
    public Node myNode;

    // Start is called before the first frame update
    public bool CanSpawnRoamer()
    {
        bool state = true;


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
