using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatLeveler : MonoBehaviour
{
    public GameObject continueObject;
    public GameObject buttonsObject;
    public TextMeshProUGUI pointsLeftText;

    public List<TextMeshProUGUI> statsNum = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> statsNumAdded = new List<TextMeshProUGUI>();

    public CaptureChoiceWindow captureChoiceWindow;

    private List<int> addedStats = new List<int>();
    private int pointsLefts = 0;

    private bool active = false;


    private Monster currentMonster;

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

    public void Init(Monster monster)
    {
        addedStats = new List<int>();
        addedStats.Add(0);
        addedStats.Add(0);
        addedStats.Add(0);
        addedStats.Add(0);
        addedStats.Add(0);
        addedStats.Add(0);


        currentMonster = monster;

        

        if (monster.symbiotic)
        {
            for (int i = 0; i < monster.level - 1; i++)
            {
                if (i < 19) // below lvl 20
                {
                    pointsLefts += 6;
                }
                else if (i >= 19 && i < 39) // above lvl 20
                {
                    pointsLefts += 3;
                }
            }


            for (int i = 0; i < pointsLefts; i++)
            {
                LevelARandomStat();
            }

            pointsLefts = 0;
        }
        else
        {
            for (int i = 0; i < monster.level - 1; i++)
            {
                if (i < 19) // below lvl 20
                {
                    pointsLefts += 4;
                }
                else if (i >= 19 && i < 39) // above lvl 20
                {
                    pointsLefts += 2;
                }
            }
        }


        UpdateStatsWithMonsters();
        UpdateAddedStats();

        continueObject.SetActive(false);
        active = true;
    }
    public void AddStat(int index)
    {
        addedStats[index]++;
        pointsLefts--;

        UpdateStatsWithMonsters();
        UpdateAddedStats();
    }

    public void Continue() // HERERERERERERE
    {
        active = false;
        captureChoiceWindow.NamePick(addedStats);

    }

    private void UpdateAddedStats()
    {
        pointsLeftText.text = "Points Left: " + pointsLefts.ToString();

        for (int i = 0; i < statsNumAdded.Count; i++)
        {
            if (addedStats[i] > 0)
            {
                statsNumAdded[i].text = "+" + addedStats[i].ToString();
            }
            else
            {
                statsNumAdded[i].text = "";
            }
        }
    }



    private void UpdateStatsWithMonsters()
    {
        for (int i = 0; i < statsNum.Count; i++)
        {
            statsNum[i].text = currentMonster.stats[i].value.ToString();
        }
    }

    private void LevelARandomStat()
    {
        int rand = Random.Range(0, 5);

        addedStats[rand]++;
    }
}
