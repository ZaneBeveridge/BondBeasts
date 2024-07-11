using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeastStarterMenuGUI : MonoBehaviour
{
    public GameManager GM;

    public GameObject title;
    public TextMeshProUGUI monName;
    public TextMeshProUGUI monDesc;

    [TextArea(5, 5)]
    public string monDesc01;
    [TextArea(5, 5)]
    public string monDesc02;
    [TextArea(5, 5)]
    public string monDesc03;

    public Animator inspectObject;
    public Animator confirmObject;
    public Animator finalStepObject;

    public GameObject normalParent;
    public GameObject focusParent;

    public GameObject mainObject;

    public Sprite startingBondBattleBackground;


    public List<MonsterUIFirstPickOption> options = new List<MonsterUIFirstPickOption>();

    private int selectedNum = 0;


    public void Init()
    {
        mainObject.SetActive(true);

        for (int i = 0; i < options.Count; i++)
        {
            options[i].Reroll();
        }
    }


    public void SelectMonster(int num)
    {
        selectedNum = num;

        for (int i = 0; i < options.Count; i++)
        {
            if (i == selectedNum - 1)
            {
                options[i].Select();
                options[i].transform.SetParent(focusParent.transform);
            }
            else
            {
                options[i].Hide();
            }
        }

        switch (selectedNum)
        {
            case 1:
                monDesc.text = monDesc01;
                break;
            case 2:
                monDesc.text = monDesc02;
                break;
            case 3:
                monDesc.text = monDesc03;
                break;
        }

        monName.text = options[selectedNum - 1].monster.name;
        title.SetActive(false);
        inspectObject.gameObject.SetActive(true);
        inspectObject.SetBool("Start", true);
    }

    public void Choose()
    {
        inspectObject.gameObject.SetActive(false);
        confirmObject.gameObject.SetActive(true);
        confirmObject.SetBool("Start", true);
    }

    public void FinalStep()
    {
        confirmObject.gameObject.SetActive(false);
        finalStepObject.gameObject.SetActive(true);
        finalStepObject.SetBool("Start", true);
    }

    public void GoBack()
    {
        title.SetActive(true);
        inspectObject.gameObject.SetActive(false);
        confirmObject.gameObject.SetActive(false);
        inspectObject.SetBool("Start", false);
        confirmObject.SetBool("Start", false);

        for (int i = 0; i < options.Count; i++)
        {
            if (i == selectedNum - 1)
            {
                options[i].Deselect();
                options[i].transform.SetParent(normalParent.transform);
            }
            else
            {
                options[i].Show();
            }
        }

    }

    public void Close()
    {
        //GOTO BOND FIGHT WITH THIS MON
        mainObject.SetActive(false);

        Monster chosenMon = options[selectedNum - 1].monster;

        GM.overworldGameobject.SetActive(false);
        GM.overworldUI.gameObject.SetActive(false);

        GM.battleManager.InitFirstBondBattle(chosenMon, startingBondBattleBackground);
    }


    public void Save()
    {
        GM.AddBeastToSeenIDs(options[selectedNum - 1].monster.backupData);
        //GM.playerName = inputfield.text;
        GM.collectionManager.SpawnMonsterInParty(options[selectedNum - 1].monster, 0);
        selectedNum = 0;

        GM.cNode = GM.playerManager.homeNode;
        GM.pNode = GM.playerManager.homeNode;
        GM.SaveData();
    }
}
