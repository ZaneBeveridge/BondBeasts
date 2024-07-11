using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    //ACCOUNT INFO - PlayerName, total collected creatutes, stuff to show on the 'select a profile' place
    public string playerName; // NAME OF PROFILE
    public float playerHP;
    public int homeID;

    public int currentLocation;
    public int previousLocation;
    public int turnCount;

    //PROGRESSION INFO - What things has the player done, open gates, completed challenges, position on the map, etc etc

    public int levelCap;
    public int numOfBagsCollection;
    public int numOfBagsMaterials;
    public int numOfBagsEquipment;

    public List<int> act1RoamersPrefab = new List<int>();
    public List<int> act1RoamersCurrentNode = new List<int>();

    public List<bool> objectivesComplete = new List<bool>();
    public List<bool> nodesComplete = new List<bool>();
    public List<bool> nodesEntered= new List<bool>();

    public List<int> survivalBest = new List<int>();

    public List<int> ownedItems = new List<int>();
    public List<int> stackNumberOfItems = new List<int>();
    public List<int> itemsStoredID = new List<int>();

    public List<int> numOfBeastsSeenID = new List<int>();

    //COLLECTION INFO - The full collection of monsters the player has in storage AND in party, all unique details, name, level, xp, colour, stats, variant, nature, strange, and the orginal data used to create it, which contains static info on the monster

    //Monsters
    public List<string> mNames = new List<string>();
    public List<int> mLevels = new List<int>();
    public List<int> mCapLevels = new List<int>();
    public List<int> mXPs = new List<int>();
    public List<int> mStatPoints = new List<int>();
    public List<bool> mSymbiotics = new List<bool>();
    public List<int> mNatures = new List<int>();
    public List<int> mVariants = new List<int>();
    public List<bool> mStranges = new List<bool>();
    //Moves
    public List<int> mBasic = new List<int>();
    public List<int> mSpecial = new List<int>();
    public List<int> mPassive = new List<int>();
    //Colour
    public List<float> mColoursR = new List<float>();
    public List<float> mColoursG = new List<float>();
    public List<float> mColoursB = new List<float>();
    public List<float> mColoursA = new List<float>();

    public List<int> mColoursRarity = new List<int>();
    public List<int> mColoursNumber = new List<int>();
    public List<int> mColoursRolled = new List<int>();
    public List<int> mColoursTotalRolled = new List<int>();
    //Stats
    public List<int> mStat1 = new List<int>();
    public List<int> mStat2 = new List<int>();
    public List<int> mStat3 = new List<int>();
    public List<int> mStat4 = new List<int>();
    public List<int> mStat5 = new List<int>();
    public List<int> mStat6 = new List<int>();

    public List<int> mEquipItem1 = new List<int>();
    public List<int> mEquipItem2 = new List<int>();
    public List<int> mEquipItem3 = new List<int>();

    public List<int> mData = new List<int>();

    public List<int> mStoredID = new List<int>();
    public List<bool> mInParty = new List<bool>();

    public PlayerData(GameManager m)
    {
        playerName = m.playerName;
        playerHP = m.playerHP;
        homeID = m.playerHomeNodeID;

        currentLocation = m.playerManager.currentNode.id;
        previousLocation = m.playerManager.previousNode.id;

        levelCap = m.levelCap;

        numOfBagsCollection = m.numOfBagsCollection;
        numOfBagsMaterials = m.numOfBagsMaterials;
        numOfBagsEquipment = m.numOfBagsEquipment;

        numOfBeastsSeenID = m.numOfBeastsSeenIDs;


        for (int i = 0; i < m.playerManager.currentRoamerController.currentRoamers.Count; i++)
        {
            act1RoamersPrefab.Add(m.playerManager.currentRoamerController.currentRoamers[i].GetComponent<Roamer>().id);
            //Debug.Log("saved as id: " + m.playerManager.currentRoamerController.currentRoamers[i].GetComponent<Roamer>().currentNode.id);
            act1RoamersCurrentNode.Add(m.playerManager.currentRoamerController.currentRoamers[i].GetComponent<Roamer>().currentNode.id);
        }

        objectivesComplete = m.objectivesComplete;
        nodesComplete = m.nodesCompleted;
        nodesEntered = m.nodesEntered;
        survivalBest = m.survivalBest;

        for (int i = 0; i < m.collectionManager.itemsOwned.Count; i++)
        {
            ownedItems.Add(m.collectionManager.itemsOwned[i].item.id);
            stackNumberOfItems.Add(m.collectionManager.itemsOwned[i].amount);
            itemsStoredID.Add(m.collectionManager.itemsOwned[i].storedID);
        }

        for (int i = 0; i < m.collectionManager.partySlots.Count; i++)
        {
            if (m.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                Monster mon = m.collectionManager.partySlots[i].storedMonsterObject.GetComponent<PartySlot>().storedMonster;

                mNames.Add(mon.name);
                mLevels.Add(mon.level);
                mCapLevels.Add(mon.capLevel);
                mXPs.Add(mon.xp);
                mStatPoints.Add(mon.statPoints);
                mSymbiotics.Add(mon.symbiotic);
                mNatures.Add(mon.nature.id);
                mVariants.Add(mon.variant.id);
                mStranges.Add(mon.strange);

                mBasic.Add(mon.basicMove.id);
                mSpecial.Add(mon.specialMove.id);
                mPassive.Add(mon.passiveMove.id);

                mColoursR.Add(mon.colour.colour.r);
                mColoursG.Add(mon.colour.colour.g);
                mColoursB.Add(mon.colour.colour.b);
                mColoursA.Add(mon.colour.colour.a);

                mColoursRarity.Add(mon.colour.rarity);
                mColoursNumber.Add(mon.colour.number);
                mColoursRolled.Add(mon.colour.rolledNum);
                mColoursTotalRolled.Add(mon.colour.totalRoll);

                mStat1.Add(mon.stats[0].value);
                mStat2.Add(mon.stats[1].value);
                mStat3.Add(mon.stats[2].value);
                mStat4.Add(mon.stats[3].value);
                mStat5.Add(mon.stats[4].value);
                mStat6.Add(mon.stats[5].value);


                if (mon.item1 != null)
                {
                    mEquipItem1.Add(mon.item1.id);
                }
                else
                {
                    mEquipItem1.Add(0);
                }

                if (mon.item2 != null)
                {
                    mEquipItem2.Add(mon.item2.id);
                }
                else
                {
                    mEquipItem2.Add(0);
                }

                if (mon.item3 != null)
                {
                    mEquipItem3.Add(mon.item3.id);
                }
                else
                {
                    mEquipItem3.Add(0);
                }

                mData.Add(mon.backupData.ID.ID);

                mStoredID.Add(m.collectionManager.partySlots[i].slotNum - 1);

                mInParty.Add(true);
            }
            
        }


        for (int i = 0; i < m.collectionManager.collectionMonsters.Count; i++)
        {
            Monster mon = m.collectionManager.collectionMonsters[i].monster;

            mNames.Add(mon.name);
            mLevels.Add(mon.level);
            mCapLevels.Add(mon.capLevel);
            mXPs.Add(mon.xp);
            mStatPoints.Add(mon.statPoints);
            mSymbiotics.Add(mon.symbiotic);
            mNatures.Add(mon.nature.id);
            mVariants.Add(mon.variant.id);
            mStranges.Add(mon.strange);

            mBasic.Add(mon.basicMove.id);
            mSpecial.Add(mon.specialMove.id);
            mPassive.Add(mon.passiveMove.id);

            mColoursR.Add(mon.colour.colour.r);
            mColoursG.Add(mon.colour.colour.g);
            mColoursB.Add(mon.colour.colour.b);
            mColoursA.Add(mon.colour.colour.a);

            mColoursRarity.Add(mon.colour.rarity);
            mColoursNumber.Add(mon.colour.number);
            mColoursRolled.Add(mon.colour.rolledNum);
            mColoursTotalRolled.Add(mon.colour.totalRoll);

            mStat1.Add(mon.stats[0].value);
            mStat2.Add(mon.stats[1].value);
            mStat3.Add(mon.stats[2].value);
            mStat4.Add(mon.stats[3].value);
            mStat5.Add(mon.stats[4].value);
            mStat6.Add(mon.stats[5].value);


            if (mon.item1 != null)
            {
                mEquipItem1.Add(mon.item1.id);
            }
            else
            {
                mEquipItem1.Add(0);
            }

            if (mon.item2 != null)
            {
                mEquipItem2.Add(mon.item2.id);
            }
            else
            {
                mEquipItem2.Add(0);
            }

            if (mon.item3 != null)
            {
                mEquipItem3.Add(mon.item3.id);
            }
            else
            {
                mEquipItem3.Add(0);
            }

            mData.Add(mon.backupData.ID.ID);

            mStoredID.Add(m.collectionManager.collectionMonsters[i].storedID);

            mInParty.Add(false);
        }
    }

}

