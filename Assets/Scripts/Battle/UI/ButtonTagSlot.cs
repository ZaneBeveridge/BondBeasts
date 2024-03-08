using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonTagSlot : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager GM;
    public Image switchButton;

    public TextMeshProUGUI tagNum;

    public GameObject tagGlow;

    public MonsterSprites tagSprites;
    public Animator anim;
    public GameObject selectedObject;


    public int slotNum = 0;
    // Update is called once per frame
    void Update()
    {
        if (GM.collectionManager.partySlots[slotNum - 1].storedMonsterObject != null && GM.battleManager.slotSelected != slotNum) // NOT SELECTED, AND ACTIVE
        {
            //Debug.Log("Slot " + slotNum + " Enabled.");
            switchButton.gameObject.SetActive(true);
            selectedObject.SetActive(false);
            if (!GM.battleManager.friendlyMonsterController.tagReady[slotNum - 1]) // OFF
            {
                switchButton.color = new Color(switchButton.color.r, switchButton.color.g, switchButton.color.b, 0.25f);
                tagSprites.SetAlpha(0.25f);
                tagNum.text = Mathf.RoundToInt(GM.battleManager.friendlyMonsterController.tagC[slotNum - 1]).ToString() + "s";

                tagGlow.SetActive(false);

                anim.SetBool("Ready", false);
            }
            else // ON
            {
                switchButton.color = new Color(switchButton.color.r, switchButton.color.g, switchButton.color.b, 1f);
                tagSprites.SetAlpha(1f);
                tagNum.text = "";

                if (GM.battleManager.friendlyMonsterController.specialReady[slotNum - 1]) // IF SPECIAL READY
                {
                    tagGlow.SetActive(true);
                }
                else
                {
                    tagGlow.SetActive(false);
                }



                anim.SetBool("Ready", true);
            }

        }
        else if (GM.battleManager.slotSelected == slotNum) // IF SELECTED SLOT
        {
            //Debug.Log("Slot " + slotNum + " Selected.");
            switchButton.color = new Color(switchButton.color.r, switchButton.color.g, switchButton.color.b, 0.25f);
            tagSprites.SetAlpha(0.25f);
            tagGlow.SetActive(false);
            selectedObject.SetActive(true);
            switchButton.gameObject.SetActive(true);
            tagNum.text = "";

            anim.SetBool("Ready", false);
        }
        else // IF NO MONSTER IN SLOT
        {
           //Debug.Log("Slot " + slotNum + " Disabled.");
            switchButton.color = new Color(switchButton.color.r, switchButton.color.g, switchButton.color.b, 0.25f);
            tagSprites.SetAlpha(0.25f);
            tagGlow.SetActive(false);
            selectedObject.SetActive(false);
            switchButton.gameObject.SetActive(false);
            tagNum.text = "";
        }
    }
}
