using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SurvivalSubMenu : MonoBehaviour
{
    public GameManager GM;
    public SurvivalMenu survivalMenu;


    public GameObject dropPrefab;
    public GameObject monPrefab;

    public RectTransform dropsGrid;
    public RectTransform monsGrid;

    public GameObject background;

    public TextMeshProUGUI gainedXPText;
    public TextMeshProUGUI bonusText;
    public TextMeshProUGUI totalText;

    public TextMeshProUGUI roundText;
    public TextMeshProUGUI currentBestText;

    public TextMeshProUGUI currentXPBonusText;
    public TextMeshProUGUI currentEnemyStatBuffText;

    public TextMeshProUGUI nextRoundXPBonusText;
    public TextMeshProUGUI nextRoundEnemyStatBuffText;


    public TextMeshProUGUI neededText;
    public EnemyMonsterController controller;

    private List<Monster> capturedMonsters = new List<Monster>();
    private int gainedGroupXP = 0;
    private int numberOfMonsInParty = 0;
    private List<float> inBattleTimes = new List<float>();
    private int survivalStreak = 0;
    private int survivalID = 0;

    public int scoreNeededToPass = 0;

    private int xpBuffPerRound = 30;
    private int statBuffPerRound = 40;



    private List<GameObject> drops = new List<GameObject>();
    private List<GameObject> mons = new List<GameObject>();
    public void Init(Monster capturedMonster, int groupXP, int numInParty, List<float> inBattleTime, int streak, int id) // with capture
    {
        background.SetActive(true);

        capturedMonsters.Add(capturedMonster);

        numberOfMonsInParty = numInParty;
        gainedGroupXP = groupXP;
        inBattleTimes = inBattleTime;
        survivalStreak = streak;
        survivalID = id;


        UpdateSubMenuVisuals();

    }

    public void Init(int groupXP, int numInParty, List<float> inBattleTime, int streak, int id)
    {
        background.SetActive(true);

        numberOfMonsInParty = numInParty;
        gainedGroupXP = groupXP;
        inBattleTimes = inBattleTime;
        survivalStreak = streak;
        survivalID = id;

        UpdateSubMenuVisuals();
    }

    private void UpdateSubMenuVisuals()
    {
        if (survivalStreak >= scoreNeededToPass)
        {
            neededText.text = "Retreat now to unlock the next node";
        }
        else
        {
            neededText.text = "Needed To Pass\n" + scoreNeededToPass.ToString();
        }

        gainedXPText.text = gainedGroupXP.ToString() + "xp";
        bonusText.text = "+" + survivalStreak * xpBuffPerRound + "%";
        
        float perc = survivalStreak / 10f;
        int amount = gainedGroupXP + (int)(gainedGroupXP * perc);
        Debug.Log(perc);
        Debug.Log(amount);
        totalText.text = amount.ToString() + "xp";

        roundText.text = "Round " + survivalStreak.ToString() + " Complete!";

        currentBestText.text = GM.survivalBest[survivalID].ToString();

        currentXPBonusText.text = survivalStreak * xpBuffPerRound + "%";
        currentEnemyStatBuffText.text = survivalStreak * statBuffPerRound + "%";

        nextRoundXPBonusText.text = "+" + xpBuffPerRound + "%";
        nextRoundEnemyStatBuffText.text = "+" + statBuffPerRound + "%";


        UpdateItemsVisuals();
        UpdateMonsterVisuals();

    }


    private void UpdateItemsVisuals()
    {
        for (int i = 0; i < GM.battleManager.rewardedItems.Count; i++)
        {
            GameObject obj = Instantiate(dropPrefab, dropsGrid);
            obj.GetComponent<DropSlot>().Init(GM.battleManager.rewardedItems[i], GM);
            drops.Add(obj);
        }
    }

    private void UpdateMonsterVisuals()
    {
        for (int i = 0; i < capturedMonsters.Count; i++)
        {
            GameObject obj = Instantiate(monPrefab, monsGrid);
            obj.GetComponent<BeastSlot>().Init(capturedMonsters[i], GM);
            mons.Add(obj);
        }
    }


    private void ClearItemsAndMonsVisuals()
    {
        for (int i = 0; i < drops.Count; i++)
        {
            Destroy(drops[i]);
        }

        for (int i = 0; i < mons.Count; i++)
        {
            Destroy(mons[i]);
        }

        drops = new List<GameObject>();
        mons = new List<GameObject>();
    }


    public void Retreat() // collect monsters, then drops and xp, then quit
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
        



        float perc = survivalStreak / 10f;
        int amount = gainedGroupXP + (int)(gainedGroupXP * perc);

        GM.DoBattleAftermath("WON", inBattleTimes, amount, numberOfMonsInParty);
        GM.battleManager.groupXp = 0;

        GM.SaveData();
    }

    public void Continue() // goto next battle +10% xp, +20% all enemy stats, new monster
    {
        ClearItemsAndMonsVisuals();
        background.SetActive(false);

        GM.battleManager.StartBattle(survivalMenu.monsSpawns, survivalMenu.nodeType, survivalMenu.backG, survivalStreak, true);
        
    }
}
