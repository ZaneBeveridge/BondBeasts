using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public GameManager GM;
    public AftermathUI aftermathUI;
    public Image dynamicImg;
    public Image staticImg;
    public Image variantImg;
    public GameObject levelUpObject;
    public GameObject continueObject;
    public GameObject buttonsObject;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI nameText2;
    public TextMeshProUGUI typeText2;
    public TextMeshProUGUI typeSText;
    public TextMeshProUGUI typePText;
    public TextMeshProUGUI typeSText2;
    public TextMeshProUGUI typePText2;

    public TextMeshProUGUI pointsLeftText;
    public TextMeshProUGUI pointsLeftText2;
    public List<TextMeshProUGUI> statsNum = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> statsNumAdded = new List<TextMeshProUGUI>();

    public List<TextMeshProUGUI> statsNum2 = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> statsNumAdded2 = new List<TextMeshProUGUI>();

    private List<int> addedStats = new List<int>();

    private Monster currentMonster;
    private int currentSlot = 0;

    private int pointsLefts = 0;

    private bool active = false;
    private LevelUpType levelUpType;

    void Update()
    {
        if (active)
        {
            if (pointsLefts > 0)
            {
                buttonsObject.SetActive(true);
                continueObject.SetActive(false);
            }
            else
            {
                buttonsObject.SetActive(false);
                continueObject.SetActive(true);
            }
        }
    }

    public void Init(Monster monster, int slot, LevelUpType type)
    {
        levelUpObject.SetActive(true);
        levelUpType = type;
        addedStats = new List<int>();
        addedStats.Add(0);
        addedStats.Add(0);
        addedStats.Add(0);
        addedStats.Add(0);
        addedStats.Add(0);
        addedStats.Add(0);


        currentMonster = monster;
        currentSlot = slot;

        dynamicImg.sprite = monster.dynamicSprite;
        staticImg.sprite = monster.staticSprite;
        variantImg.sprite = monster.variant.variantStillSprite;

        dynamicImg.color = monster.colour.colour;

        nameText.text = monster.name;
        nameText2.text = monster.name;
        int randAmount = 0;
        int chosenAmount = 0;


        if (monster.level > 0 && monster.level < 20) // from 1-19 BASE MON +3 rand +2 chosen, 1 item slot
        {
            randAmount = 6;
            chosenAmount = 4;
        }
        else if (monster.level >= 20 && monster.level < 40) // from 20-39 PRESTIGE 1 +2 rand +1 chosen, 2 item slot
        {
            randAmount = 4;
            chosenAmount = 2;
        }
        else if (monster.level >= 40 && monster.level < 60) // from 40-59 PRESTIGE 2 no stats, 3 item slot
        {
            randAmount = 0;
            chosenAmount = 0;
        }
        else if (monster.level >= 60 && monster.level < 80) // from 60-79 PRESTIGE 3 no stats, 3 item slot, second nature rolled
        {
            randAmount = 0;
            chosenAmount = 0;
        }
        else if (monster.level >= 80 && monster.level < 100) // from 80-99 PRESTIGE 4 no statsn, 3 item slot, may reroll nature
        {
            randAmount = 0;
            chosenAmount = 0;
        }

        if (monster.symbiotic)
        {
            typeText.text = "Symbiotic";
            typeText2.text = "Symbiotic";
            typeText.color = new Color(0f, 180f, 255f);

            pointsLefts = randAmount;
            for (int i = 0; i < pointsLefts; i++)
            {
                LevelARandomStat();
            }
            pointsLefts = 0;

            typeSText.gameObject.SetActive(true);
            typePText.gameObject.SetActive(false);
            typeSText2.gameObject.SetActive(true);
            typePText2.gameObject.SetActive(false);
        }
        else
        {
            typeText.text = "Parasitic";
            typeText2.text = "Parasitic";
            typeText.color = new Color(255f, 0, 0);

            pointsLefts = chosenAmount;

            typeSText.gameObject.SetActive(false);
            typePText.gameObject.SetActive(true);
            typeSText2.gameObject.SetActive(false);
            typePText2.gameObject.SetActive(true);
        }

        UpdateStatsWithMonsters();
        UpdateAddedStats();

        continueObject.SetActive(false);
        active = true;
    }


    private void LevelARandomStat()
    {
        int rand = Random.Range(0, 5);

        addedStats[rand]++;
    }

    public void Continue()
    {
        float xpReq = 100;
        for (int j = 0; j < currentMonster.level - 1; j++)
        {
            xpReq = xpReq * 1.2f;
        }

        currentMonster.level++;

        currentMonster.xp = currentMonster.xp - Mathf.RoundToInt(xpReq);

        for (int i = 0; i < currentMonster.stats.Count; i++)
        {
            currentMonster.stats[i].value += addedStats[i];
        }


        GM.collectionManager.partySlots[currentSlot].storedMonsterObject.GetComponent<PartySlot>().storedMonster = currentMonster;
        levelUpObject.SetActive(false);

        if (levelUpType == LevelUpType.Afterbattle)
        {
            aftermathUI.DoneLeveling(currentSlot);
        }
        else if (levelUpType == LevelUpType.Party)
        {
            GM.collectionManager.UpdateCollectionItems();
        }

        
    }

    public void AddStat(int index)
    {
        addedStats[index]++;
        pointsLefts--;

        UpdateAddedStats();
    }


    private void UpdateAddedStats()
    {
        pointsLeftText.text = "Points Left: " + pointsLefts.ToString();
        pointsLeftText2.text = "Points Left: " + pointsLefts.ToString();
        for (int i = 0; i < statsNumAdded.Count; i++)
        {
            if (addedStats[i] > 0)
            {
                statsNumAdded[i].text = "+" + addedStats[i].ToString();
                statsNumAdded2[i].text = "+" + addedStats[i].ToString();
            }
            else
            {
                statsNumAdded[i].text = "";
                statsNumAdded2[i].text = "";
            }
            
        }


    }

    private void UpdateStatsWithMonsters()
    {
        for (int i = 0; i < statsNum.Count; i++)
        {
            statsNum[i].text = (currentMonster.stats[i].value + addedStats[i]).ToString();
            statsNum2[i].text = (currentMonster.stats[i].value + addedStats[i]).ToString();
        }
    }
}

public enum LevelUpType
{
    Afterbattle,
    Party
}
