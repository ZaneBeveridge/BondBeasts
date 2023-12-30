using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartySlot : Slot
{
    public Sprite firstBackground;
    public Sprite secondBackground;
    public Sprite thirdBackground;

    public Sprite firstStar;
    public Sprite secondStar;
    public Sprite thirdStar;

    public Image backgroundImage;
    public Image backgroundStar;

    public Slider slider;
    public TextMeshProUGUI sliderText1;

    public GameObject levelupObject;
    public GameObject evolveObject;

    public Image levelUpImage;
    public Image evolveImage;

    public PartySlotManager partySlotManager;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;

    //public int slotNum;
    public void Init(Monster monster, CollectionManager man, PartySlotManager partyMan)
    {
        if (partyMan.slotNum == 1)
        {
            backgroundImage.sprite = firstBackground;
            backgroundStar.sprite = firstStar;
        }
        else if (partyMan.slotNum == 2)
        {
            backgroundImage.sprite = secondBackground;
            backgroundStar.sprite = secondStar;
        }
        else if (partyMan.slotNum == 3)
        {
            backgroundImage.sprite = thirdBackground;
            backgroundStar.sprite = thirdStar;
        }

        //slotNum = num;
        type = SlotType.Party;
        manager = man;
        partySlotManager = partyMan;

        storedMonster = monster;

        dynamicImage.sprite = monster.dynamicSprite;
        dynamicImage.color = monster.colour.colour;

        staticImage.sprite = monster.staticSprite;
        variantImage.sprite = monster.variant.variantStillSprite;

        nameText.text = monster.name;
        levelText.text = "Lvl " + monster.level.ToString();

        if (monster.level >= manager.GM.levelCap) // if monster level bigger or equal to cap show MAX LEVEL
        {
            slider.value = 1f;
            sliderText1.text = "MAX LEVEL";

            levelUpImage.gameObject.SetActive(false);
            levelupObject.SetActive(false);
        }
        else
        {
            float xpReq = 100;
            for (int j = 0; j < monster.level - 1; j++)
            {
                xpReq = xpReq * 1.2f;
            }

            float xpMax = Mathf.RoundToInt(xpReq);

            slider.value = monster.xp / xpMax;
            sliderText1.text = monster.xp.ToString() + "/" + xpMax.ToString() + " XP";


            if (monster.xp >= xpMax) // Show level up button if xp above current level max xp required
            {
                levelupObject.SetActive(true);
                levelUpImage.gameObject.SetActive(true);
            }
            else
            {
                levelUpImage.gameObject.SetActive(false);
                levelupObject.SetActive(false);
            }

        }


        if (((monster.level / 10) - 1) == monster.capLevel && !levelupObject.activeSelf)
        {
            evolveObject.SetActive(true);
            evolveImage.gameObject.SetActive(true);

            slider.value = 1f;
            sliderText1.text = "TRANSFORM READY";

            levelUpImage.gameObject.SetActive(false);
            levelupObject.SetActive(false);
        }
        else
        {
            evolveObject.SetActive(false);
            evolveImage.gameObject.SetActive(false);
        }






        evolveObject.SetActive(false);
    }

    public void UpdateItem()
    {
        if (partySlotManager.slotNum == 1)
        {
            backgroundImage.sprite = firstBackground;
            backgroundStar.sprite = firstStar;
        }
        else if (partySlotManager.slotNum == 2)
        {
            backgroundImage.sprite = secondBackground;
            backgroundStar.sprite = secondStar;
        }
        else if (partySlotManager.slotNum == 3)
        {
            backgroundImage.sprite = thirdBackground;
            backgroundStar.sprite = thirdStar;
        }

        dynamicImage.sprite = storedMonster.dynamicSprite;
        dynamicImage.color = storedMonster.colour.colour;

        staticImage.sprite = storedMonster.staticSprite;
        variantImage.sprite = storedMonster.variant.variantStillSprite;

        nameText.text = storedMonster.name;
        levelText.text = "Lvl " + storedMonster.level.ToString();


        if (storedMonster.level >= manager.GM.levelCap)
        {
            slider.value = 1f;
            sliderText1.text = "MAX LEVEL";

            levelUpImage.gameObject.SetActive(false);
            levelupObject.SetActive(false);
        }
        else
        {
            float xpReq = 100;
            for (int j = 0; j < storedMonster.level - 1; j++)
            {
                xpReq = xpReq * 1.2f;
            }

            float xpMax = Mathf.RoundToInt(xpReq);

            slider.value = storedMonster.xp / xpMax;
            sliderText1.text = storedMonster.xp.ToString() + "/" + xpMax.ToString() + " XP";

            if (storedMonster.xp >= xpMax)
            {
                levelUpImage.gameObject.SetActive(true);
                levelupObject.SetActive(true);
            }
            else
            {
                levelUpImage.gameObject.SetActive(false);
                levelupObject.SetActive(false);
            }
        }


        if (((storedMonster.level / 10) - 1) == storedMonster.capLevel && !levelupObject.activeSelf)
        {
            evolveObject.SetActive(true);
            evolveImage.gameObject.SetActive(true);

            slider.value = 1f;
            sliderText1.text = "TRANSFORM READY";

            levelUpImage.gameObject.SetActive(false);
            levelupObject.SetActive(false);
        }
        else
        {
            evolveObject.SetActive(false);
            evolveImage.gameObject.SetActive(false);
        }





        evolveObject.SetActive(false);
    }

    public override void OnClick()
    {
        if (manager.mode == 1)
        {
            manager.OpenPartyInspect(storedMonster, this.gameObject, "Party", partySlotManager.slotNum - 1);

        }
        else if (manager.mode == 2)
        {
            manager.OpenCollectionInspect(storedMonster, this.gameObject, "Party", partySlotManager.slotNum - 1);
        }
        
    }

    public void ClickLevelup()
    {
        manager.GM.levelUpUI.Init(storedMonster, partySlotManager.slotNum - 1, LevelUpType.Party);
    }

    public void ClickEvolve()
    {
        manager.GM.evolveUI.Init(storedMonster, partySlotManager.slotNum - 1);
    }

}
