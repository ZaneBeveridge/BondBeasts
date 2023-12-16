using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateProfile : MonoBehaviour
{
    public GameManager GM;
    
    public GameObject first;
    public TMP_InputField inputfield;
    public GameObject buttonObjectFirst;
    public Image background;

    public GameObject second;
    public GameObject buttonObjectSecond;
    public List<MonsterUIFirstPickOption> options = new List<MonsterUIFirstPickOption>();

    private int selectedNum = 0;
    private bool active = false;

    public void Update()
    {
        if (active)
        {
            if (inputfield.text == "")
            {
                buttonObjectFirst.SetActive(false);
            }
            else
            {
                buttonObjectFirst.SetActive(true);
            }

            if (selectedNum == 0)
            {
                buttonObjectSecond.SetActive(false);
            }
            else
            {
                buttonObjectSecond.SetActive(true);
            }
        }

        
        
    }
    public void Open()
    {
        for (int i = 0; i < options.Count; i++)
        {
            options[i].Reroll();
        }

        background.enabled = true;
        first.SetActive(true);
        second.SetActive(false);

        active = true;
    }

    public void NextPage()
    {
        first.SetActive(false);
        second.SetActive(true);
    }

    public void SelectMonster(int num)
    {
        selectedNum = num;

        for (int i = 0; i < options.Count; i++)
        {
            options[i].Deselect();
        }

        options[num - 1].Select();
    }

    public void Close()
    {
        if (selectedNum != 0)
        {
            background.enabled = false;
            first.SetActive(false);
            second.SetActive(false);

            active = false;

            Save();
        }
        
    }


    public void Save()
    {
        
        GM.playerName = inputfield.text;
        GM.collectionManager.SpawnMonsterInParty(options[selectedNum - 1].monster, 1);
        selectedNum = 0;

        GM.cNode = GM.playerManager.homeNode;
        GM.pNode = GM.playerManager.homeNode;
        GM.SaveData();
    }
}
