using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionManager : MonoBehaviour
{
    [Header("Collection Spawning")]
    public Transform collectionContent;
    public Transform itemContent;
    public GameObject collectionSlotPrefab;
    public GameObject partySlotPrefab;
    public GameObject itemSlotPrefab;

    [Header("Items")]
    public MenuTabButtons monsterButton;
    public MenuTabButtons itemButton;

    [Header("Inspection Panels")]
    public CollectionInspectPanel rightInspectPanel;
    public CollectionInspectPanel leftInspectPanel;

    [Header("Collection")]
    public List<GameObject> collectionMonsters = new List<GameObject>();
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
    public PlayerInfoInterface playerInfoInterface;

    public int mode = 0;

    void Start()
    {
        for (int i = 0; i < collectionMonsters.Count; i++)
        {
            collectionMonsters[i].GetComponent<CollectionSlot>().manager = this;
        }
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
        GameObject mon = Instantiate(collectionSlotPrefab, collectionContent) as GameObject;

        collectionMonsters.Add(mon);
        mon.GetComponent<CollectionSlot>().Init(monster, this, collectionMonsters.Count - 1);

    }

    public void ClearMonsterFromParty(GameObject monsterObejct, int num)
    {
        partySlots[num - 1].storedMonsterObject = null;
        Destroy(monsterObejct);

    }

    public void ClearMonster(GameObject monsterObject)
    {
        collectionMonsters.Remove(monsterObject);
        Destroy(monsterObject);

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
        if (leftInspectPanel.panel.gameObject.activeSelf == true)
        {
            leftInspectPanel.itemEquipMenu.Hide();
        }

        if (rightInspectPanel.panel.gameObject.activeSelf == true)
        {
            rightInspectPanel.itemEquipMenu.Hide();
        }

        mode = 0;
        GM.SaveData();
        collectionInterface.SetActive(false);
        itemInterface.SetActive(false);
        partyInterface.SetActive(false);
        playerInterface.SetActive(false);
        mainInterface.SetActive(false);
    }

    public void OpenInterface()
    {
        mode = 1;
        UpdateCollectionItems();
        collectionInterface.SetActive(true);
        monsterButton.Select();
        itemButton.ResetSelection();
        itemInterface.SetActive(false);
        partyInterface.SetActive(true);
        mainInterface.SetActive(true);
    }

    public void OpenPartyInterface()
    {
        mode = 2;
        UpdateCollectionItems();
        collectionInterface.SetActive(false);
        monsterButton.Hide();
        itemButton.Hide();

        itemInterface.SetActive(false);
        partyInterface.SetActive(true);
        playerInterface.SetActive(true);
        mainInterface.SetActive(true);

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



        avgTeamLvlText.text = "Average Team Level: " + round.ToString();

        GM.avgTeamLevel = round;
    }

    

    public void UpdateCollectionItems()
    {
        UpdatePartyLevel();

        for (int i = 0; i < collectionMonsters.Count; i++)
        {
            collectionMonsters[i].GetComponent<CollectionSlot>().UpdateItem();
        }

        for (int i = 0; i < partySlots.Count; i++)
        {
            if (partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject != null)
            {
                partySlots[i].GetComponent<PartySlotManager>().storedMonsterObject.GetComponent<PartySlot>().UpdateItem();
            }
            
        }

        playerInfoInterface.UpdateInfo();
    }


    // MONSTER COLLECTOR TOOL
    public void ClearAllMonstersFromCollection()
    {
        for (int i = 0; i < collectionMonsters.Count; i++)
        {
            DestroyImmediate(collectionMonsters[i]);
        }

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
        int value = 0;

        for (int i = 0; i < partySlots.Count; i++)
        {
            if (partySlots[i].storedMonsterObject == null)
            {
                value = i + 1;
                break;
            }
        }
        return value;
    }

    public void SpawnMonsterInParty(Monster monster, int slot)
    {
        GameObject mon = Instantiate(partySlotPrefab, partySlots[slot - 1].gameObject.transform) as GameObject;

        partySlots[slot - 1].storedMonsterObject = mon;


        mon.GetComponent<PartySlot>().Init(monster, this, partySlots[slot - 1]);

        UpdatePartyLevel();
    }
}
