using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleType
{
    Battle,
    Punk,
    ChallengeCrawl
}

public class BattleManager : MonoBehaviour
{
    [Header("References")]
    public GameManager GM;
    public IntroBattleTimelineController battleIntroTimeline;
    public GameObject fightingIntroObject;
    public SpriteRenderer backgroundImage;

    public SpriteRenderer friendlyPunk;
    public SpriteRenderer enemyPunk;

    public List<ButtonTagSlot> tagButtons = new List<ButtonTagSlot>();

    [Header("Friendly")]
    public FriendlyMonsterController friendlyMonsterController;

    public int slotSelected;
    public int enemySlotSelected;

    public CaptureButton captureButton;

    [Header("Enemy")]
    public EnemyMonsterController enemyMonsterController;

    [Header("XP/Values")]
    public int xpGainedPerLevel = 25;

    public List<StoredItem> rewardedItems = new List<StoredItem>();


    private List<StoredItem> actualStoredItems = new List<StoredItem>();

    [Header("Survival")]
    public SurvivalMenu survivalMenu;
    public SurvivalSubMenu survivalSubMenu;

    public bool survival = false;
    public int survivalStreak = 0;
    public int survivalID = 0;

    [Header("TESTING")]
    public List<MonsterSpawn> testMonster;

    private NodeType gameType;


    public int groupXp = 0;

    public bool controlsActive = true;

    public bool isPlayingIntro = true;

    public float enemyCapturePoints = 100f;

    private AlphaRoamer currentRoamer;

    //public float extraCaptureTimeEnemyLevel = 0f;
    //public float extraCaptureTimeMissingHealth = 0f;

    public Animator cameraAnimator;

    void Start()
    {
        // FOR TESTING 
        //InitBattle(testMonster, false);
        cameraAnimator.SetInteger("Focus", 1);
    }

    public void CalcRewardSurv()
    {
        

        if (actualStoredItems.Count > 0)
        {
            for (int i = 0; i < actualStoredItems.Count; i++)
            {
                if (rewardedItems.Count > 0)
                {
                    bool passed = false;

                    for (int j = 0; j < rewardedItems.Count; j++)
                    {
                        if (actualStoredItems[i].item.id == rewardedItems[j].item.id)
                        {
                            rewardedItems[j].amount += actualStoredItems[i].amount;
                            passed = true;
                            Debug.Log("MergeItems");
                            break;
                            
                        }
                    }

                    if (!passed)
                    {
                        rewardedItems.Add(actualStoredItems[i]);
                        Debug.Log("NewItems");
                        break;
                    }
                }
                else
                {
                    Debug.Log("NoItems");
                    rewardedItems.Add(actualStoredItems[i]);
                }

            }
        }
    }


    private void CalcReward(MonsterSpawn spawn, NodeType type)
    {
        List<StoredItem> rewardedIt = new List<StoredItem>();

        for (int i = 0; i < spawn.monster.itemDrops.Count; i++)
        {
            float random = Random.Range(0f, 100f);

            if (spawn.monster.itemDrops[i].chance >= random)
            {
                int randomAmount = Random.Range(spawn.monster.itemDrops[i].minDrops, spawn.monster.itemDrops[i].maxDrops);

                rewardedIt.Add(new StoredItem(spawn.monster.itemDrops[i].item, randomAmount));
            }
        }


        if (type == NodeType.Survival)
        {
            actualStoredItems = rewardedIt;
        }
        else
        {
            rewardedItems = rewardedIt;
        }

        
    }

    private void CalcRewardNode(List<ItemDrop> drops)
    {
        List<StoredItem> rewardedIt = new List<StoredItem>();

        for (int i = 0; i < drops.Count; i++)
        {
            float random = Random.Range(0f, 100f);

            if (drops[i].chance >= random)
            {
                int randomAmount = Random.Range(drops[i].minDrops, drops[i].maxDrops);

                rewardedIt.Add(new StoredItem(drops[i].item, randomAmount));
            }
        }

        rewardedItems = rewardedIt;
    }

