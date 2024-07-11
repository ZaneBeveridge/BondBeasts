using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaptureChoiceWindow : MonoBehaviour
{
    public GameManager GM;


    [Header("Main")]

    public GameObject mainObject;

    public TextMeshProUGUI nameText;
    public TMP_InputField inputField;
    public TextMeshProUGUI levelText;

    public Image dynamicImg;
    public Image staticImg;
    public Image variantImg;

    public Animator paraAnim;
    public Animator symbAnim;

    public Button statsButton;
    public GameObject statsArea;
    public GameObject movesArea;

    public Sprite personalityUp;
    public Sprite personalityDown;

    public Color normalTextColour;
    public Color redTextColour;
    public Color greenTextColour;

    public List<TextMeshProUGUI> statNumbers = new List<TextMeshProUGUI>();
    public List<Image> statPersonalityIcon = new List<Image>();
    public List<TextMeshProUGUI> statMultipliers = new List<TextMeshProUGUI>();

    public Sprite typeProjectileSprite;
    public Sprite typeInstantSprite;
    public Sprite typeGuardSprite;

    public TextMeshProUGUI personalityText;
    public GameObject strangeObject;

    [Header("Basic")]
    public TextMeshProUGUI basicEffectText;
    public TextMeshProUGUI basicTypeText;
    public TextMeshProUGUI basicCooldownText;
    public Image basicTypeImage;

    [Header("Special")]
    public TextMeshProUGUI specialEffectText;
    public TextMeshProUGUI specialTypeText;
    public TextMeshProUGUI specialCooldownText;
    public Image specialTypeImage;
    [Header("Passive")]
    public TextMeshProUGUI passiveEffectText;


    public Monster currentMonster;
    public EnemyMonsterController enemyMonsterController;
    private SurvivalSubMenu subMenu;
    private List<Monster> monsters = new List<Monster>();

    private bool survivalMode = false;

    public void Init(List<Monster> mons, EnemyMonsterController controller)
    {
        survivalMode = false;
        mainObject.SetActive(true);
        monsters = mons;
        enemyMonsterController = controller;

        NextMonster();
    }

    public void Init(List<Monster> mons, EnemyMonsterController controller, SurvivalSubMenu menu)
    {
        subMenu = menu;
        survivalMode = true;
        mainObject.SetActive(true);
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

        nameText.text = currentMonster.backupData.defaultName;
        inputField.text = currentMonster.backupData.defaultName;

        levelText.text = "Lvl " + currentMonster.level.ToString();

        for (int i = 0; i < statNumbers.Count; i++)
        {
            float personalityMod = currentMonster.nature.addedStats[i].value;

            statNumbers[i].text = (currentMonster.stats[i].value * personalityMod).ToString();

            if (personalityMod > 1)
            {
                statNumbers[i].color = greenTextColour;
                statPersonalityIcon[i].gameObject.SetActive(true);
                statPersonalityIcon[i].sprite = personalityUp;
                statMultipliers[i].text = "x" + personalityMod.ToString();
                statMultipliers[i].color = greenTextColour;
            }
            else if (personalityMod < 1)
            {
                statNumbers[i].color = redTextColour;
                statPersonalityIcon[i].gameObject.SetActive(true);
                statPersonalityIcon[i].sprite = personalityDown;
                statMultipliers[i].text = "x" + personalityMod.ToString();
                statMultipliers[i].color = redTextColour;
            }
            else
            {
                statNumbers[i].color = normalTextColour;
                statPersonalityIcon[i].gameObject.SetActive(false);
                statMultipliers[i].text = "";
            }
        }


        personalityText.text = "Personality: " + currentMonster.nature.natureName;

        if (currentMonster.strange)
        {
            strangeObject.SetActive(true);
        }
        else
        {
            strangeObject.SetActive(false);
        }

        //BASIC

        basicEffectText.text = currentMonster.basicMove.moveDescription;

        basicTypeText.text = currentMonster.basicMove.moveType;

        if (currentMonster.basicMove.moveType == "Guard")
        {
            basicTypeImage.sprite = typeGuardSprite;
        }
        else if (currentMonster.basicMove.moveType == "Instant")
        {
            basicTypeImage.sprite = typeInstantSprite;
        }
        else
        {
            basicTypeImage.sprite = typeProjectileSprite;
        }


        basicCooldownText.text = currentMonster.basicMove.baseCooldown.ToString("F1");

        //SPECIAL
        specialEffectText.text = currentMonster.specialMove.moveDescription;

        specialTypeText.text = currentMonster.specialMove.moveType;

        if (currentMonster.specialMove.moveType == "Guard")
        {
            specialTypeImage.sprite = typeGuardSprite;
        }
        else if (currentMonster.specialMove.moveType == "Instant")
        {
            specialTypeImage.sprite = typeInstantSprite;
        }
        else
        {
            specialTypeImage.sprite = typeProjectileSprite;
        }

        specialCooldownText.text = currentMonster.specialMove.baseCooldown.ToString("F1");


        //PASSIVE

        passiveEffectText.text = currentMonster.passiveMove.moveDescription;

        statsButton.Select();
        statsArea.SetActive(true);
        movesArea.SetActive(false);

    }

    public void PressType(string type)
    {
        if (type == "Symbiotic")
        {
            symbAnim.SetBool("Open", true);
            paraAnim.SetBool("Open", false);
        }
        else if (type == "Parasitic")
        {
            symbAnim.SetBool("Open", false);
            paraAnim.SetBool("Open", true);
        }
    }

    public void ConfirmType(string type)
    {
        int amountOfUsableLevels = 0;

        for (int i = 0; i < currentMonster.level; i++)
        {
            amountOfUsableLevels++;
        }

        if (amountOfUsableLevels > 30)
        {
            amountOfUsableLevels = 30;
        }


        if (type == "Symbiotic")
        {
            currentMonster.symbiotic = true;
            currentMonster.statPoints = amountOfUsableLevels * 6;
        }
        else if (type == "Parasitic")
        {
            currentMonster.symbiotic = false;
            currentMonster.statPoints = amountOfUsableLevels * 4;
        }

        GM.levelUpUI.Init(currentMonster);
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


        currentMonster.name = inputField.text;


        nameText.text = inputField.text;

    }

    public void UpdateFromLevelUp(Monster updateMonster)
    {
        currentMonster = updateMonster;
        Fin();
    }

    public void Fin()
    {
        currentMonster.name = inputField.text;
        ///enemyMonsterController.ActivateAI(true);


        if (GM.collectionManager.CheckFreePartySlot() >= 3)
        {
            GM.collectionManager.SpawnMonsterInCollection(currentMonster);
        }
        else
        {
            GM.collectionManager.SpawnMonsterInParty(currentMonster, GM.collectionManager.CheckFreePartySlot());
        }

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
                mainObject.SetActive(false);
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

                GM.overworldUI.healthBar.SetHealth(GM.playerHP, false);

                int num = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
                    {
                        num++;
                    }
                }

                GM.SaveData();

                mainObject.SetActive(false);
            }

            
        }

        
    }


}
