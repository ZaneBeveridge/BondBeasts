using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class CollectionManager : MonoBehaviour
{
    [Header("Collection Spawning")]
    public List<Transform> collectionContents = new List<Transform>();
    //public Transform itemContent;
    public GameObject collectionSlotPrefab;
    public GameObject partySlotPrefab;
    public GameObject bagSlotPrefab;
    public GameObject folderSlotPrefab;
    public GameObject rightArrowSlotPrefab;
    //public GameObject itemSlotPrefab;

    [Header("Items")]
    public List<StoredItem> itemsOwned = new List<StoredItem>();
    public Transform itemListContent;

    public GameObject equipmentSlotPrefab;

    public MenuTabButtons monsterButton;
    public MenuTabButtons itemButton;
    public MenuTabButtons playerInfoButton;

    public MonsterItemSO blankItem;

    [Header("Inspection Panels")]
    public CollectionInspectPanel rightInspectPanel;
    public CollectionInspectPanel leftInspectPanel;

    [Header("Collection")]
    public List<StoredMonster> collectionMonsters = new List<StoredMonster>();
    public TextMeshProUGUI avgTeamLvlText;


    [Header("References")]
    public GameManager GM;
    public List<PartySlotManager> partySlots = new List<PartySlotManager>();
    public GameObject buttonObject;
    public GameObject mainInterface;
    public GameObject partyInterface;
    public GameObject collectionInterface;
    public GameObject itemInterface;
    public GameObject playerInterface;
    public GameObject folderContents;
    public GameObject bagContents;
    public GameObject bagsWrapper;
    public GameObject leftArrowGameObject;

    public GameObject equipmentParent;
    public TextMeshProUGUI glitterCounterText;
    
    
    public PlayerInfoInterface playerInfoInterface;

    public int currentBag = 0;
    public int currentAmountOfCollectionSlots = 0;

    private List<GameObject> currentCollectionMonsters = new List<GameObject>();
    private List<StorageSlotManager> currentStorageBags = new List<StorageSlotManager>();
    private List<StorageSlotManager> currentStorageFolders = new List<StorageSlotManager>();
    private GameObject currentArrowObject;

    private List<GameObject> currentCollectionItems = new List<GameObject>();

    public int selectedBagTemp = 0;
    public int selectedFolderTemp = 0;

    private List<MasterStoredItems> tItems = new List<MasterStoredItems>();


    public int bagMode = 0;//0 = collection, 1 = equipment
    void Start()
    {
        /*
        for (int i = 0; i < collectionMonsters.Count; i++)
        {
            collectionMonsters[i].monster.GetComponent<CollectionSlot>().manager = this;
        }
        */
    }

    void Update()
    {
        bool canSave = false;

        for (int i = 0; i < partySlots.Count; i++)
        {
            if (partySlots[i].storedMonsterObject != null)
            {
                canSave = true;
                break;
            }

        }

        if (canSave)
        {
            buttonObject.SetActive(true);
        }
        else
        {
            buttonObject.SetActive(false);
        }
    }

    // COLLECTION MOVEMENTS
    public void SpawnMonsterInCollection(Monster monster)
    {
        // FIND NEXT POSSIBLE PLACE
        bool foundSpot = false;
        int countUp = 0;
        if (collectionMonsters.Count <= 0)
        {
            monster.storedID = countUp;
            StoredMonster storedMon = new StoredMonster(monster, countUp);
            collectionMonsters.Add(storedMon);
            return;
        }
        else
        {
            while (foundSpot == false)
            {
                bool found = true;
                for (int i = 0; i < collectionMonsters.Count; i++)
                {
                    if (collectionMonsters[i].storedID == countUp)
                    {
                        countUp++;
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    foundSpot = true;
                }
            }
            if (foundSpot)
            {
                monster.storedID = countUp;
                StoredMonster storedMon = new StoredMonster(monster, countUp);
                collectionMonsters.Add(storedMon);
            }
        }
    }

    public void SpawnMonsterInCollectionWithBag(Monster monster, int bagID)
    {
        BagSpace bagCheck = CheckSpaceInBag(bagID);

        if (bagCheck.state == true)
        {
            SpawnMonsterInCollectionWithID(monster, bagCheck.idEmpty);
        }
        else
        {
            Debug.Log("No Space in bag to place monster");
        }
    }

    public void SpawnMonsterInCollectionWithID(Monster monster, int storedID)
    {
        //FIND SPECIFIED SLOT TO SPAWN MON IN
        //Debug.Log("Spawning At: " + storedID);

        for (int i = 0; i < collectionMonsters.Count; i++)
        {
            if (collectionMonsters[i].storedID == storedID)
            {
                Debug.Log("Error: Tried to put monster into same stored ID slot as another IN COLLECTION");
                return;
            }
        }

        monster.storedID = storedID;
        StoredMonster storedMon = new StoredMonster(monster, storedID);
        collectionMonsters.Add(storedMon);
    }

    public void SpawnMonsterInParty(Monster monster, int slot)
    {
        GameObject mon = Instantiate(partySlotPrefab, partySlots[slot].gameObject.transform) as GameObject;
        partySlots[slot].storedMonsterObject = mon;
        monster.storedID = slot;
        mon.GetComponent<PartySlot>().Init(monster, this, partySlots[slot]);
        UpdatePartyLevel();
    }


    public void ClearMonsterFromParty(GameObject monsterObject, int num)
    {
        partySlots[num].storedMonsterObject = null;
        Destroy(monsterObject);
    }

    public void ClearMonster(Monster monsterObject)
    {
        for (int i = 0; i < collectionMonsters.Count; i++)
        {
            if (collectionMonsters[i].monster == monsterObject)
            {
                collectionMonsters.Remove(collectionMonsters[i]);
                break;
            }
        }
    }
    // ITEMS

    public void AddItemToStorage(MonsterItemSO itm, int amount) // Spawns an item in storage, adds the count to be higher
    {
        bool foundSpot = false;
        int countUp = 0;

        bool merge = false;
        int mergeId = 0;

        if (itemsOwned.Count > 0)
        {
            for (int j = 0; j < itemsOwned.Count; j++)
            {
                if (itemsOwned[j].item == itm)
                {
                    mergeId = j;
                    merge = true;
                    break;
                }
            }
        }

        if (merge)
        {
            itemsOwned[mergeId].amount += amount;
        }
        else
        {
            if (itemsOwned.Count <= 0)
            {
                StoredItem storedItem = new StoredItem(itm, amount, countUp);
                itemsOwned.Add(storedItem);
                return;
            }
            else
            {
                while (foundSpot == false)
                {
                    bool found = true;
                    for (int i = 0; i < itemsOwned.Count; i++)
                    {
                        if (itemsOwned[i].storedID == countUp && itemsOwned[i].item.type == itm.type)
                        {
                            countUp++;
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        foundSpot = true;
                    }
                }

                if (foundSpot)
                {
                    StoredItem storedItem = new StoredItem(itm, amount, countUp);
                    itemsOwned.Add(storedItem);
                }
            }
        }
    }

    public void AddItemToStorageWithBag(MonsterItemSO itm, int amount, int bagID)
    {
        BagSpace bagCheck = CheckSpaceInBag(bagID);

        if (bagCheck.state == true)
        {
            AddItemToStorageWithID(itm, amount, bagCheck.idEmpty);
        }
        else
        {
            Debug.Log("No Space in bag to place item");
        }
    }

    public void AddItemToStorageWithID(MonsterItemSO itm, int amount, int storedID)
    {
        bool merge = false;
        int mergeId = 0;

        if (itemsOwned.Count > 0)
        {
            for (int j = 0; j < itemsOwned.Count; j++)
            {
                if (itemsOwned[j].item == itm)
                {
                    mergeId = j;
                    merge = true;
                    break;
                }
            }
        }

        if (merge)
        {
            itemsOwned[mergeId].amount += amount;
        }
        else
        {
            StoredItem storedItem = new StoredItem(itm, amount, storedID);
            itemsOwned.Add(storedItem);
            return;
        }

    }

    public void RemoveItemFromStorage(MonsterItemSO itm)
    {
        for (int j = 0; j < itemsOwned.Count; j++)
        {
            if (itemsOwned[j].item == itm && itemsOwned[j].amount > 0)
            {
                itemsOwned[j].amount -= 1;
                break;
            }
            //else if (itemsOwned[j].item == itm && itemsOwned[j].amount <= 1)
            //{
            //    itemsOwned.RemoveAt(j);
            //}
        }

    }

    public void RemoveItemFromStorage(MonsterItemSO itm, int amount)
    {
        for (int j = 0; j < itemsOwned.Count; j++)
        {
            if (itemsOwned[j].item == itm && itemsOwned[j].amount > 0)
            {
                itemsOwned[j].amount -= amount;
                //if (itemsOwned[j].amount <= 0)
                //{
                //    itemsOwned.RemoveAt(j);
                //}
                break;
            }
            //else if (itemsOwned[j].item == itm && itemsOwned[j].amount <= 1)
            //{
            //    itemsOwned.RemoveAt(j);
            //}
        }

    }

    public void RemoveItemFromMonsterInParty(int slotInParty, int equipSlotNum)
    {
        if (partySlots[slotInParty].storedMonsterObject != null)
        {
            Monster mon = partySlots[slotInParty].storedMonsterObject.GetComponent<PartySlot>().storedMonster;

            if (equipSlotNum == 1)
            {
                mon.item1 = blankItem;
            }
            else if (equipSlotNum == 2)
            {
                mon.item2 = blankItem;
            }
            else if (equipSlotNum == 3)
            {
                mon.item3 = blankItem;
            }

            partySlots[slotInParty].storedMonsterObject.GetComponent<PartySlot>().storedMonster = mon;
            //Debug.Log("RemoveFromParty: Party Slot: " + slotInParty + ", EquipSlot: " + equipSlotNum);
        }
    }

    public void AddItemToMonsterInParty(MonsterItemSO itm, int slotInParty, int equipSlotNum)
    {
        if (partySlots[slotInParty].storedMonsterObject != null)
        {
            Monster mon = partySlots[slotInParty].storedMonsterObject.GetComponent<PartySlot>().storedMonster;

            if (equipSlotNum == 1)
            {
                mon.item1 = itm;
            }
            else if (equipSlotNum == 2)
            {
                mon.item2 = itm;
            }
            else if (equipSlotNum == 3)
            {
                mon.item3 = itm;
            }

            partySlots[slotInParty].storedMonsterObject.GetComponent<PartySlot>().storedMonster = mon;

        }
    }

    public void ChangeMonsterNameInParty(int slotInParty, string newName)
    {
        if (partySlots[slotInParty].storedMonsterObject != null)
        {
            partySlots[slotInParty].storedMonsterObject.GetComponent<PartySlot>().storedMonster.name = newName;
        }
    }

    public void UpdatePartyMonsters() // NOT NEEDED BUT NOT SURE WHY LOL
    {
        for (int i = 0; i < collectionMonsters.Count; i++)
        {
            if (collectionMonsters[i].storedID <= 2)
            {
                collectionMonsters[i].monster = partySlots[collectionMonsters[i].storedID].storedMonsterObject.GetComponent<PartySlot>().storedMonster;
            }
        }
    }

    //INSPECTION PANELS
    public void OpenCollectionInspect(Monster monster, GameObject obj, string type, int slotNum)
    {
        rightInspectPanel.OpenPanel(monster, obj, GM, type, slotNum);
    }

    public void OpenPartyInspect(Monster monster, GameObject obj, string type, int slotNum)
    {
        leftInspectPanel.OpenPanel(monster, obj, GM, type, slotNum);
    }

    //BUTTON FUNCTIONALITY

    public void PressExit()
    {
        GM.SaveData();
        collectionInterface.SetActive(false);
        itemInterface.SetActive(false);
        partyInterface.SetActive(true);
        playerInterface.SetActive(false);
        mainInterface.SetActive(false);
        monsterButton.Hide(true);
        itemButton.Hide(true);
        playerInfoButton.Hide(true);
    }

    public void OpenInterface()
    {
        bagMode = 0;
        UpdateStorage(0, 0);
        mainInterface.SetActive(true);
        collectionInterface.SetActive(true);
        monsterButton.Select(true);
        itemInterface.SetActive(false);
        partyInterface.SetActive(true);
        playerInterface.SetActive(false);
    }

    public void UpdatePartyLevel()
    {
        float avgLevel = 0;
        float count = 0;

        for (int i = 0; i < partySlots.Count; i++)
        {
            if (partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject != null)
            {
                avgLevel += partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject.GetComponent<PartySlot>().storedMonster.level;
                count++;
            }
            else
            {
                avgLevel += 1f;
                count++;
            }

        }

        //Debug.Log(avgLevel);


        float str = avgLevel / count;

        double round = System.Math.Round(str, 2);



        avgTeamLvlText.text = round.ToString();

        GM.avgTeamLevel = round;
    }

    public void ActivateEquipMode(bool state)
    {
        if (state) // turn on
        {
            for (int i = 0; i < partySlots.Count; i++)
            {
                PartySlotManager pManager = partySlots[i].GetComponent<PartySlotManager>();

                if (pManager.storedMonsterObject != null)
                {
                    pManager.storedMonsterObject.GetComponent<PartySlot>().EquipMode(true);
                }
            }
        }
        else // turn off
        {
            for (int i = 0; i < partySlots.Count; i++)
            {
                PartySlotManager pManager = partySlots[i].GetComponent<PartySlotManager>();

                if (pManager.storedMonsterObject != null)
                {
                    pManager.storedMonsterObject.GetComponent<PartySlot>().EquipMode(false);
                }
            }
        }
    }
    

    public void UpdateCollectionBeasts(int bagID) // bagID: 0,1,2 etc
    {
        UpdatePartyLevel();

        currentAmountOfCollectionSlots = 10;

        int minIDRange = (bagID * currentAmountOfCollectionSlots);
        int maxIDRange = (bagID * currentAmountOfCollectionSlots) + 9; // max collection size on page here

        for (int i = 0; i < currentCollectionMonsters.Count; i++)
        {
            Destroy(currentCollectionMonsters[i]);
        }

        currentCollectionMonsters = new List<GameObject>();
       
        for (int i = 0; i < collectionMonsters.Count; i++)
        {
            if (collectionMonsters[i].storedID >= minIDRange && collectionMonsters[i].storedID <= maxIDRange) // if monster ID range is within the selected bags range of stored IDs
            {
                int realSlot = collectionMonsters[i].storedID - (currentAmountOfCollectionSlots * bagID);
                GameObject mon = Instantiate(collectionSlotPrefab, collectionContents[realSlot]);
                mon.GetComponent<CollectionSlot>().Init(collectionMonsters[i].monster, this, collectionMonsters[i].storedID);
                currentCollectionMonsters.Add(mon);
            }
        }

        for (int i = 0; i < partySlots.Count; i++)
        {
            if (partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject != null)
            {
                partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject.GetComponent<PartySlot>().UpdateItem();
            }
        }
        
        ActivateEquipMode(false);
    }

    public void UpdateCollectionEquipment() // bagID: 0,1,2 etc
    {
        for (int i = 0; i < currentCollectionItems.Count; i++)
        {
            Destroy(currentCollectionItems[i]);
        }

        currentCollectionItems = new List<GameObject>();

        List<MasterStoredItems> tempItems = new List<MasterStoredItems>();


        for (int i = 0; i < itemsOwned.Count; i++)
        {
            if (itemsOwned[i].item.type == ItemType.Catalyst)
            {
                //Merge
                int tmN = itemsOwned[i].item.ownGroupedId;
                bool merge = false;
                for (int j = 0; j < tempItems.Count; j++)
                {
                    if (tempItems[j].groupID == itemsOwned[i].item.groupedId)
                    {
                        //Add to same group
                        if (tmN == 1){tempItems[j].item01 = itemsOwned[i];}
                        else if (tmN == 2){tempItems[j].item02 = itemsOwned[i];}
                        else if (tmN == 3){tempItems[j].item03 = itemsOwned[i];}
                        merge = true;
                        break;
                    }
                }
                //Add new

                if (!merge)
                {
                    StoredItem blankI = new StoredItem(blankItem, 1, 0);
                    MasterStoredItems masterItem = new MasterStoredItems(blankI, blankI, blankI, itemsOwned[i].item.groupedId);
                    if (tmN == 1)
                    {
                        masterItem = new MasterStoredItems(itemsOwned[i], blankI, blankI, itemsOwned[i].item.groupedId);
                    }
                    else if (tmN == 2)
                    {
                        masterItem = new MasterStoredItems(blankI, itemsOwned[i], blankI, itemsOwned[i].item.groupedId);
                    }
                    else if (tmN == 3)
                    {
                        masterItem = new MasterStoredItems(blankI, blankI, itemsOwned[i], itemsOwned[i].item.groupedId);
                    }
                    tempItems.Add(masterItem);
                }
            }
        }
        tItems = tempItems;


        for (int i = 0; i < tempItems.Count; i++)
        {
            GameObject itm = Instantiate(equipmentSlotPrefab, itemListContent);
            itm.GetComponent<MasterEquipmentSlot>().Init(tempItems[i], GM);
            currentCollectionItems.Add(itm);
        }


        int glitterAmount = 0;
        for (int i = 0; i < itemsOwned.Count; i++)
        {
            if (itemsOwned[i].item.id == 1) // Glitter
            {
                glitterAmount = itemsOwned[i].amount;
                break;
            }
        }

        glitterCounterText.text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", glitterAmount);

        ActivateEquipMode(true);

        /*
        currentAmountOfCollectionSlots = 15;

        int minIDRange = (bagID * currentAmountOfCollectionSlots);
        int maxIDRange = (bagID * currentAmountOfCollectionSlots) + 14; // max collection size on page here

        */
    }

    public void PressLeftStorage()
    {
        if (selectedBagTemp - 1 < 0 && selectedFolderTemp > 0) // if should swap folders back
        {
            UpdateStorage(8, selectedFolderTemp - 1);
        }
        else
        {
            UpdateStorage(selectedBagTemp - 1, selectedFolderTemp);
        }

        
    }

    public void PressRightStorage()
    {
        int num = 0; // 0=collection 1=equipment

        switch (bagMode)
        {
            case 0:
                num = GM.numOfBagsCollection;
                break;
            case 1:
                num = GM.numOfBagsEquipment;
                break;
        }

        if (selectedBagTemp + 1 >= 9 && selectedFolderTemp + 1 <= num / 9) 
        {
            UpdateStorage(0, selectedFolderTemp + 1);
        }
        else
        {
            UpdateStorage(selectedBagTemp + 1, selectedFolderTemp);
        }
    }

    public void PressButtonBag(int bagID)
    {
        

        int selBag = bagID - (selectedFolderTemp * 9);
        int selFolder = Mathf.FloorToInt((bagID) / 9);

        UpdateStorage(selBag, selFolder);
    }

    public void PressButtonFolder(int folderID)
    {
        UpdateStorage(0, folderID);
    }

    public void PressTabButton(int num) // 0 Monsters, 1 Items, 2 Player Info
    {
        if (num == 0) // Monsters
        {
            bagMode = 0;
            bagsWrapper.SetActive(true);
            collectionInterface.SetActive(true);
            UpdateStorage(0, 0);
            itemInterface.SetActive(false);
            playerInterface.SetActive(false);
            monsterButton.Select(false);
            itemButton.Hide(true);
            playerInfoButton.Hide(true);

        }
        else if (num == 1) // Items
        {
            bagMode = 1;
            bagsWrapper.SetActive(true);
            itemInterface.SetActive(true);
            UpdateStorage(0, 0);
            collectionInterface.SetActive(false);
            playerInterface.SetActive(false);
            equipmentParent.SetActive(true);
            monsterButton.Hide(true);
            itemButton.Select(false);
            playerInfoButton.Hide(true);
        }
        else if (num == 2) // Player Info
        {
            ActivateEquipMode(false);
            playerInfoInterface.UpdateInfo();
            bagsWrapper.SetActive(false);
            collectionInterface.SetActive(false);
            itemInterface.SetActive(false);
            playerInterface.SetActive(true);
            monsterButton.Hide(true);
            itemButton.Hide(true);
            playerInfoButton.Select(false);
        }
    }
    
    public BagSpace CheckSpaceInBag(int bagID)
    {
        BagSpace state = new BagSpace(false, 0);
        int addedExtraLow = 0;
        int addedExtraHigh = 0;
        bool foundSpot = false;

        switch (bagMode)
        {
            case 0:
                addedExtraLow = 0;
                addedExtraHigh = 9;
                break;
            case 1:
                addedExtraLow = 0;
                addedExtraHigh = 14;
                break;
        }

        int minIDRange = (bagID * currentAmountOfCollectionSlots) + addedExtraLow;
        int maxIDRange = (bagID * currentAmountOfCollectionSlots) + addedExtraHigh; // max collection size on page here

        int countUp = minIDRange;

        if (bagMode == 0) // monsters
        {
            while (foundSpot == false)
            {
                bool found = true;
                for (int i = 0; i < collectionMonsters.Count; i++)
                {
                    if (collectionMonsters[i].storedID == countUp)
                    {
                        countUp++;
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    if (countUp > maxIDRange)
                    {
                        return state;
                    }

                    foundSpot = true;
                    state = new BagSpace(true, countUp);
                }
            }
        }
        else if (bagMode == 1)
        {
            while (foundSpot == false)
            {
                bool found = true;
                for (int i = 0; i < itemsOwned.Count; i++)
                {
                    if (itemsOwned[i].storedID == countUp && itemsOwned[i].item.type == ItemType.Catalyst)
                    {
                        countUp++;
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    if (countUp > maxIDRange)
                    {
                        return state;
                    }

                    foundSpot = true;
                    state = new BagSpace(true, countUp);
                }
            }
        }


        return state;
    }

    public void UpdateStorage(int selectedBag, int selectedFolder)
    {
        int amountOfBags = 0; // 0=collection 1=equipment

        switch (bagMode)
        {
            case 0:
                amountOfBags = GM.numOfBagsCollection;
                break;
            case 1:
                amountOfBags = GM.numOfBagsEquipment;
                break;
        }


        int maxBagsPerFolder = 9;

        for (int i = 0; i < currentStorageBags.Count; i++)
        {
            Destroy(currentStorageBags[i].gameObject);
        }
        currentStorageBags = new List<StorageSlotManager>();

        for (int i = 0; i < currentStorageFolders.Count; i++)
        {
            Destroy(currentStorageFolders[i].gameObject);
        }
        currentStorageFolders = new List<StorageSlotManager>();

        Destroy(currentArrowObject);

        int amountOfBagsToSpawn = amountOfBags;
        int numberOfFolders = Mathf.CeilToInt(amountOfBags / 9f);
        int numberOfFoldersForBags = 1;

        while (amountOfBagsToSpawn > maxBagsPerFolder) // FIX HERE
        {
            if (selectedFolder == numberOfFoldersForBags - 1)
            {
                amountOfBagsToSpawn = maxBagsPerFolder;
            }
            else
            {
                amountOfBagsToSpawn -= maxBagsPerFolder;
            }
            numberOfFoldersForBags++;
        }

        //Debug.Log(numberOfFolders);
        //Debug.Log(numberOfFoldersForBags);
        //Debug.Log(amountOfBagsToSpawn);

        for (int i = 0; i < amountOfBagsToSpawn; i++)
        {
            GameObject bagSlot = Instantiate(bagSlotPrefab, bagContents.transform);
            StorageSlotManager bagSlotStorage = bagSlot.GetComponent<StorageSlotManager>();
            bagSlotStorage.Init(i + 1, i + (selectedFolder * maxBagsPerFolder), this);

            currentStorageBags.Add(bagSlotStorage);
        }

        if (amountOfBags > selectedBag + (selectedFolder * maxBagsPerFolder) + 1) // Right Arrow On
        {
            GameObject arrowRight = Instantiate(rightArrowSlotPrefab, bagContents.transform);
            arrowRight.GetComponent<BagsArrowRight>().GM = GM;
            currentArrowObject = arrowRight;
        }


        //Left Arrow

        if (selectedBag + (selectedFolder * maxBagsPerFolder) > 0)
        {
            leftArrowGameObject.SetActive(true);
        }
        else
        {
            leftArrowGameObject.SetActive(false);
        }

        if (numberOfFolders > 1)
        {
            for (int i = 0; i < numberOfFolders; i++)
            {
                GameObject folderSlot = Instantiate(folderSlotPrefab, folderContents.transform);
                StorageSlotManager folderSlotStorage = folderSlot.GetComponent<StorageSlotManager>();
                folderSlotStorage.Init(i + 1, i, this);

                currentStorageFolders.Add(folderSlotStorage);
            }
        }

        for (int i = 0; i < currentStorageBags.Count; i++)
        {
            if (i == selectedBag)
            {
                currentStorageBags[i].Select(true);
            }
            else
            {
                currentStorageBags[i].Select(false);
            }
        }

        for (int i = 0; i < currentStorageFolders.Count; i++)
        {
            if (i == selectedFolder)
            {
                currentStorageFolders[i].Select(true);
            }
            else
            {
                currentStorageFolders[i].Select(false);
            }
        }

        selectedBagTemp = selectedBag;
        selectedFolderTemp = selectedFolder;
        currentBag = selectedBag + (selectedFolder * maxBagsPerFolder);

        UpdateCollectionAll();

    }

    public void UpdateCollectionAll()
    {
        switch (bagMode)
        {
            case 0:
                UpdateCollectionBeasts(selectedBagTemp + (selectedFolderTemp * 9));
                break;
            case 1:
                UpdateCollectionEquipment();
                break;
        }
    }

    public void StartDrag(GameObject draggingObject) // for dragging objects around in collection and what happens when the drag starts
    {
        if (draggingObject.TryGetComponent<Slot>(out Slot slot)) // Party/Collection Drag
        {
            if (slot.type == SlotType.Collection)
            {
                for (int i = 0; i < partySlots.Count; i++)
                {
                    if (partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject != null)
                    {
                        PartySlot pSlot = partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject.GetComponent<PartySlot>();
                        pSlot.anim.SetBool("Holding", true);
                    }
                    else
                    {
                        PartySlotManager pSlotMan = partySlots[i].GetComponent<PartySlotManager>();
                        pSlotMan.SetDroppableArt(true);
                    }
                }

                //Jumping sprite on here, but gotta go through every single variant data to make this work
            }
            else if (slot.type == SlotType.Party)
            {
                for (int i = 0; i < partySlots.Count; i++)
                {
                    if (partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject != null && partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject != draggingObject)
                    {
                        PartySlot pSlot = partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject.GetComponent<PartySlot>();
                        pSlot.anim.SetBool("Holding", true);
                    }
                    else
                    {
                        PartySlotManager pSlotMan = partySlots[i].GetComponent<PartySlotManager>();
                        pSlotMan.SetDroppableArt(true);
                    }
                }
            }
        }
        else if (draggingObject.TryGetComponent<ItemSlot>(out ItemSlot itemSlot)) // Mats / Cata Drag
        {
            if (itemSlot.itemType == ItemType.Material)
            {
                // NOTHING ATM
            }
            else if (itemSlot.itemType == ItemType.Catalyst)
            {
                for (int i = 0; i < partySlots.Count; i++)
                {
                    if (partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject != null)
                    {
                        PartySlot pSlot = partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject.GetComponent<PartySlot>();

                        for (int j = 0; j < pSlot.equipSlots.Count; j++)
                        {
                            if (itemSlot.slotType == ItemSlotType.EquipSlot)
                            {
                                if (pSlot.equipSlots[j] != itemSlot.GetComponent<TeamEquipSlot>().tManager)
                                {
                                    pSlot.equipSlots[j].UpdateSlot(pSlot.storedMonster, true, GM);
                                }
                            }
                            else if (itemSlot.slotType == ItemSlotType.StorageSlot)
                            {
                                pSlot.equipSlots[j].UpdateSlot(pSlot.storedMonster, true, GM);
                            }
                            
                        }

                    }
                }
            }
        }

    }

    public void EndDrag(GameObject draggingObject)
    {
        if (draggingObject.TryGetComponent<Slot>(out Slot slot)) // Party/Collection Drag
        {
            if (slot.type == SlotType.Collection)
            {
                for (int i = 0; i < partySlots.Count; i++)
                {
                    if (partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject != null)
                    {
                        PartySlot pSlot = partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject.GetComponent<PartySlot>();
                        pSlot.anim.SetBool("Holding", false);
                    }
                    else
                    {
                        PartySlotManager pSlotMan = partySlots[i].GetComponent<PartySlotManager>();
                        pSlotMan.SetDroppableArt(false);
                    }
                }

                

            }
            else if (slot.type == SlotType.Party)
            {
                for (int i = 0; i < partySlots.Count; i++)
                {
                    if (partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject != null && partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject != draggingObject)
                    {
                        PartySlot pSlot = partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject.GetComponent<PartySlot>();
                        pSlot.anim.SetBool("Holding", false);
                    }
                    else
                    {
                        PartySlotManager pSlotMan = partySlots[i].GetComponent<PartySlotManager>();
                        pSlotMan.SetDroppableArt(false);
                    }
                }
            }
        }
        else if (draggingObject.TryGetComponent<ItemSlot>(out ItemSlot itemSlot)) // Mats / Cata Drag
        {
            if (itemSlot.itemType == ItemType.Material)
            {
                
            }
            else if (itemSlot.itemType == ItemType.Catalyst)
            {
                for (int i = 0; i < partySlots.Count; i++)
                {
                    if (partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject != null)
                    {
                        PartySlot pSlot = partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject.GetComponent<PartySlot>();

                        for (int j = 0; j < pSlot.equipSlots.Count; j++)
                        {
                            if (itemSlot.slotType == ItemSlotType.EquipSlot)
                            {
                                if (pSlot.equipSlots[j] != itemSlot.GetComponent<TeamEquipSlot>().tManager)
                                {
                                    pSlot.equipSlots[j].UpdateSlot(pSlot.storedMonster, false, GM);
                                }
                            }
                            else if (itemSlot.slotType == ItemSlotType.StorageSlot)
                            {
                                pSlot.equipSlots[j].UpdateSlot(pSlot.storedMonster, false, GM);
                            }
                        }

                    }
                }
            }
        }
    }

    // MONSTER COLLECTOR TOOL
    public void ClearAllMonstersFromCollection()
    {
        collectionMonsters.Clear();
    }

    public void ClearAllMonstersFromParty()
    {
        for (int i = 0; i < partySlots.Count; i++)
        {
            DestroyImmediate(partySlots[i].storedMonsterObject);
            partySlots[i].storedMonsterObject = null;
        }
    }

    public int CheckFreePartySlot()
    {
        int value = 0; // 3 == full 0,1,2 = space

        for (int i = 0; i < partySlots.Count; i++)
        {
            if (partySlots[i].storedMonsterObject == null)
            {
                break;
            }
            value = i + 1;
        }
        return value;
    }


   
}
[System.Serializable]
public class StoredMonster
{
    public Monster monster;
    public int storedID;

    public StoredMonster(Monster m, int s)
    {
        monster = m;
        storedID = s;
    }
}

[System.Serializable]
public class StoredItem
{
    public MonsterItemSO item;
    public int amount = 1;
    public int storedID = 0;

    public StoredItem(MonsterItemSO i, int a, int s)
    {
        item = i;
        amount = a;
        storedID = s;
    }
}

[System.Serializable]
public class BagSpace
{
    public bool state = false;
    public int idEmpty = 0;

    public BagSpace(bool s, int i)
    {
        state = s;
        idEmpty = i;
    }
}


[System.Serializable]
public class MasterStoredItems
{
    public int groupID;
    public StoredItem item01;
    public StoredItem item02;
    public StoredItem item03;

    public MasterStoredItems(StoredItem itm1, StoredItem itm2, StoredItem itm3, int id)
    {
        groupID = id;
        item01 = itm1;
        item02 = itm2;
        item03 = itm3;
    }
}
