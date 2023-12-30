using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvolveUIManager : MonoBehaviour
{
    [Header("Pick References")]
    public GameObject pickScreen;
    public Button ascendButton;
    public Button keepButton;
    public Button descendButton;


    public Image ascendImage;
    public Image keepImage;
    public Image descendImage;

    public TextMeshProUGUI ascendText;
    public TextMeshProUGUI keepText;
    public TextMeshProUGUI descendText;

    public GameObject gainedPrefab;
    public Transform gainedContent;

    public GameObject confirmButton;

    public TextMeshProUGUI pickedText;
    public TextMeshProUGUI subText;

    public GameObject itemReqPrefab;
    public Transform itemReqArea;

    public GameObject cataArea;
    public MonsterUIIcon monsterUIIcon;


    public Color clickTextColor;
    public Color normalTextColor;
    public Color hideTextColor;

    public Color normalImageColor;
    public Color hideImageColor;

    [Header("Transform References")]
    public GameObject evolveScreen;
    public Animator beastAnim;
    public Transform gotTextArea;
    public GameObject gotTextPrefab;

    public TextMeshProUGUI evoTitle;

    public MonsterUIIcon newBeast;
    public MonsterUIIcon currentBeast;


    [Header("Manager References")]
    public GameManager GM;

    
    

    [Header("Art References")]
    public Sprite ascendOn;
    public Sprite ascendOff;
    public Sprite keepOn;
    public Sprite keepOff;
    public Sprite descendOn;
    public Sprite descendOff;



    private List<GameObject> gainedContents = new List<GameObject>();
    private List<GameObject> reqItems = new List<GameObject>();

    private Monster currentMonster;
    private int slotNumber;

    private bool hasItemsToUpgrade = true;

    public Monster testMonster;

    private string trigger = "";

    private List<GameObject> textObjs = new List<GameObject>();

    private Monster aaMon;
    private Monster ddMon;
    public void Start()
    {
        //Init(testMonster, 0);
    }
    public void Init(Monster monster, int slotNum)
    {
        pickScreen.SetActive(true);
        hasItemsToUpgrade = true;
        currentMonster = monster;
        slotNumber = slotNum;
        subText.text = "Select a transformation path for:\n" + monster.name;
        monsterUIIcon.SetVisuals(monster);
        if (monster.backupData.ascendForm.form != null)
        {
            ShowFormButton(ascendButton, ascendImage, ascendText, ascendOn);
        }
        else
        {
            HideFormButton(ascendButton, ascendImage, ascendText, ascendOff);
        }

        if (monster.backupData.descendForm.form != null)
        {
            ShowFormButton(descendButton, descendImage, descendText, descendOn);
        }
        else
        {
            HideFormButton(descendButton, descendImage, descendText, descendOff);
        }
    }





    private void ShowFormButton(Button button, Image image, TextMeshProUGUI text, Sprite sprite)
    {
        button.interactable = true;
        image.sprite = sprite;
        image.color = normalImageColor;
        text.color = normalTextColor;
    }

    private void HideFormButton(Button button, Image image, TextMeshProUGUI text, Sprite sprite)
    {
        button.interactable = false;
        image.sprite = sprite;
        image.color = hideImageColor;
        text.color = hideTextColor;
    }


    public void PressAscend()
    {
        ascendText.color = clickTextColor;

        if (currentMonster.backupData.descendForm.form != null)
        {
            descendText.color = normalTextColor;
        }
        else
        {
            descendText.color = hideTextColor;
        }
        

        
        keepText.color = normalTextColor;

        UpdateGainingContent(1);

        trigger = "Ascend";
    }

    public void PressDescend()
    {
        
        if (currentMonster.backupData.ascendForm.form != null)
        {
            ascendText.color = normalTextColor;
        }
        else
        {
            ascendText.color = hideTextColor;
        }

        descendText.color = clickTextColor;
        keepText.color = normalTextColor;

        UpdateGainingContent(2);

        trigger = "Descend";
    }

    public void PressKeep()
    {
        if (currentMonster.backupData.ascendForm.form != null)
        {
            ascendText.color = normalTextColor;
        }
        else
        {
            ascendText.color = hideTextColor;
        }


        if (currentMonster.backupData.descendForm.form != null)
        {
            descendText.color = normalTextColor;
        }
        else
        {
            descendText.color = hideTextColor;
        }


        keepText.color = clickTextColor;

        UpdateGainingContent(3);

        trigger = "Keep";
    }

    private void UpdateGainingContent(int type)
    {
        if (gainedContents.Count > 0)
        {
            for (int i = 0; i < gainedContents.Count; i++)
            {
                Destroy(gainedContents[i].gameObject);
            }
            gainedContents = new List<GameObject>();
        }

        if (reqItems.Count > 0)
        {
            for (int i = 0; i < reqItems.Count; i++)
            {
                Destroy(reqItems[i].gameObject);
            }
            reqItems = new List<GameObject>();
        }


        if (type == 1)//ASCEND
        {
            hasItemsToUpgrade = true;
            pickedText.text = "ASCENDING";

            if (currentMonster.capLevel <= 1)
            {
                SpawnGainingContent("+1 Item Slot");
            }

            SpawnGainingContent("Ascend to a new form");

            if (currentMonster.backupData.ascendForm.requiredItems.Count > 0)
            {
                for (int i = 0; i < currentMonster.backupData.ascendForm.requiredItems.Count; i++)
                {
                    SpawnItemReqContent(currentMonster.backupData.ascendForm.requiredItems[i]);
                }
            }

            if (currentMonster.backupData.cataForms.Count > 0)
            {
                cataArea.SetActive(true); // do other cata stuff when this is active but for now is juust shows and hides the buutton which you will be able to press to use an item
            }
            else
            {
                cataArea.SetActive(false);
            }
        }
        else if (type == 2)//DESCEND
        {
            hasItemsToUpgrade = true;
            pickedText.text = "DESCENDING";
            if (currentMonster.capLevel <= 1)
            {
                SpawnGainingContent("+1 Item Slot");
            }
            SpawnGainingContent("Descend to the previous form");

            if (currentMonster.backupData.descendForm.requiredItems.Count > 0)
            {
                for (int i = 0; i < currentMonster.backupData.descendForm.requiredItems.Count; i++)
                {
                    SpawnItemReqContent(currentMonster.backupData.descendForm.requiredItems[i]);
                }
            }

            cataArea.SetActive(false);
        }
        else if (type == 3)//KEEP FORM
        {
            hasItemsToUpgrade = true;

            pickedText.text = "KEEPING FORM";
            if (currentMonster.capLevel <= 1)
            {
                SpawnGainingContent("+1 Item Slot");
            }
            SpawnGainingContent("Keep this beasts current form");
            cataArea.SetActive(false);
            
        }



        if (hasItemsToUpgrade)
        {
            confirmButton.SetActive(true);
        }
        else
        {
            confirmButton.SetActive(false);
        }

    }


    private void SpawnItemReqContent(MonsterItemSOAmount item)
    {

        int ownedAmount = 0;
        bool hasAmount = false;

        for (int i = 0; i < GM.itemsOwned.Count; i++)
        {
            if (GM.itemsOwned[i].item.id == item.item.id)
            {
                ownedAmount = GM.itemsOwned[i].amount;
                break;
            }
        }

        if (ownedAmount >= item.amount)
        {
            hasAmount = true;
        }
        else
        {
            hasItemsToUpgrade = false;
            hasAmount = false;
        }



        GameObject mat = Instantiate(itemReqPrefab, itemReqArea);
        mat.GetComponent<ItemUpgradeMatSlot>().Init(item, ownedAmount, hasAmount);

        reqItems.Add(mat);


        
    }
    private void SpawnGainingContent(string text)
    {
        GameObject gain = Instantiate(gainedPrefab, gainedContent);
        gain.GetComponent<GainedContent>().Init(text);
        gainedContents.Add(gain);
    }

    public void ConfirmForm()
    {
        //hide this menu and show new one with anim and showing what you have gained from evolving.

        pickScreen.SetActive(false);
        evolveScreen.SetActive(true);

        TransformInit(trigger);
    }

    private void TransformInit(string trigger)
    {
       

        if (trigger == "Keep")
        {
            currentBeast.SetVisuals(currentMonster);
            evoTitle.text = currentMonster.name + "\nKept Current Form";

            if (currentMonster.capLevel <= 1)
            {
                SpawnInfoBit("+1 Item Slot", 80f);
            }
            
            //
        }
        else if (trigger == "Ascend")
        {
            List<MonsterItemSO> itms = new List<MonsterItemSO>();
            itms.Add(currentMonster.item1);
            itms.Add(currentMonster.item2);
            itms.Add(currentMonster.item3);

            Monster aMon = new Monster(currentMonster.name, currentMonster.level, currentMonster.capLevel + 1, currentMonster.xp, currentMonster.symbiotic, currentMonster.nature, currentMonster.variant, currentMonster.strange, currentMonster.colour, currentMonster.stats, currentMonster.backupData, currentMonster.basicMove, currentMonster.specialMove, currentMonster.passiveMove, itms);
            aMon.backupData = currentMonster.backupData.ascendForm.form;
            aMon.basicMove = currentMonster.backupData.ascendForm.form.basicMove;
            aMon.specialMove = currentMonster.backupData.ascendForm.form.specialMove;
            aMon.passiveMove = currentMonster.backupData.ascendForm.form.passiveMove;

            aMon.variant = currentMonster.backupData.ascendForm.form.possibleVariants[currentMonster.variant.parentId].variant;
            aMon.staticSprite = currentMonster.backupData.ascendForm.form.stillSpriteStatic;
            aMon.dynamicSprite = currentMonster.backupData.ascendForm.form.stillSpriteDynamic;
            aMon.animator = currentMonster.backupData.ascendForm.form.animator;

            aaMon = aMon;


            currentBeast.SetVisuals(currentMonster);
            newBeast.SetVisuals(aMon);
            evoTitle.text = currentMonster.name + "\nAscended";

            SpawnInfoBit("NEW MOVES", 100f);
            SpawnInfoBit("-" + currentMonster.backupData.ascendForm.form.basicMove.moveName, 60f);
            SpawnInfoBit("-" + currentMonster.backupData.ascendForm.form.specialMove.moveName, 60f);
            SpawnInfoBit("-" + currentMonster.backupData.ascendForm.form.passiveMove.moveName, 60f);
  
            if (currentMonster.capLevel <= 1)
            {
                SpawnInfoBit("+1 Item Slot", 80f);
            }

        }
        else if (trigger == "Descend")
        {
            List<MonsterItemSO> itms = new List<MonsterItemSO>();
            itms.Add(currentMonster.item1);
            itms.Add(currentMonster.item2);
            itms.Add(currentMonster.item3);

            Monster dMon = new Monster(currentMonster.name, currentMonster.level, currentMonster.capLevel - 1, currentMonster.xp, currentMonster.symbiotic, currentMonster.nature, currentMonster.variant, currentMonster.strange, currentMonster.colour, currentMonster.stats, currentMonster.backupData, currentMonster.basicMove, currentMonster.specialMove, currentMonster.passiveMove, itms);

            dMon.backupData = currentMonster.backupData.descendForm.form;
            dMon.basicMove = currentMonster.backupData.descendForm.form.basicMove;
            dMon.specialMove = currentMonster.backupData.descendForm.form.specialMove;
            dMon.passiveMove = currentMonster.backupData.descendForm.form.passiveMove;

            dMon.variant = currentMonster.backupData.descendForm.form.possibleVariants[currentMonster.variant.parentId].variant;
            dMon.staticSprite = currentMonster.backupData.descendForm.form.stillSpriteStatic;
            dMon.dynamicSprite = currentMonster.backupData.descendForm.form.stillSpriteDynamic;
            dMon.animator = currentMonster.backupData.descendForm.form.animator;

            ddMon = dMon;

            currentBeast.SetVisuals(currentMonster);
            newBeast.SetVisuals(dMon);
            evoTitle.text = currentMonster.name + "\nDescended";

            SpawnInfoBit("NEW MOVES", 100f);
            SpawnInfoBit("-" + currentMonster.backupData.descendForm.form.basicMove.moveName, 60f);
            SpawnInfoBit("-" + currentMonster.backupData.descendForm.form.specialMove.moveName, 60f);
            SpawnInfoBit("-" + currentMonster.backupData.descendForm.form.passiveMove.moveName, 60f);

            if (currentMonster.capLevel <= 1)
            {
                SpawnInfoBit("+1 Item Slot", 80f);
            }
        }



        beastAnim.SetTrigger(trigger);

        UpdateMons();
    }


    private void SpawnInfoBit(string txt, float fontS)
    {
        GameObject obj = Instantiate(gotTextPrefab, gotTextArea);
        obj.GetComponent<TextMeshProUGUI>().text = txt;
        obj.GetComponent<TextMeshProUGUI>().fontSize = fontS;
        textObjs.Add(obj);
    }


    public void Clear()
    {
        for (int i = 0; i < textObjs.Count; i++)
        {
            Destroy(textObjs[i]);
        }

        textObjs = new List<GameObject>();
    }

    private void UpdateMons()
    {
        if (trigger == "Keep")
        {
            currentMonster.capLevel++;
            GM.collectionManager.partySlots[slotNumber].storedMonsterObject.GetComponent<PartySlot>().storedMonster = currentMonster;
        }
        else if (trigger == "Ascend")
        {
            GM.collectionManager.partySlots[slotNumber].storedMonsterObject.GetComponent<PartySlot>().storedMonster = aaMon;

            if (currentMonster.backupData.ascendForm.requiredItems.Count > 0)
            {
                for (int i = 0; i < currentMonster.backupData.ascendForm.requiredItems.Count; i++)
                {
                    GM.RemoveItemFromStorage(currentMonster.backupData.ascendForm.requiredItems[i].item, currentMonster.backupData.ascendForm.requiredItems[i].amount);
                }
            }
        }
        else if (trigger == "Descend")
        {
            GM.collectionManager.partySlots[slotNumber].storedMonsterObject.GetComponent<PartySlot>().storedMonster = ddMon;

            if (currentMonster.backupData.descendForm.requiredItems.Count > 0)
            {
                for (int i = 0; i < currentMonster.backupData.descendForm.requiredItems.Count; i++)
                {
                    GM.RemoveItemFromStorage(currentMonster.backupData.descendForm.requiredItems[i].item, currentMonster.backupData.descendForm.requiredItems[i].amount);
                }
            }
        }

        GM.SaveData();
    }

    public void ContinueFinal()
    {
        Clear();

        pickScreen.SetActive(false);
        evolveScreen.SetActive(false);

        GM.collectionManager.UpdateCollectionItems();

        
    }
}
