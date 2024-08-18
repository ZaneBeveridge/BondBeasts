using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SurvivalSubMenu : MonoBehaviour
{
    public GameManager GM;
    public SurvivalMenu survivalMenu;

    public Animator retreatButtonAnim;
    public Animator fightOnButtonAnim;

    public Animator loseRewardsAnim;
    public Animator enemyStatsAnim;

    public Animator clearRoundAnim;

    public GameObject dropPrefab;
    public GameObject monPrefab;

    public RectTransform dropsGrid;

    public GameObject leftArrow;
    public GameObject rightArrow;

    public GameObject background;

    public TextMeshProUGUI gainedXPText;
    public TextMeshProUGUI bonusText;

    public Color redTextColour;
    public Color greenTextColour;

    public TextMeshProUGUI clearToPassText;
    public Image clearToPassDot;
    public GameObject clearToPassTickImage;
    public GameObject clearToPassObject;

    public TextMeshProUGUI roundText;

    public EnemyMonsterController controller;

    private List<Monster> capturedMonsters = new List<Monster>();
    private List<Monster> tempMonsters = new List<Monster>();
    private int gainedGroupXP = 0;
    private int numberOfMonsInParty = 0;
    private List<float> inBattleTimes = new List<float>();
    private int survivalStreak = 0;
    private int survivalID = 0;

    public int scoreNeededToPass = 0;

    private int xpBuffPerRound = 40;
    //private .connect.tomybrain.iloveyou.cya
    //private int statBuffPerRound = 8;

    private List<StoredItem> items = new List<StoredItem>();
    private List<StoredMonster> mons = new List<StoredMonster>();

    private List<GameObject> drops = new List<GameObject>();

    private int currentPageID = 0;


    private bool readyToContinue = false;
    private bool readyToRetreat = false;
    public void Init(Monster capturedMonster, int groupXP, int numInParty, List<float> inBattleTime, int streak, int id) // with capture
    {
        currentPageID = 0;
        readyToContinue = false;
        readyToRetreat = false;

        background.SetActive(true);

        capturedMonsters.Add(capturedMonster);

        numberOfMonsInParty = numInParty;
        gainedGroupXP = groupXP;
        inBattleTimes = inBattleTime;
        survivalStreak = streak;
        survivalID = id;

        retreatButtonAnim.SetTrigger("Start");
        fightOnButtonAnim.SetTrigger("Start");

        UpdateSubMenuVisuals();
    }

    public void Init(int groupXP, int numInParty, List<float> inBattleTime, int streak, int id)
    {
        currentPageID = 0;
        readyToContinue = false;
        readyToRetreat = false;

        background.SetActive(true);

        numberOfMonsInParty = numInParty;
        gainedGroupXP = groupXP;
        inBattleTimes = inBattleTime;
        survivalStreak = streak;
        survivalID = id;

        retreatButtonAnim.SetTrigger("Start");
        fightOnButtonAnim.SetTrigger("Start");

        UpdateSubMenuVisuals();
    }

    private void UpdateSubMenuVisuals()
    {
        if (GM.survivalBest[survivalID] >= scoreNeededToPass)
        {
            clearToPassObject.SetActive(false);
        }
        else
        {
            clearToPassObject.SetActive(true);            
            if (survivalStreak >= scoreNeededToPass)
            {
                clearToPassText.text = "CLEAR ROUND " + scoreNeededToPass.ToString() + " TO PASS THIS NODE";
                clearToPassText.color = greenTextColour;
                clearToPassDot.color = greenTextColour;
                clearToPassTickImage.SetActive(true);
            }
            else
            {
                clearToPassText.text = "CLEAR ROUND " + scoreNeededToPass.ToString() + " TO PASS THIS NODE";
                clearToPassText.color = redTextColour;
                clearToPassDot.color = redTextColour;
                clearToPassTickImage.SetActive(false);
            }
        }

        

        gainedXPText.text = gainedGroupXP.ToString();
        bonusText.text = "+" + (survivalStreak - 1) * xpBuffPerRound + "%";
        
        roundText.text = survivalStreak.ToString();

        UpdateDropsStorage();
        UpdateDropsVisuals(0);
    }


    private void UpdateDropsStorage()
    {
        int storedID = 0;

        for (int i = 0; i < capturedMonsters.Count; i++)
        {
            StoredMonster mon = new StoredMonster(capturedMonsters[i], storedID);
            mons.Add(mon);
            storedID++;
        }

        for (int i = 0; i < GM.battleManager.rewardedItems.Count; i++)
        {
            StoredItem itm = new StoredItem(GM.battleManager.rewardedItems[i].item, GM.battleManager.rewardedItems[i].amount, storedID);
            items.Add(itm);
            storedID++;
        }
    }

    private void UpdateDropsVisuals(int pageID)
    {
        int amountOfDropsPerPage = 4;

        int minIDRange = (pageID * amountOfDropsPerPage);
        int maxIDRange = (pageID * amountOfDropsPerPage) + (amountOfDropsPerPage - 1); // max collection size on page here

        for (int i = 0; i < drops.Count; i++)
        {
            Destroy(drops[i]);
        }

        drops = new List<GameObject>();

        for (int i = 0; i < mons.Count; i++)
        {
            if (mons[i].storedID >= minIDRange && mons[i].storedID <= maxIDRange) // if monster ID range is within the selected bags range of stored IDs
            {
                GameObject obj = Instantiate(monPrefab, dropsGrid);
                obj.GetComponent<BeastSlot>().Init(mons[i].monster, GM);
                drops.Add(obj);
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].storedID >= minIDRange && items[i].storedID <= maxIDRange) // if monster ID range is within the selected bags range of stored IDs
            {
                GameObject obj = Instantiate(dropPrefab, dropsGrid);
                obj.GetComponent<DropSlot>().Init(items[i], GM);
                drops.Add(obj);
            }
        }

        if (pageID > 0)
        {
            leftArrow.SetActive(true);
        }
        else
        {
            leftArrow.SetActive(false);
        }

        int totalCount = mons.Count + items.Count;

        if (totalCount > (pageID + 1) * amountOfDropsPerPage)
        {
            rightArrow.SetActive(true);
        }
        else
        {
            rightArrow.SetActive(false);
        }

    }
    public void PressDropsButtonLeft()
    {
        if (currentPageID - 1 < 0) { return; }
        currentPageID--;
        UpdateDropsVisuals(currentPageID);
    }

    public void PressDropsButtonRight()
    {
        currentPageID++;
        UpdateDropsVisuals(currentPageID);
    }

    private void ClearItemsAndMonsVisuals()
    {
        mons = null;
        items = null;
        mons = new List<StoredMonster>();
        items = new List<StoredItem>();
    }

    public void ClearCaptureMons()
    {
        capturedMonsters = null;
        capturedMonsters = new List<Monster>();
    }

    private void UpdatePressedButtonVisuals() // after pressing either retreat or continue once, update the text visuals on screen, or start/stop playing anims
    {
        if (readyToContinue)
        {
            enemyStatsAnim.SetBool("Active", true);
            loseRewardsAnim.SetBool("Active", true);
        }
        else
        {
            enemyStatsAnim.SetBool("Active", false);
            loseRewardsAnim.SetBool("Active", false);
        }

        if (readyToRetreat)
        {
            clearRoundAnim.SetBool("Active", true);
        }
        else
        {
            clearRoundAnim.SetBool("Active", false);
        }
    }
    


    public void Retreat() // collect monsters, then drops and xp, then quit
    {
        tempMonsters = new List<Monster>();

        for (int i = 0; i < capturedMonsters.Count; i++)
        {
            tempMonsters.Add(capturedMonsters[i]);
        }

        if (readyToRetreat)
        {
            ClearItemsAndMonsVisuals();

            background.SetActive(false);

            

            if (capturedMonsters.Count > 0)
            {
                if (GM.survivalBest[survivalID] < survivalStreak)
                {
                    GM.survivalBest[survivalID] = survivalStreak;
                }

                GM.captureChoiceWindow.Init(capturedMonsters, controller, this);
            }
            else
            {
                ToVictory();
            }

            
        }
        else
        {
            readyToRetreat = true;
            retreatButtonAnim.SetTrigger("Pressed");

            if (readyToContinue)
            {
                readyToContinue = false;
                fightOnButtonAnim.SetTrigger("Unpressed");
            }
            

            UpdatePressedButtonVisuals();
        }

        

    }

    public void ToVictory()
    {
        // Spawn victory menu

        if (survivalStreak >= scoreNeededToPass)
        {
            GM.playerManager.currentNode.SetComplete(true);
        }

        
        GM.playerManager.currentNode.Refresh();

        GM.battleGameobject.SetActive(false);
        GM.battleUI.gameObject.SetActive(false);

        GM.overworldGameobject.SetActive(true);
        GM.overworldUI.gameObject.SetActive(true);

        if (GM.survivalBest[survivalID] < survivalStreak)
        {
            GM.survivalBest[survivalID] = survivalStreak;
        }


        //Debug.Log((gainedGroupXP));

        float perc = (survivalStreak - 1) * 0.4f;
        int amount = gainedGroupXP + (int)(gainedGroupXP * perc);


        List<StoredMonster> partyMons = new List<StoredMonster>();

        for (int i = 0; i < GM.collectionManager.partySlots.Count; i++)
        {
            if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                StoredMonster storedMon = new StoredMonster(GM.collectionManager.partySlots[i].storedMonsterObject.GetComponent<PartySlot>().storedMonster, i);
                partyMons.Add(storedMon);
            }
        }

        Debug.Log(tempMonsters.Count);


        //Debug.Log(amount);
        GM.aftermathUI.Init(inBattleTimes, amount, partyMons, tempMonsters);
        GM.battleManager.groupXp = 0;

        ClearCaptureMons();
        //GM.SaveData();
    }

    public void Continue() // goto next battle +10% xp, +20% all enemy stats, new monster
    {
        if (readyToContinue)
        {
            ClearItemsAndMonsVisuals();
            background.SetActive(false);

            GM.battleManager.StartBattle(survivalMenu.monsSpawns, survivalMenu.nodeType, survivalMenu.backG, survivalStreak, true);

            GM.overworldGameobject.SetActive(false);

        }
        else
        {
            readyToContinue = true;
            fightOnButtonAnim.SetTrigger("Pressed");

            if (readyToRetreat)
            {
                readyToRetreat = false;
                retreatButtonAnim.SetTrigger("Unpressed");
            }
            

            UpdatePressedButtonVisuals();
        }
        
    }
}
