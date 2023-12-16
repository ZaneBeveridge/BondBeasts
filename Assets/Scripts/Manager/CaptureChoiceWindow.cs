using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaptureChoiceWindow : MonoBehaviour
{
    public GameManager GM;

    public GameObject firstObject;
    public Image dynamicImg;
    public Image staticImg;
    public Image variantImg;

    public GameObject symObject;
    public GameObject paraObject;
    public GameObject nameObject;
    public TMP_InputField inputField;

    public TextMeshProUGUI levelText;
    public GameObject amountsObject;
    public TextMeshProUGUI amountsText;

    public GameObject lvlsParent;
    public GameObject statPickParent;

    public TextMeshProUGUI typeText;

    public StatLeveler statLeveler;
    public Monster currentMonster;
    public EnemyMonsterController enemyMonsterController;
    private SurvivalSubMenu subMenu;
    private List<Monster> monsters = new List<Monster>();

    private bool survivalMode = false;

    public void Init(List<Monster> mons, EnemyMonsterController controller)
    {
        survivalMode = false;
        firstObject.SetActive(true);
        GM.battleUI.gameObject.SetActive(false);
        GM.battleManager.PauseControls();
        monsters = mons;
        enemyMonsterController = controller;

        NextMonster();
    }

    public void Init(List<Monster> mons, EnemyMonsterController controller, SurvivalSubMenu menu)
    {
        subMenu = menu;
        survivalMode = true;
        firstObject.SetActive(true);
        GM.battleUI.gameObject.SetActive(false);
        GM.battleManager.PauseControls();
        monsters = mons;
        enemyMonsterController = controller;

        NextMonster();
    }


    public void NextMonster()
    {
        if (monsters.Count > 0)
        {
            currentMonster = monsters[0];
            monsters.RemoveAt(0);
        }


        dynamicImg.sprite = currentMonster.dynamicSprite;
        dynamicImg.color = currentMonster.colour.colour;
        staticImg.sprite = currentMonster.staticSprite;
        variantImg.sprite = currentMonster.variant.variantStillSprite;

        levelText.text = "Level " + currentMonster.level.ToString();

        symObject.SetActive(true);
        paraObject.SetActive(true);
        nameObject.SetActive(false);
        lvlsParent.SetActive(true);
        amountsObject.SetActive(false);
        statPickParent.SetActive(false);
    }

    public void UpdateLevelAmounts(string type)
    {
        int firstAddedAmount = 6;
        int secondAddedAmount = 3;

        if (type == "Symbiotic")
        {
            firstAddedAmount = 6;
            secondAddedAmount = 3;
        }
        else if (type == "Parasitic")
        {
            firstAddedAmount = 4;
            secondAddedAmount = 2;
        }

        if (currentMonster.level == 1)
        {
            amountsObject.SetActive(false);
        }
        else if (currentMonster.level <= 20 && currentMonster.level > 1)
        {
            amountsObject.SetActive(true);
            if (currentMonster.level - 1 == 1)
            {
                amountsText.text = (currentMonster.level - 1).ToString() + " Level of +" + firstAddedAmount;
            }
            else
            {
                amountsText.text = (currentMonster.level - 1).ToString() + " Levels of +" + firstAddedAmount;
            }
            
        }
        else if (currentMonster.level > 20)
        {
            amountsObject.SetActive(true);
            if (currentMonster.level - 20 == 1)
            {
                amountsText.text = "19 Levels of +" + firstAddedAmount + "\n" + (currentMonster.level - 20).ToString() + " Level of +" + secondAddedAmount;
            }
            else
            {
                amountsText.text = "19 Levels of +" + firstAddedAmount + "\n" + (currentMonster.level - 20).ToString() + " Levels of +" + secondAddedAmount;
            }

            

        }

    }

    public void StatPick(string type)
    {
        if (type == "Symbiotic")
        {
            currentMonster.symbiotic = true;
            typeText.text = "Symbiotic";
            typeText.color = new Color(0f, 180f, 255f);
        }
        else if (type == "Parasitic")
        {
            currentMonster.symbiotic = false;
            typeText.text = "Parasitic";
            typeText.color = new Color(255f, 0, 0);
        }



        symObject.SetActive(false);
        paraObject.SetActive(false);
        nameObject.SetActive(false);
        lvlsParent.SetActive(false);
        statPickParent.SetActive(true);
        statLeveler.Init(currentMonster);

    }

    public void NamePick(List<int> addedStats)
    {
        for (int i = 0; i < currentMonster.stats.Count; i++)
        {
            if (addedStats[i] > 0)
            {
                currentMonster.AddStat(i, addedStats[i]);
                //monster.stats[i].value += addedStats[i];
            }    
            
        }

        
        symObject.SetActive(false);
        paraObject.SetActive(false);
        nameObject.SetActive(true);
        lvlsParent.SetActive(false);
        statPickParent.SetActive(false);
        inputField.text = currentMonster.name;
    }

    public void Fin()
    {
        currentMonster.name = inputField.text;
        ///enemyMonsterController.ActivateAI(true);
        GM.collectionManager.SpawnMonsterInCollection(currentMonster);
        currentMonster = null;
        

        if (monsters.Count > 0)
        {
            NextMonster();
        }
        else
        {
            if (survivalMode)
            {
                if (subMenu != null)
                {
                    subMenu.ToVictory();
                }
                firstObject.SetActive(false);
            }
            else
            {
                GM.playerManager.currentNode.SetComplete(true);
                GM.playerManager.currentNode.Refresh();

                enemyMonsterController.ActivateAI(false);


                GM.battleGameobject.SetActive(false);
                GM.battleUI.gameObject.SetActive(false);

                GM.overworldGameobject.SetActive(true);
                GM.overworldUI.gameObject.SetActive(true);

                GM.overworldUI.healthBar.SetHealth(GM.playerHP);

                List<float> tms = new List<float>();
                tms.Add(0);
                tms.Add(0);
                tms.Add(0);

                int num = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
                    {
                        num++;
                    }
                }


                GM.DoBattleAftermath("CAPTURE", tms, 0, num);

                GM.SaveData();

                firstObject.SetActive(false);
            }

            
        }

        
    }


}