    public void InitSurvival(List<MonsterSpawn> mons, NodeType type, Sprite background, int id, int scoreNeededToPass)
    {
        survival = true;
        rewardedItems = new List<StoredItem>();
        actualStoredItems = new List<StoredItem>();

        survivalStreak = 0;
        survivalID = id;

        survivalSubMenu.scoreNeededToPass = scoreNeededToPass;
        survivalMenu.Init(mons, type, background, id, scoreNeededToPass);

        friendlyMonsterController.inBattleTime = new List<float>();
        friendlyMonsterController.inBattleTime.Add(0f);
        friendlyMonsterController.inBattleTime.Add(0f);
        friendlyMonsterController.inBattleTime.Add(0f);


    }

    public void InitBattle(List<MonsterSpawn> mons, NodeType type, Sprite background)
    {
        survival = false;
        rewardedItems = new List<StoredItem>();
        actualStoredItems = new List<StoredItem>();
        friendlyMonsterController.inBattleTime = new List<float>();
        friendlyMonsterController.inBattleTime.Add(0f);
        friendlyMonsterController.inBattleTime.Add(0f);
        friendlyMonsterController.inBattleTime.Add(0f);

        StartBattle(mons, type, background, 0, true);
    }

    public void InitPunk(List<Monster> mons, NodeType type, Sprite background, List<ItemDrop> drops, int punkMaxHealth)
    {
        survival = false;
        rewardedItems = new List<StoredItem>();
        actualStoredItems = new List<StoredItem>();
        friendlyMonsterController.inBattleTime = new List<float>();
        friendlyMonsterController.inBattleTime.Add(0f);
        friendlyMonsterController.inBattleTime.Add(0f);
        friendlyMonsterController.inBattleTime.Add(0f);

        StartBattlePunk(mons, type, background, 0, drops, true, punkMaxHealth);
    }


    public void InitAlpha(List<Monster> mons, Sprite background, List<ItemDrop> drops, AlphaRoamer roamer, int roamerMaxHealth)
    {
        survival = false;
        currentRoamer = roamer;
        rewardedItems = new List<StoredItem>();
        actualStoredItems = new List<StoredItem>();
        friendlyMonsterController.inBattleTime = new List<float>();
        friendlyMonsterController.inBattleTime.Add(0f);
        friendlyMonsterController.inBattleTime.Add(0f);
        friendlyMonsterController.inBattleTime.Add(0f);

        StartBattlePunk(mons, NodeType.Roamer, background, 0, drops, false, roamerMaxHealth);
    }

    public void StartBattle(List<MonsterSpawn> mons, NodeType type, Sprite background, int extraStats, bool captureOn)
    {
        gameType = type;
        
        isPlayingIntro = true;
        // Sets up the battle.
        // Generates the monster to fight
        // Sets the initial player monster to be switch in to first found going from top slot to bottom
        // Sets the BATTLE button to true and positons in the middle of the screen


        
        friendlyPunk.gameObject.SetActive(true);
        enemyPunk.gameObject.SetActive(false);

        if (captureOn)
        {
            GM.battleUI.captureButton.SetActive(true);
            GM.battleUI.enemyHealthBar.ActivateCapBar(true);
        }
        else
        {
            GM.battleUI.captureButton.SetActive(false);
            GM.battleUI.enemyHealthBar.ActivateCapBar(false);
        }

        

        GM.battleUI.gameObject.SetActive(true);
        GM.battleGameobject.SetActive(true);
        backgroundImage.sprite = background;
        enemyMonsterController.RefreshCooldowns();
        friendlyMonsterController.RefreshCooldowns();

        if (type == NodeType.Survival)
        {
            List<MonsterSpawn> monsSurvival = new List<MonsterSpawn>();

            float total = 0;

            for (int j = 0; j < mons.Count; j++)
            {
                total += mons[j].weight;
            }

            float random = Random.Range(0f, total);



            float addUp = 0;
            for (int j = 0; j < mons.Count; j++)
            {
                addUp = addUp + mons[j].weight;

                if (random <= addUp)
                {
                    monsSurvival.Add(mons[j]);
                    break;
                }
            }

            if (monsSurvival.Count > 1)
            {
                GM.battleUI.enemyMultiMonsterSwitches.SetActive(true);
            }
            else
            {
                GM.battleUI.enemyMultiMonsterSwitches.SetActive(false);
            }


            CalcReward(monsSurvival[0], type);
            enemyMonsterController.SetupEnemy(monsSurvival, type, extraStats);
        }
        else
        {
            if (mons.Count > 1)
            {
                GM.battleUI.enemyMultiMonsterSwitches.SetActive(true);
            }
            else
            {
                GM.battleUI.enemyMultiMonsterSwitches.SetActive(false);
            }

            CalcReward(mons[0], type);
            enemyMonsterController.SetupEnemy(mons, type, 0);
        }

        
        friendlyMonsterController.healthBar.SetMaxHealth(100);
        friendlyMonsterController.healthBar.SetHealth(GM.playerHP, false);

        
        for (int i = 0; i < 3; i++)
        {
            if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                friendlyMonsterController.SetFriendlyMonster(i);
                break;
            }
        }


