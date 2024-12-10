using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartySlot : Slot
{
    public Sprite paraSprite;
    public Sprite symbSprite;

    public Image backgroundImage;

    public Slider slider;
    public TextMeshProUGUI sliderText1;
    public Image sliderBacking;
    public Color blueSliderColour;
    public Color yellowSliderColour;

    public GameObject levelUpObject;
    public GameObject evolveObject;

    [HideInInspector]public PartySlotManager partySlotManager;

    public TextMeshProUGUI nameText;
    public TMP_InputField inputField;
    public TextMeshProUGUI levelText;

    public GameObject renameIconObject;
    public GameObject itemsObj;
    public GameObject equipModeParent;

    public List<TeamEquipSlotManager> equipSlots = new List<TeamEquipSlotManager>();

    public CanvasGroup canvasGroup;
    public Dragable dragable;

    public Animator anim;

    public List<PartyItemSlotMini> miniItemSlots = new List<PartyItemSlotMini>();

    //public int slotNum;
    public void Init(Monster monster, CollectionManager man, PartySlotManager partyMan)
    {
        storedMonster = monster;
        type = SlotType.Party;
        manager = man;
        partySlotManager = partyMan;

        UpdateItem();
    }

    public void UpdateItem()
    {
        if (storedMonster.symbiotic)
        {
            backgroundImage.sprite = symbSprite;
        }
        else
        {
            backgroundImage.sprite = paraSprite;
        }

        dynamicImage.sprite = storedMonster.dynamicSprite;
        dynamicImage.color = storedMonster.colour.colour;
        staticImage.sprite = storedMonster.staticSprite;
        variantImage.sprite = storedMonster.variant.variantStillSprite;
        nameText.text = storedMonster.name;
        levelText.text = "Level " + storedMonster.level.ToString();

        if (storedMonster.level >= manager.GM.levelCap)
        {
            slider.value = 1f;
            sliderText1.text = "MAX LEVEL";

            levelUpObject.SetActive(false);
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

        }

        if (storedMonster.statPoints > 0)
        {
            levelUpObject.SetActive(true);
        }
        else
        {
            levelUpObject.SetActive(false);
        }


        if (((storedMonster.level / 10) - 1) == storedMonster.capLevel && !levelUpObject.activeSelf)
        {
            evolveObject.SetActive(true);
            slider.value = 1f;
            sliderText1.text = "TRANSFORM READY";
            sliderBacking.color = yellowSliderColour;
        }
        else
        {
            sliderBacking.color = blueSliderColour;
            evolveObject.SetActive(false);
        }


        if (storedMonster.item1.id == 0) // locked or empty
        {
            int unlockLevel = 0 * 10;
            if (storedMonster.level >= unlockLevel)
            {
                miniItemSlots[0].Init(2);
            }
            else
            {
                miniItemSlots[0].Init(1);
            }
        }
        else // full
        {
            miniItemSlots[0].Init(storedMonster.item1.icon);
        }

        if (storedMonster.item2.id == 0) // locked or empty
        {
            int unlockLevel = 1 * 10;
            if (storedMonster.level >= unlockLevel)
            {
                miniItemSlots[1].Init(2);
            }
            else
            {
                miniItemSlots[1].Init(1);
            }
        }
        else // full
        {
            miniItemSlots[1].Init(storedMonster.item2.icon);
        }

        if (storedMonster.item3.id == 0) // locked or empty
        {
            int unlockLevel = 2 * 10;
            if (storedMonster.level >= unlockLevel)
            {
                miniItemSlots[2].Init(2);
            }
            else
            {
                miniItemSlots[2].Init(1);
            }
        }
        else // full
        {
            miniItemSlots[2].Init(storedMonster.item3.icon);
        }



    }

    public void EquipMode(bool state)
    {
        if (state)
        {
            nameText.gameObject.SetActive(false);
            levelText.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
            renameIconObject.SetActive(false);
            evolveObject.SetActive(false);
            levelUpObject.SetActive(false);
            itemsObj.SetActive(false);
            equipModeParent.SetActive(true);

            for (int i = 0; i < equipSlots.Count; i++)
            {
                equipSlots[i].UpdateSlot(storedMonster, false, partySlotManager.manager.GM);
            }

            canvasGroup.enabled = false;
            dragable.active = false;
            //canvasGroup.interactable = false;
        }
        else
        {
            nameText.gameObject.SetActive(true);
            levelText.gameObject.SetActive(true);
            slider.gameObject.SetActive(true);
            renameIconObject.SetActive(true);
            itemsObj.SetActive(true);

            equipModeParent.SetActive(false);

            UpdateItem();

            canvasGroup.enabled = true;
            dragable.active = true;
            //canvasGroup.interactable = true;
        }
    }

    public override void OnClick()
    {
        manager.OpenPartyInspect(storedMonster, this.gameObject, "Party", partySlotManager.slotNum - 1);
    }


    public void OnClickRename()
    {
        nameText.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);

        inputField.text = nameText.text;
        inputField.Select();
    }
    
    public void FinishNameEdit()
    {
        nameText.gameObject.SetActive(true);
        inputField.gameObject.SetActive(false);
        

        manager.ChangeMonsterNameInParty(partySlotManager.slotNum - 1, inputField.text);


        nameText.text = inputField.text;

    }

    public void ClickLevelup()
    {
        manager.GM.levelUpUI.Init(storedMonster, partySlotManager.slotNum - 1, true);
    }

    public void ClickEvolve()
    {
        manager.GM.evolveUI.Init(storedMonster, partySlotManager.slotNum - 1);
    }

}
