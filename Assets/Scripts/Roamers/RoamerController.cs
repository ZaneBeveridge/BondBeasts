using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamerController : MonoBehaviour
{
    public List<GameObject> currentRoamers = new List<GameObject>();
    public List<NodeSpawnPoint> spawnPoints = new List<NodeSpawnPoint>();

    public List<RoamTypeWeight> roamTypeWeights = new List<RoamTypeWeight>();

    public Transform roamersTransform;
    public List<GameObject> alphaRoamerPrefabs = new List<GameObject>();
    public List<GameObject> traderRoamerPrefabs = new List<GameObject>();
    public List<GameObject> healerRoamerPrefabs = new List<GameObject>();

    public int turnsUntilSpawn = 20;
    public int maxRoamers = 1;
    [SerializeField]private int turnCount = 0;

    public bool roamerActive = false;
    public int roamerControllerUnlockObjective;

    public GameManager GM;


    public void Start()
    {
        if (GM.objectivesComplete.Count <= roamerControllerUnlockObjective - 1)
        {
            GM.objectivesComplete.Add(false);
        }
    }



    public void TurnCount()
    {
        if (GM.objectivesComplete[roamerControllerUnlockObjective - 1])
        {
            turnCount++;
            TrySpawn();
        }
        
    }

    public void RoamerControllerMove()
    {
        //Debug.Log("Hello1");

        if (GM.objectivesComplete[roamerControllerUnlockObjective - 1])
        {
            //Debug.Log("Hello2");
            for (int i = 0; i < currentRoamers.Count; i++)
            {
                currentRoamers[i].GetComponent<Roamer>().DoTurn();
            }

            GM.SaveData();
        }

        // GM.playerManager.isMoving = false;
    }

    public void SpawnRoamerFromLoad(GameObject prefab, Node currentNode)
    {
        GameObject roam = Instantiate(prefab, currentNode.transform.position, Quaternion.identity, roamersTransform);
        roam.GetComponent<Roamer>().Init(currentNode, GM, this);

        currentRoamers.Add(roam);
    }

    private void TrySpawn()
    {
        if (turnCount >= turnsUntilSpawn & currentRoamers.Count < maxRoamers)
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                if (spawnPoints[i].CanSpawnRoamer())
                {
                    SpawnRoamer(i);
                    turnCount = 0;
                    break;
                }
            }
        }
    }


    private void SpawnRoamer(int spawnPointID)
    {
        float total = 0;

        for (int i = 0; i < roamTypeWeights.Count; i++)
        {
            total += roamTypeWeights[i].weight;
        }

        float random = Random.Range(1, total);

        int variantIndex = 0;
        float addUpVariants = 0;
        for (int i = 0; i < roamTypeWeights.Count; i++)
        {
            addUpVariants = addUpVariants + roamTypeWeights[i].weight;

            if (random <= addUpVariants)
            {
                variantIndex = i;
                break;
            }
        }


        if (variantIndex == 0) // alpha
        {
            if(alphaRoamerPrefabs.Count > 0)
            {
                int rand = Random.Range(0, alphaRoamerPrefabs.Count);

                GameObject roam = Instantiate(alphaRoamerPrefabs[rand],spawnPoints[spawnPointID].transform.position, Quaternion.identity ,roamersTransform);
                roam.GetComponent<Roamer>().Init(spawnPoints[spawnPointID].myNode, GM, this);

                currentRoamers.Add(roam);

            }
        }
        else if (variantIndex == 1) // healer
        {
            if (healerRoamerPrefabs.Count > 0)
            {
                int rand = Random.Range(0, healerRoamerPrefabs.Count);

                GameObject roam = Instantiate(healerRoamerPrefabs[rand], spawnPoints[spawnPointID].transform.position, Quaternion.identity, roamersTransform);
                roam.GetComponent<Roamer>().Init(spawnPoints[spawnPointID].myNode, GM, this);

                currentRoamers.Add(roam);

            }
        }
        else if (variantIndex == 2) // trader
        {
            if (traderRoamerPrefabs.Count > 0)
            {
                int rand = Random.Range(0, traderRoamerPrefabs.Count);

                GameObject roam = Instantiate(traderRoamerPrefabs[rand], spawnPoints[spawnPointID].transform.position, Quaternion.identity, roamersTransform);
                roam.GetComponent<Roamer>().Init(spawnPoints[spawnPointID].myNode, GM, this);

                currentRoamers.Add(roam);

            }
        }
        
    }


    public bool IsRoamerOnThisNode(Node node)
    {
        bool state = false;


        for (int i = 0; i < currentRoamers.Count; i++)
        {
            if (currentRoamers[i].GetComponent<Roamer>().currentNode == node)
            {
                state = true;
                break;
            }
        }

        return state;
    }

    public Roamer GetRoamerOnNode(Node node) // only works if there is a roamer on node
    {
        for (int i = 0; i < currentRoamers.Count; i++)
        {
            if (currentRoamers[i].GetComponent<Roamer>().currentNode == node)
            {
                return currentRoamers[i].GetComponent<Roamer>();
            }
        }

        return null;
    }

}

[System.Serializable]
public class RoamTypeWeight
{
    public string name;
    public int weight;
}
