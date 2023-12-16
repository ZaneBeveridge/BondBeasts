using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IconRotatorItem : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager GM;
    public Image switchButton;

    public TextMeshProUGUI tagNum;

    public MonsterSprites tagSprites;

    public GameObject selectedObject;

    public int slotNum = 0;
    // Update is called once per frame
    void Update()
    {
        if (GM.battleManager.enemyMonsterController.backupMonsters.Count >= slotNum && GM.battleManager.enemySlotSelected != slotNum)
        {
            //Debug.Log("Slot " + slotNum + " Enabled.");
            switchButton.gameObject.SetActive(true);
            selectedObject.SetActive(false);
            if (!GM.battleManager.enemyMonsterController.tagReady[slotNum - 1])
            {
                switchButton.color = new Color(switchButton.color.r, switchButton.color.g, switchButton.color.b, 0.25f);
                tagSprites.SetAlpha(0.25f);
                tagNum.text = Mathf.RoundToInt(GM.battleManager.enemyMonsterController.tagC[slotNum - 1]).ToString() + "s";

                //tagGlow.SetActive(false);
            }
            else
            {
                switchButton.color = new Color(switchButton.color.r, switchButton.color.g, switchButton.color.b, 1f);
                tagSprites.SetAlpha(1f);
                tagNum.text = "";
            }

        }
        else if (GM.battleManager.enemySlotSelected == slotNum)
        {
            //Debug.Log("Slot " + slotNum + " Selected.");
            switchButton.color = new Color(switchButton.color.r, switchButton.color.g, switchButton.color.b, 0.25f);
            tagSprites.SetAlpha(0.25f);
            //tagGlow.SetActive(false);
            selectedObject.SetActive(true);
            switchButton.gameObject.SetActive(true);
            tagNum.text = "";
        }
        else
        {
            //Debug.Log("Slot " + slotNum + " Disabled.");
            switchButton.color = new Color(switchButton.color.r, switchButton.color.g, switchButton.color.b, 0.25f);
            tagSprites.SetAlpha(0.25f);
            //tagGlow.SetActive(false);
            selectedObject.SetActive(false);
            switchButton.gameObject.SetActive(false);
            tagNum.text = "";
        }
    }

}
