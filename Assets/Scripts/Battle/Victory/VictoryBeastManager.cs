using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryBeastManager : MonoBehaviour
{
    public GameObject mainObject;

    public Image backingImage;

    public Sprite paraSprite;
    public Sprite symbSprite;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI xpText;

    public Slider staticSlider;
    public Slider movingSlider;

    public Image dynamicImg;
    public Image staticImg;
    public Image variantImage;

    public GameObject levelUpParticlePrefab;
    public Transform levelUpParticleLoc;

    public AftermathUI aftermathUI;

    private int xpToGain = 0;

    private bool stopSlider = false;
    private float sliderAmount = 0f;
    private float xpToGainAmount = 0f;

    private int currentMaxXp;

    private float sliderTimeMultiplier = 40f;

    private StoredMonster mon;
    public void Init(StoredMonster m, int xpTG)
    {
        mainObject.SetActive(true);

        dynamicImg.sprite = m.monster.dynamicSprite;
        staticImg.sprite = m.monster.staticSprite;
        variantImage.sprite = m.monster.variant.variantStillSprite;

        dynamicImg.color = m.monster.colour.colour;

        nameText.text = m.monster.name;
        xpToGain = xpTG;
        xpToGainAmount = xpTG;
        xpText.text = "+ " + xpTG.ToString() + " XP";

        levelText.text = "Level " + m.monster.level;
        statsText.text = "";

        float xpReq = 100;
        for (int j = 0; j < m.monster.level - 1; j++)
        {
            xpReq = xpReq * 1.2f;
        }

        int xpMax = Mathf.RoundToInt(xpReq);

        currentMaxXp = xpMax;

        staticSlider.maxValue = xpMax;
        movingSlider.maxValue = xpMax;

        staticSlider.value = m.monster.xp;
        movingSlider.value = m.monster.xp;


        if (m.monster.level >= aftermathUI.GM.levelCap)
        {
            staticSlider.value = xpMax;
            movingSlider.value = xpMax;
            xpText.text = "MAX LEVEL";
        }

        if (xpToGain <= 0)
        {
            xpText.text = m.monster.xp + " / " + xpMax + " XP";
        }

        sliderAmount = m.monster.xp;

        if (m.monster.symbiotic)
        {
            backingImage.sprite = symbSprite;
        }
        else
        {
            backingImage.sprite = paraSprite;
        }

        mon = m;
    }

    public void DoXpSlider()
    {
        if (!mainObject.activeSelf) { aftermathUI.ContinueSliders(); return; }
        stopSlider = false;
        StartCoroutine(StartXPCharger());
    }

    public void FinishSlider()
    {
        
        int levelsToGain = 0;
        int xp = mon.monster.xp + xpToGain;

        

        while (xp >= currentMaxXp)
        {
            if (mon.monster.level + levelsToGain >= aftermathUI.GM.levelCap)
            {
                xp = 0;
                break;
            }

            levelsToGain++;
            xp -= currentMaxXp;

            float xpReq = 100;
            for (int j = 0; j < mon.monster.level - 1 + levelsToGain; j++)
            {
                xpReq = xpReq * 1.2f;
            }

            int xpMax = Mathf.RoundToInt(xpReq);

            currentMaxXp = xpMax;
        }

        staticSlider.maxValue = currentMaxXp;
        staticSlider.value = xp;

        if (mon.monster.level >= aftermathUI.GM.levelCap)
        {
            xp = mon.monster.xp;
            staticSlider.maxValue = currentMaxXp;
            staticSlider.value = currentMaxXp;
        }


        mon.monster.xp = xp;
        mon.monster.level = mon.monster.level + levelsToGain;

        if (mon.monster.symbiotic)
        {
            mon.monster.statPoints += 6 * levelsToGain;
        }
        else
        {
            mon.monster.statPoints += 4 * levelsToGain;
        }

        if (aftermathUI.GM.collectionManager.partySlots[mon.storedID].storedMonsterObject != null)
        {
            aftermathUI.GM.collectionManager.partySlots[mon.storedID].storedMonsterObject.GetComponent<PartySlot>().storedMonster = mon.monster;
        }
        else
        {
            Debug.LogError("Error trying to update party slot level + xp with new amounts after battle, slot " + mon.storedID.ToString() + " is empty and stored object is null");
        }

        



        aftermathUI.ContinueSliders();
    }

    IEnumerator StartXPCharger()
    {
        int levelUpCount = 0;

        int tempMaxXP = currentMaxXp;
        int lastTempMaxXP = 0;

        while (stopSlider == false)
        {
            sliderAmount += Time.deltaTime * sliderTimeMultiplier;
            xpToGainAmount -= Time.deltaTime * sliderTimeMultiplier;
            yield return new WaitForSeconds(0.001f);


            if (mon.monster.level >= aftermathUI.GM.levelCap)
            {
                stopSlider = true;
                xpText.text = "MAX LEVEL";
                FinishSlider();
            }

            if (sliderAmount >= tempMaxXP)
            {
                if (mon.monster.level + levelUpCount + 1 >= aftermathUI.GM.levelCap)
                {
                    stopSlider = true;
                    xpText.text = "MAX LEVEL";
                    LevelUp();
                    FinishSlider();
                }
                else if (xpToGainAmount > 0) // new bar / continue
                {
                    levelUpCount++;
                    sliderAmount = 0f;
                    staticSlider.value = 0f;

                    float xpReq = 100;
                    for (int j = 0; j < mon.monster.level - 1 + levelUpCount; j++)
                    {
                        xpReq = xpReq * 1.2f;
                    }

                    int xpMax = Mathf.RoundToInt(xpReq);
                    lastTempMaxXP += tempMaxXP;
                    tempMaxXP = xpMax;

                    staticSlider.maxValue = xpMax;
                    movingSlider.maxValue = xpMax;

                    levelText.text = "Level " + (mon.monster.level + levelUpCount);

                    int statsPerLevel = 0;
                    if (mon.monster.symbiotic)
                    {
                        statsPerLevel = 6;
                    }
                    else
                    {
                        statsPerLevel = 4;
                    }

                    statsText.text = "+ " + (levelUpCount * statsPerLevel) + "\nStat Points\nToSpend";

                    LevelUp();
                }
                else // done
                {
                    stopSlider = true;
                    xpText.text = ((xpToGain + mon.monster.xp) - lastTempMaxXP) + " / " + tempMaxXP + " XP";
                    FinishSlider();
                }
            }
            else if (xpToGainAmount <= 0)
            {
                stopSlider = true;
                xpText.text = ((xpToGain + mon.monster.xp) - lastTempMaxXP) + " / " + tempMaxXP + " XP";
                FinishSlider();
            }

            if (stopSlider == false)
            {
                movingSlider.value = sliderAmount;
                xpText.text = "+ " + Mathf.RoundToInt(xpToGainAmount).ToString() + " XP";
            }
        }
        
    }

    private void LevelUp()
    {
        GameObject obj = Instantiate(levelUpParticlePrefab, levelUpParticleLoc);
    }

    public void ResetInit()
    {
        mainObject.SetActive(false);
    }
}
