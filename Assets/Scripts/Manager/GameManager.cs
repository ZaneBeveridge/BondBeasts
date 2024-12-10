using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class GameManager : MonoBehaviour
{
    public string playerName;
    public float playerHP = 1000f;
    public int levelCap = 3;

    public int playerHomeNodeID = 0;
    public int numOfBagsCollection = 5;
    public int numOfBagsMaterials = 5;
    public int numOfBagsEquipment = 5;

    public BeastStarterMenuGUI beastStarterMenuGUI;
    public CollectionManager collectionManager;

    public OverworldUI overworldUI;
    public GameObject overworldGameobject;
    public PlayerManager playerManager;
    public CutsceneController cutsceneController;
    public AftermathUI aftermathUI;
    public LevelUpUI levelUpUI;
    public EvolveUIManager evolveUI;
    public ItemInspectManagerPopup itemInspectManagerPopup;

    public BattleManager battleManager;
    public BattleUI battleUI;
    public GameObject battleGameobject;

    public CaptureChoiceWindow captureChoiceWindow;

    public PopupManager popupManager;
    public TimeManager timeManager;
    public FadeToBlackManager fadeManager;

    [Header("DATA")]
    public List<MonsterSO> monsterSOData = new List<MonsterSO>();
    public List<NatureSO> natureSOData = new List<NatureSO>();
    //public List<VariantSO> variantSOData = new List<VariantSO>();
    public List<MoveSO> basicSOData = new List<MoveSO>();
    public List<MoveSO> specialSOData = new List<MoveSO>();
    //public List<MoveSO> moveSOData = new List<MoveSO>();
    public List<PassiveSO> passiveSOData = new List<PassiveSO>();
    public List<Node> nodesData = new List<Node>();
    public List<MonsterItemSO> monsterItemData = new List<MonsterItemSO>();

    public List<GameObject> roamerPrefabData = new List<GameObject>();

    public List<bool> objectivesComplete = new List<bool>(); // (+1 to id to get the object)
                                                             // 1 = completedFirstBattle(node 12 + 21),
    public List<bool> nodesCompleted = new List<bool>();
    public List<bool> nodesEntered = new List<bool>();
    public List<int> survivalBest = new List<int>();
    public List<int> numOfBeastsSeenIDs = new List<int>();

    public double avgTeamLevel;

    public PlayerData playerData;

    [HideInInspector]public Node cNode;
    [HideInInspector] public Node pNode;

    public string monsterType;
    public int monsterNumInStorage;
    


    [HideInInspector] public bool invulnerableDebug = false;
    private void Awake()
    {
        if (SaveSystem.GetSave() == false) // First time!
        {
            playerHP = 1000f;
            playerName = "Kid";
            playerManager.StartNew();

            for (int i = 0; i < nodesData.Count; i++)
            {
                nodesData[i].SetComplete(nodesCompleted[i]);
            }

            for (int i = 0; i < nodesData.Count; i++)
            {
                nodesData[i].SetEntered(nodesEntered[i]);
            }
        }
        else
        {
            LoadData();
            // LOAD NODES
            //Set current and prev location

            if (nodesCompleted[8])
            {
                overworldUI.partyButtonParent.SetActive(true);
            }
            else
            {
                overworldUI.partyButtonParent.SetActive(false);
            }

            for (int i = 0; i < nodesData.Count; i++)
            {
                if (nodesData[i].id == playerData.currentLocation)
                {
                    //Debug.Log("Current id: " + nodesData[i].id);
                    //Debug.Log("Current loc: " + playerData.currentLocation);
                    cNode = nodesData[i];
                }

                if (nodesData[i].id == playerData.previousLocation)
                {
                    //Debug.Log("Prev id: " + nodesData[i].id);
                    //Debug.Log("Prev loc: " + playerData.previousLocation);
                    pNode = nodesData[i];
                }
            }
            
            //Set nodes to complete
            
            for (int i = 0; i < nodesData.Count; i++)
            {
                nodesData[i].SetComplete(nodesCompleted[i]);
            }

            for (int i = 0; i < nodesData.Count; i++)
            {
                nodesData[i].SetEntered(nodesEntered[i]);
            }

            if (cNode != null)
            {
                playerManager.StartLoad(cNode, pNode);
            }
            else
            {
                Debug.Log("Error in Load");
            }

            for (int i = 0; i < playerData.act1RoamersPrefab.Count; i++)
            {
                for (int l = 0; l < roamerPrefabData.Count; l++)
                {
                    if (roamerPrefabData[l].GetComponent<Roamer>().id == playerData.act1RoamersPrefab[i])
                    {
                        for (int x = 0; x < nodesData.Count; x++)
                        {
                            if (nodesData[x].id == playerData.act1RoamersCurrentNode[i])
                            {
                                playerManager.currentRoamerController.SpawnRoamerFromLoad(roamerPrefabData[l], nodesData[x]);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }
        

        // LOAD FROM THE SAVE
    }

    public void LoadData()
    {
        playerData = SaveSystem.LoadPlayer();

        playerName = playerData.playerName;
        playerHP = playerData.playerHP;
        playerHomeNodeID = playerData.homeID;


        if (playerData.levelCap > levelCap)
        {
            levelCap = playerData.levelCap;
        }

        if (playerData.numOfBagsCollection > numOfBagsCollection)
        {
            numOfBagsCollection = playerData.numOfBagsCollection;
        }

        if (playerData.numOfBagsMaterials > numOfBagsMaterials)
        {
            numOfBagsMaterials = playerData.numOfBagsMaterials;
        }

        if (playerData.numOfBagsEquipment > numOfBagsEquipment)
        {
            numOfBagsEquipment = playerData.numOfBagsEquipment;
        }

        numOfBeastsSeenIDs = playerData.numOfBeastsSeenID;


        if (objectivesComplete.Count == playerData.objectivesComplete.Count)
        {
            objectivesComplete = playerData.objectivesComplete;

            //Debug.Log("Same");
        }
        else
        {
            if (objectivesComplete.Count < playerData.objectivesComplete.Count) //remove until the same
            {
                List<bool> tempObjs = new List<bool>();
                tempObjs = playerData.objectivesComplete;
                //Debug.Log("Start" + tempObjs.Count);
                while (objectivesComplete.Count != tempObjs.Count)
                {
                    
                    tempObjs.RemoveAt(tempObjs.Count - 1);
                }

                //Debug.Log("After" + tempObjs.Count);

              

                objectivesComplete = tempObjs;
            }
            else if (objectivesComplete.Count > playerData.objectivesComplete.Count) // add until the same
            {
                List<bool> tempObjs = new List<bool>();
                tempObjs = playerData.objectivesComplete;
                while (objectivesComplete.Count != tempObjs.Count)
                {
                    tempObjs.Add(false);
                }

                //Debug.Log("Add");
                objectivesComplete = tempObjs;
            }
        }

        


        if (nodesCompleted.Count == playerData.nodesComplete.Count) // if game count is bigger than save count
        {
            nodesCompleted = playerData.nodesComplete;
        }
        else
        {
            if (nodesCompleted.Count < playerData.nodesComplete.Count) //remove until the same
            {
                List<bool> tempNodes = new List<bool>();
                tempNodes = playerData.nodesComplete;
                while (nodesCompleted.Count != tempNodes.Count)
                {
                    tempNodes.RemoveAt(tempNodes.Count - 1);
                }

                nodesCompleted = tempNodes;
            }
            else if (nodesCompleted.Count > playerData.nodesComplete.Count) // add until the same
            {
                List<bool> tempNodes = new List<bool>();
                tempNodes = playerData.nodesComplete;
                while (nodesCompleted.Count != tempNodes.Count)
                {
                    tempNodes.Add(false);
                }

                nodesCompleted = tempNodes;
            }
        }

        if (nodesEntered.Count == playerData.nodesEntered.Count) // if game count is bigger than save count
        {
            nodesEntered = playerData.nodesEntered;
        }
        else
        {
            if (nodesEntered.Count < playerData.nodesEntered.Count) //remove until the same
            {
                List<bool> tempNodes = new List<bool>();
                tempNodes = playerData.nodesEntered;
                while (nodesEntered.Count != tempNodes.Count)
                {
                    tempNodes.RemoveAt(tempNodes.Count - 1);
                }

                nodesEntered = tempNodes;
            }
            else if (nodesEntered.Count > playerData.nodesEntered.Count) // add until the same
            {
                List<bool> tempNodes = new List<bool>();
                tempNodes = playerData.nodesEntered;
                while (nodesEntered.Count != tempNodes.Count)
                {
                    tempNodes.Add(false);
                }

                nodesEntered = tempNodes;
            }
        }


        if (survivalBest.Count == playerData.survivalBest.Count) // if game count is bigger than save count
        {
            survivalBest = playerData.survivalBest;
        }
        else
        {
            if (survivalBest.Count < playerData.survivalBest.Count) //remove until the same
            {
                List<int> tempSurv = new List<int>();
                tempSurv = playerData.survivalBest;
                while (survivalBest.Count != tempSurv.Count)
                {
                    tempSurv.RemoveAt(tempSurv.Count - 1);
                }

                survivalBest = tempSurv;
            }
            else if (survivalBest.Count > playerData.survivalBest.Count) // add until the same
            {
                List<int> tempSurv = new List<int>();
                tempSurv = playerData.survivalBest;
                while (survivalBest.Count != tempSurv.Count)
                {
                    tempSurv.Add(0);
                }

                survivalBest = tempSurv;
            }
        }



        for (int i = 0; i < playerData.ownedItems.Count; i++)
        {
            for (int j = 0; j < monsterItemData.Count; j++)
            {
                if (monsterItemData[j].id == playerData.ownedItems[i])
                {
                    collectionManager.AddItemToStorageWithID(monsterItemData[j], playerData.stackNumberOfItems[i], playerData.itemsStoredID[i]);
                    break;
                }
            }
            
        }

        collectionManager.ClearAllMonstersFromCollection();
        collectionManager.ClearAllMonstersFromParty();

        for (int i = 0; i < playerData.mNames.Count; i++) // for every monster in save data
        {
            //Natures
            int nat = 0;
            for (int j = 0; j < natureSOData.Count; j++)
            {
                if (natureSOData[j].id == playerData.mNatures[i])
                {
                    nat = j;
                }
            }
            

            //DATA
            int dt = 0;
            for (int j = 0; j < monsterSOData.Count; j++)
            {
                if (monsterSOData[j].ID.ID == playerData.mData[i])
                {
                    dt = j;
                }
            }

            //Variants
            int var = 0;
            for (int j = 0; j < monsterSOData[dt].possibleVariants.Count; j++)
            {
                if (monsterSOData[dt].possibleVariants[j].variant.id == playerData.mVariants[i])
                {
                    var = j;
                }
            }


            MoveSO bMove = ScriptableObject.CreateInstance<MoveSO>();
            MoveSO sMove = ScriptableObject.CreateInstance<MoveSO>();
            PassiveSO pMove = ScriptableObject.CreateInstance<PassiveSO>();

            for (int j = 0; j < basicSOData.Count; j++)
            {
                if (basicSOData[j].id == playerData.mBasic[i])
                {
                    bMove = basicSOData[j];
                }
            }

            for (int j = 0; j < specialSOData.Count; j++)
            {
                if (specialSOData[j].id == playerData.mSpecial[i])
                {
                    sMove = specialSOData[j];
                }
            }
    
            for (int j = 0; j < passiveSOData.Count; j++)
            {
                if (passiveSOData[j].id == playerData.mPassive[i])
                {
                    pMove = passiveSOData[j];
                }

            }
            //Color
            Color colo = new Color(playerData.mColoursR[i], playerData.mColoursG[i], playerData.mColoursB[i], playerData.mColoursA[i]);
            ColourRoll col = new ColourRoll(playerData.mColoursRarity[i], playerData.mColoursNumber[i], colo, playerData.mColoursRolled[i], playerData.mColoursTotalRolled[i]);



            //Stats

            List<Stat> st = new List<Stat>();

            st.Add(new Stat("Oomph", playerData.mStat1[i]));
            st.Add(new Stat("Guts", playerData.mStat2[i]));
            st.Add(new Stat("Juice", playerData.mStat3[i]));
            st.Add(new Stat("Edge", playerData.mStat4[i]));
            st.Add(new Stat("Wits", playerData.mStat5[i]));
            st.Add(new Stat("Spark", playerData.mStat6[i]));

            List<MonsterItemSO> ownedItems = new List<MonsterItemSO>();

            bool slot1Full = false;
            bool slot2Full = false;
            bool slot3Full = false;
            //1
            for (int j = 0; j < monsterItemData.Count; j++)
            {
                if (monsterItemData[j].id == playerData.mEquipItem1[i])
                {
                    ownedItems.Add(monsterItemData[j]);
                    slot1Full = true;
                    break;
                }
            }

            if (!slot1Full)
            {
                ownedItems.Add(monsterItemData[0]);
            }
            //2
            for (int j = 0; j < monsterItemData.Count; j++)
            {
                if (monsterItemData[j].id == playerData.mEquipItem2[i])
                {
                    ownedItems.Add(monsterItemData[j]);
                    slot2Full = true;
                    break;
                }
            }

            if (!slot2Full)
            {
                ownedItems.Add(monsterItemData[0]);
            }
            //3
            for (int j = 0; j < monsterItemData.Count; j++)
            {
                if (monsterItemData[j].id == playerData.mEquipItem3[i])
                {
                    ownedItems.Add(monsterItemData[j]);
                    slot3Full = true;
                    break;
                }
            }

            if (!slot3Full)
            {
                ownedItems.Add(monsterItemData[0]);
            }


            int capLevel = 0;

            if (playerData.mCapLevels.Count > 0)
            {
                capLevel = playerData.mCapLevels[i];
            }


            Monster mon = new Monster(playerData.mNames[i], playerData.mLevels[i], capLevel, playerData.mXPs[i], playerData.mStatPoints[i], playerData.mSymbiotics[i], natureSOData[nat], monsterSOData[dt].possibleVariants[var].variant, playerData.mStranges[i], col, st, monsterSOData[dt], bMove, sMove, pMove, ownedItems);





            if (playerData.mInParty[i]) // in party
            {
                collectionManager.SpawnMonsterInParty(mon, playerData.mStoredID[i]);
            }
            else if (!playerData.mInParty[i]) //in collection
            {
                collectionManager.SpawnMonsterInCollectionWithID(mon, playerData.mStoredID[i]);
            }


        }
    }

    public void MovePlayerHome()
    {
        playerManager.isMoving = false;


        for (int i = 0; i < nodesData.Count; i++)
        {
            if (nodesData[i].id == playerHomeNodeID)
            {
                cNode = nodesData[i];
            }
        }

        if (cNode != null)
        {
            playerManager.StartLoad(cNode, pNode);
            SaveData();
        }
        else
        {
            Debug.Log("Error in Load");
        }

    }

    public bool PassageClear(Node currNode) // if is battle/challenge node, if completed is false, only allow moving back to previous node, if completed is true, allow all movement
    {
        bool state = false;

        if (currNode.nodeType == NodeType.Battle || currNode.nodeType == NodeType.Challenge || currNode.nodeType == NodeType.Survival || currNode.nodeType == NodeType.Punk)
        {
            if (currNode.IsComplete())
            {
                state = true;
            }
            else
            {
                state = false;
            }
        }
        else
        {
            state = true;
        }

        return state;
    }

    //Node northNode, int nRankNeeded, int nMonstersNeeded, int nLevelNeeded, int nCapNeeded, int nObjNeeded, List<int> nNodesNeeded

    public bool CanPassToNode(int monsters, int lvl, int cap, int objs, List<int> nodes)
    {
        if (monsters == 0 && lvl == 0 && cap == 0 && objs == 0 && nodes.Count == 0)
        {
            return true;
        }


        //check total monsters

        int mvalue = 0;
        bool monstersState = false;

        for (int i = 0; i < playerData.mNames.Count; i++)
        {
            mvalue++;
        }

        if (mvalue >= lvl)
        {
            monstersState = true;
        }


        //Check monster levels

        int lvalue = 0;
        bool levelState = false;

        for (int i = 0; i < playerData.mLevels.Count; i++)
        {
            lvalue = lvalue + playerData.mLevels[i];
        }


        if (lvalue >= lvl)
        {
            levelState = true;
        }

        //Check cap
        bool capState = false;
        if (levelCap >= cap)
        {
            capState = true;
        }


        // Check objectives

        bool objState = true;

        for (int i = 0; i < objectivesComplete.Count; i++)
        {
            if (objs == i + 1)
            {
                if (!objectivesComplete[i])
                {
                    objState = false;
                }
            }
        }



        // Check Nodes
        bool nodeState = true;

        for (int i = 0; i < nodes.Count; i++) // nodes == 17, 46
        {
            if (!playerData.nodesComplete[nodes[i]])
            {
                nodeState = false;
            }
        }

        bool ret;

        if (levelState && objState & monstersState && capState & nodeState)
        {
            ret = true;
        }
        else
        {
            ret = false;
        }

        //Debug.Log("Level Value: " + value);
        //Debug.Log("Level State: " + levelState);

        //Debug.Log("Objectives Value: " + objs);
        //Debug.Log("Objectives State: " + objState);

        return ret;
    }



    public void AddBeastToSeenIDs(MonsterSO mon)
    {
        bool seen = false;

        for (int i = 0; i < numOfBeastsSeenIDs.Count; i++)
        {
            if (mon.ID.ID == numOfBeastsSeenIDs[i])
            {
                seen = true;
            }
        }

        if (!seen)
        {
            numOfBeastsSeenIDs.Add(mon.ID.ID);
        }
    }


    public void SaveData()
    {
        //Debug.Log("Saved");

        for (int i = 0; i < nodesData.Count; i++)
        {
            nodesCompleted[i] = nodesData[i].IsComplete();
            nodesEntered[i] = nodesData[i].IsEntered();

        }



        PlayerData data = new PlayerData(this);

        playerData = data;


        SaveSystem.SavePlayer(playerData);
    }

    public void ResetSaveAndCloseGame()
    {
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath + "/Saves/");

        foreach (string item in filePaths)
        {
            File.Delete(item);
        }

        Application.Quit();
    }

    

    public void Quit()
    {
        Application.Quit();
    }


    public void OpenItemInspectTooltip(MonsterItemSO item)
    {
        itemInspectManagerPopup.SpawnInspectPanel(item, this);
    }

    public void OpenItemInspectTooltipEquipped(MonsterItemSO item, int partySlot, int equipSlot, TeamEquipSlot tSlot)
    {
        itemInspectManagerPopup.SpawnInspectPanel(item, this, partySlot, equipSlot, tSlot);
    }

    public void CloseItemInspectTooltip()
    {
        itemInspectManagerPopup.currentPanel.GetComponent<ItemInspectPopup>().ClosePanel();
    }

    //Random management/debug methods

    public void LevelPartyPlusOne()
    {
        for (int i = 0; i < collectionManager.partySlots.Count; i++)
        {
            if (collectionManager.partySlots[i].storedMonsterObject != null)
            {
                collectionManager.partySlots[i].storedMonsterObject.GetComponent<PartySlot>().storedMonster.level++;
            }
        }
    }

    public void FullyHealSelf()
    {
        playerHP = 1000f;
        battleManager.friendlyMonsterController.healthBar.SetHealth(playerHP, false);
        overworldUI.healthBar.SetHealth(playerHP, false);
        
    }

    public void Invulnerable()
    {
        invulnerableDebug = !invulnerableDebug;
    }
   
}