        for (int i = 0; i < 3; i++)
        {
            if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                if (i == 0)
                {
                    GM.battleUI.tagSprites1.tagSprites.UpdateArt(GM.collectionManager.partySlots[0].storedMonsterObject.GetComponent<PartySlot>().storedMonster);
                }
                else if (i == 1)
                {
                    GM.battleUI.tagSprites2.tagSprites.UpdateArt(GM.collectionManager.partySlots[1].storedMonsterObject.GetComponent<PartySlot>().storedMonster);
                }
                else if (i == 2)
                {
                    GM.battleUI.tagSprites3.tagSprites.UpdateArt(GM.collectionManager.partySlots[2].storedMonsterObject.GetComponent<PartySlot>().storedMonster);
                }
            }
        }

        

        //PLAY ENTER ANIM HERE
        fightingIntroObject.SetActive(true);

        battleIntroTimeline.Play(enemyMonsterController.currentMonster);
        
    }

    public void StartBattlePunk(List<Monster> mons, NodeType type, Sprite background, int extraStats, List<ItemDrop> drops, bool punkOn, int punkMaxHealth)
    {
        gameType = type;

        isPlayingIntro = true;
        // Sets up the battle.
        // Generates the monster to fight
        // Sets the initial player monster to be switch in to first found going from top slot to bottom
        // Sets the BATTLE button to true and positons in the middle of the screen

        if (mons.Count > 1)
        {
            GM.battleUI.enemyMultiMonsterSwitches.SetActive(true);
        }
        else
        {
            GM.battleUI.enemyMultiMonsterSwitches.SetActive(false);
        }
        
        if (punkOn)
        {
            enemyPunk.gameObject.SetActive(true);
        }
        else
        {
            enemyPunk.gameObject.SetActive(false);
        }
            
        friendlyPunk.gameObject.SetActive(true);
        
        GM.battleUI.captureButton.SetActive(false);
        GM.battleUI.enemyHealthBar.ActivateCapBar(false);

        GM.battleUI.gameObject.SetActive(true);
        GM.battleGameobject.SetActive(true);
        backgroundImage.sprite = background;
        enemyMonsterController.RefreshCooldowns();
        friendlyMonsterController.RefreshCooldowns();

        CalcRewardNode(drops);
        enemyMonsterController.SetupEnemyPunk(mons, type, 0, punkMaxHealth);


        friendlyMonsterController.healthBar.SetMaxHealth(100);
        friendlyMonsterController.healthBar.SetHealth(GM.playerHP, false);


        for (int i = 0; i < 3; i++)
        {
            if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                friendlyMonsterController.SetFriendlyMonster(i);
                break;
            }
        }


        for (int i = 0; i < 3; i++)
        {
            if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                if (i == 0)
                {
                    GM.battleUI.tagSprites1.tagSprites.UpdateArt(GM.collectionManager.partySlots[0].storedMonsterObject.GetComponent<PartySlot>().storedMonster);
                }
                else if (i == 1)
                {
                    GM.battleUI.tagSprites2.tagSprites.UpdateArt(GM.collectionManager.partySlots[1].storedMonsterObject.GetComponent<PartySlot>().storedMonster);
                }
                else if (i == 2)
                {
                    GM.battleUI.tagSprites3.tagSprites.UpdateArt(GM.collectionManager.partySlots[2].storedMonsterObject.GetComponent<PartySlot>().storedMonster);
                }
            }
        }

        //PLAY ENTER ANIM HERE
        fightingIntroObject.SetActive(true);

        battleIntroTimeline.Play(enemyMonsterController.currentMonster);

    }

    public void EnterStart()
    {
        //Debug.Log("Start");
        isPlayingIntro = true;
        PauseControls();

        friendlyMonsterController.alive = false;
        enemyMonsterController.ActivateAI(false);
    }

    public void EnterDone()
    {
        //Debug.Log("Start");
        isPlayingIntro = false;
        ResumeControls();

        friendlyMonsterController.alive = true;
        enemyMonsterController.ActivateAI(true);

        float averageLevel = 0f;
        int numOfMons = 0;

        for (int i = 0; i < 3; i++)
        {
            if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                averageLevel += GM.collectionManager.partySlots[i].storedMonsterObject.GetComponent<PartySlot>().storedMonster.level;
                numOfMons++;
            }
        }

        averageLevel = averageLevel / numOfMons;


        float averageLevelEnemy = enemyMonsterController.currentMonster.level;
        int numOfMonsEnemy = 1;

        for (int i = 0; i < enemyMonsterController.backupMonsters.Count; i++)
        {
            averageLevelEnemy += enemyMonsterController.backupMonsters[i].level;
            numOfMonsEnemy++;
        }

        averageLevelEnemy = averageLevelEnemy / numOfMonsEnemy;

        float enemyDifference = averageLevelEnemy - averageLevel;

        if (enemyDifference < 1)
        {
            enemyCapturePoints = 100f;
        }
        else
        {
            enemyCapturePoints = 100 + (enemyDifference * 40f);
        }
        //Debug.Log(averageLevelEnemy);
        //Debug.Log(enemyDifference);
        //Debug.Log(enemyCapturePoints);

        enemyMonsterController.capBar.SetMaxCapturePoints(enemyCapturePoints, enemyMonsterController.enemyHealth);
        enemyMonsterController.capBar.SetCaptureLevel(0f, enemyMonsterController.enemyHealth);
    }


    

    // BUTTONS

    public void Switch(int slot) // switchs monster to said slotted monster in party IF possible
    {
        if (controlsActive && friendlyMonsterController.tagReady[slot] && friendlyMonsterController.currentSlot != slot)
        {
            friendlyMonsterController.SetFriendlyMonster(slot);

            tagButtons[slot].anim.SetTrigger("Pressed");
        }
    }

    public void SwitchEnemy()
    {
        if (enemyMonsterController.stunned) { return; }

        for (int i = 0; i < enemyMonsterController.backupMonsters.Count; i++)
        {
            if (enemyMonsterController.tagReady[i] && enemyMonsterController.currentSlot != i)
            {
                enemyMonsterController.SetMonsterActive(i);
                break;
            }
        }
    }

    public void GetXP()
    {
        int baseXp = xpGainedPerLevel; // xpGainedPerLevel = 20
        for (int i = 0; i < enemyMonsterController.currentMonster.level - 1; i++)
        {
            baseXp = Mathf.RoundToInt(baseXp * 1.2f);
        }

        groupXp = groupXp + baseXp;
    }

    public void WinRoundSurvival(bool withCapture)
    {
        // Spawn sub menu
        friendlyMonsterController.alive = false;
        CleanUpProjectiles();
        enemyMonsterController.ActivateAI(false);

        //GM.battleGameobject.SetActive(false);
        GM.battleUI.gameObject.SetActive(false);


        int num = 0;
        for (int i = 0; i < 3; i++)
        {
            if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                num++;
            }
        }

        survivalStreak++;

        if (withCapture)
        {
            survivalSubMenu.Init(enemyMonsterController.currentMonster, groupXp, num, friendlyMonsterController.inBattleTime, survivalStreak, survivalID);
        }
        else
        {
            CalcRewardSurv();
            survivalSubMenu.Init(groupXp, num, friendlyMonsterController.inBattleTime, survivalStreak, survivalID);
        }
    }

    public void Win()
    {

        friendlyMonsterController.alive = false;
        CleanUpProjectiles();
        enemyMonsterController.ActivateAI(false);

        GM.playerManager.currentNode.SetComplete(true);
        GM.playerManager.currentNode.Refresh();

        GM.battleGameobject.SetActive(false);
        GM.battleUI.gameObject.SetActive(false);

        GM.overworldGameobject.SetActive(true);
        GM.overworldUI.gameObject.SetActive(true);


        if (currentRoamer != null)
        {
            groupXp = groupXp * 2;
            currentRoamer.Kill();
            currentRoamer = null;
        }

        int num = 0;
        for (int i = 0; i < 3; i++)
        {
            if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                num++;
            }
        }


        GM.DoBattleAftermath("WON", friendlyMonsterController.inBattleTime, groupXp, num);
        groupXp = 0;

        GM.SaveData();
    }

    public void Lose()
    {
        friendlyMonsterController.alive = false;
        CleanUpProjectiles();
        enemyMonsterController.ActivateAI(false);

        GM.battleGameobject.SetActive(false);
        GM.battleUI.gameObject.SetActive(false);

        GM.overworldGameobject.SetActive(true);
        GM.overworldUI.gameObject.SetActive(true);

        int num = 0;
        for (int i = 0; i < 3; i++)
        {
            if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                num++;
            }
        }

        GM.DoBattleAftermath("LOST", friendlyMonsterController.inBattleTime, 0, num);
        groupXp = 0;
    }


    public void Capture() // Captures the monster
    {
        //PAUSE GAME
        if (survival)
        {
            WinRoundSurvival(true);
        }
        else
        {
            CleanUpProjectiles();
            enemyMonsterController.ActivateAI(false);

            List<Monster> ms = new List<Monster>();
            ms.Add(enemyMonsterController.currentMonster);

            GM.captureChoiceWindow.Init(ms, enemyMonsterController);
        }
    }

    public void Special() // Uses Special Attack
    {
        if (controlsActive)
        {
            friendlyMonsterController.Special();
        }
        
    }

    public void Attack() // Used Basic Attack
    {
        if (controlsActive)
        {
            friendlyMonsterController.Attack();
        }
        
    }

    public void PauseControls()
    {
        GM.battleUI.DisableControls();
        controlsActive = false;
    }

    public void PauseControlsExceptCapture()
    {
        GM.battleUI.DisableAllButCapture();
        controlsActive = false;
    }


    public void ResumeControls()
    {
        GM.battleUI.EnableControls();
        controlsActive = true;
    }

    private void CleanUpProjectiles()
    {
        for (int i = 0; i < friendlyMonsterController.projectiles.Count; i++)
        {
            Destroy(friendlyMonsterController.projectiles[i].gameObject);
        }

        for (int i = 0; i < enemyMonsterController.projectiles.Count; i++)
        {
            Destroy(enemyMonsterController.projectiles[i].gameObject);
        }

        friendlyMonsterController.projectiles = new List<GameObject>();
        enemyMonsterController.projectiles = new List<GameObject>();

        friendlyMonsterController.hitNumbers.End();
        enemyMonsterController.hitNumbers.End();


        friendlyMonsterController.ClearCooldowns();
        enemyMonsterController.ClearCooldowns();

        friendlyMonsterController.friendlyItemController.ClearItems();
        friendlyMonsterController.friendlyPassiveController.ClearPassives();

        friendlyMonsterController.friendlyBattleBuffManager.ClearSlotsAfterBattle();
        enemyMonsterController.enemyBattleBuffManager.ClearSlotsAfterBattle();

        friendlyMonsterController.stunManager.StopStun();
        enemyMonsterController.stunManager.StopStun();

        GM.battleUI.captureBute.ResetBar();

        

    }
}










