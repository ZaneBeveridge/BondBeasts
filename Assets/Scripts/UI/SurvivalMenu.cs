using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SurvivalMenu : MonoBehaviour
{
    public GameManager GM;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI neededText;

    public GameObject clearedObject;
    public GameObject unclearedObject;

    public Animator leaveButtonAnim;
    public Animator startButtonAnim;

    public GameObject background;

    [HideInInspector] public List<MonsterSpawn> monsSpawns = new List<MonsterSpawn>();
    [HideInInspector] public NodeType nodeType;
    [HideInInspector] public Sprite backG;

    public void Init(List<MonsterSpawn> mons, NodeType type, Sprite backgroundG, int id, int scoreN)
    {
        background.SetActive(true);

        monsSpawns = mons;
        nodeType = type;
        backG = backgroundG;

        scoreText.text = GM.survivalBest[id].ToString();

        if (GM.survivalBest[id] >= scoreN)
        {
            clearedObject.SetActive(true);
            unclearedObject.SetActive(false);
        }
        else
        {
            clearedObject.SetActive(false);
            unclearedObject.SetActive(true);
            neededText.text = scoreN.ToString();
        }

        leaveButtonAnim.SetTrigger("Start");
        startButtonAnim.SetTrigger("Start");
        
    }




    public void Leave()
    {
        GM.overworldUI.gameObject.SetActive(true);
        background.SetActive(false);
    }


    public void Enter()
    {
        // FOR BUTTON WHEN CLICK ENTER
        GM.overworldGameobject.SetActive(false);
        GM.battleManager.StartBattle(monsSpawns, nodeType, backG, 0, true);
        background.SetActive(false);
    }
}
