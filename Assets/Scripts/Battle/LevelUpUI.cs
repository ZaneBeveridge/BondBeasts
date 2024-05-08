using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public GameObject mainObject;
    public TimelineController timelineController;

    public Image dynamicImg;
    public Image staticImg;
    public Image variantImg;

    public Image beastBacking;
    public Sprite symbioticBacking;
    public Sprite parasiticBacking;

    public GameObject symbioticPreMenu;
    public GameObject confirmButton;

    public List<GameObject> removeStatButtons = new List<GameObject>();
    public List<GameObject> addStatButtons = new List<GameObject>();
    public List<TextMeshProUGUI> statAmounts = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> statAddedAmounts = new List<TextMeshProUGUI>();

    public List<TextMeshProUGUI> detailedStatBases = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> detailedStatChangables = new List<TextMeshProUGUI>();

    public TextMeshProUGUI baseStatPoints;
    public TextMeshProUGUI currentStatPoints;

    public Color normalColour;
    public Color greenColour;


    public GameManager GM;
    public AftermathUI aftermathUI;

    

    private List<int> addedStats = new List<int>();


    private Monster currentMonster;
    private int currentSlot = 0;
    private bool slotIsForParty = false;
    private bool newBeast = false;

    private int currentStatPointsLeft = 0;
    private bool stopLoop = false;

    public void Init(Monster monster) // for new beasts i.e. bond
    {
        slotIsForParty = false;
        newBeast = true;
        InitAll(monster);
    }

    public void Init(Monster monster, int slot, bool beastIsInParty) // for beasts in party/collection, already owned
    {
        newBeast = false;
        currentSlot = slot;
        slotIsForParty = beastIsInParty;
        InitAll(monster);
    }

    private void InitAll(Monster monster)
    {
        mainObject.SetActive(true);

        addedStats = new List<int>();
        addedStats.Add(0);
        addedStats.Add(0);
        addedStats.Add(0);
        addedStats.Add(0);
        addedStats.Add(0);
        addedStats.Add(0);


        currentMonster = monster;
        currentStatPointsLeft = monster.statPoints;

        dynamicImg.sprite = monster.dynamicSprite;
        dynamicImg.color = monster.colour.colour;
        staticImg.sprite = monster.staticSprite;
        variantImg.sprite = monster.variant.variantStillSprite;

        if (monster.symbiotic)
        {
            beastBacking.sprite = symbioticBacking;
        }
        else
        {
            beastBacking.sprite = parasiticBacking;
        }


        if (currentMonster.symbiotic) // initiate auto picker
        {
            symbioticPreMenu.SetActive(true);
            confirmButton.SetActive(false);
        }
        else
        {
            symbioticPreMenu.SetActive(false);
            confirmButton.SetActive(true);
        }


        UpdateMenuStats();

        timelineController.Play();
    }

    public void StartMenu()
    {
        if (Application.isPlaying)
        {
            if (!currentMonster.symbiotic) // initiate auto picker
            {
                confirmButton.SetActive(true);
            }
        }
    }

    public void ContinueToSymbiotic()
    {
        symbioticPreMenu.SetActive(false);
        stopLoop = false;
        StartCoroutine(AutoLevelingLoop());
    }

    IEnumerator AutoLevelingLoop()
    {
        while (stopLoop == false)
        {
            yield return new WaitForSeconds(0.3f);

            LevelRandomStat();

            if (currentStatPointsLeft <= 0)
            {
                stopLoop = true;
                confirmButton.SetActive(true);
            }
        }
    }

    private void UpdateMenuStats()
    {
        baseStatPoints.text = currentMonster.statPoints.ToString();
        currentStatPoints.text = currentStatPointsLeft.ToString();

        float basicDamage = 0;
        float specialDamage = 0;

        if (currentMonster.basicMove.moveType == "Projectile" || currentMonster.basicMove.moveType == "Slow Projectile" || currentMonster.basicMove.moveType == "Very Slow Projectile" || currentMonster.basicMove.moveType == "Fast Projectile" || currentMonster.basicMove.moveType == "Multi Projectile")
        {
            FireProjectileEffectSO effect = currentMonster.basicMove.moveActions[0].effect as FireProjectileEffectSO;
            basicDamage = effect.projectileDamage;
        }

        if (currentMonster.specialMove.moveType == "Projectile" || currentMonster.specialMove.moveType == "Slow Projectile" || currentMonster.specialMove.moveType == "Very Slow Projectile" || currentMonster.specialMove.moveType == "Fast Projectile" || currentMonster.specialMove.moveType == "Multi Projectile")
        {
            FireProjectileEffectSO effect = currentMonster.specialMove.moveActions[0].effect as FireProjectileEffectSO;
            specialDamage = effect.projectileDamage;
        }

        detailedStatBases[0].text = Mathf.RoundToInt(basicDamage + (basicDamage * (currentMonster.stats[0].value * 0.0016f))).ToString();
        detailedStatChangables[0].text = Mathf.RoundToInt(basicDamage + (basicDamage * ((currentMonster.stats[0].value + addedStats[0]) * 0.0016f))).ToString();

        float basicCooldown = Mathf.RoundToInt(currentMonster.basicMove.baseCooldown - (currentMonster.basicMove.baseCooldown * (currentMonster.stats[3].value * 0.008f)));
        if (basicCooldown < currentMonster.basicMove.minCooldown) { basicCooldown = currentMonster.basicMove.minCooldown; }

        float basicCooldownNew = Mathf.RoundToInt(currentMonster.basicMove.baseCooldown - (currentMonster.basicMove.baseCooldown * ((currentMonster.stats[3].value + addedStats[3]) * 0.008f)));
        if (basicCooldownNew < currentMonster.basicMove.minCooldown) { basicCooldownNew = currentMonster.basicMove.minCooldown; }

        detailedStatBases[1].text = basicCooldown.ToString() + "s";
        detailedStatChangables[1].text = basicCooldownNew.ToString() + "s";

        detailedStatBases[2].text = Mathf.RoundToInt(specialDamage + (specialDamage * (currentMonster.stats[0].value * 0.0016f))).ToString();
        detailedStatChangables[2].text = Mathf.RoundToInt(specialDamage + (specialDamage * ((currentMonster.stats[0].value + addedStats[0]) * 0.0016f))).ToString();


        float specialCooldown = Mathf.RoundToInt(currentMonster.specialMove.baseCooldown - (currentMonster.specialMove.baseCooldown * (currentMonster.stats[4].value * 0.008f)));
        if (specialCooldown < currentMonster.specialMove.minCooldown) { specialCooldown = currentMonster.specialMove.minCooldown; }

        float specialCooldownNew = Mathf.RoundToInt(currentMonster.specialMove.baseCooldown - (currentMonster.specialMove.baseCooldown * ((currentMonster.stats[4].value + addedStats[4]) * 0.008f)));
        if (specialCooldownNew < currentMonster.specialMove.minCooldown) { specialCooldownNew = currentMonster.specialMove.minCooldown; }

        detailedStatBases[3].text = specialCooldown.ToString() + "s";
        detailedStatChangables[3].text = specialCooldownNew.ToString() + "s";

        float reductionAmount = currentMonster.stats[1].value * 0.8f;
        if (reductionAmount > 80f) { reductionAmount = 80f; }

        float reductionAmountNew = (currentMonster.stats[1].value + addedStats[1]) * 0.8f;
        if (reductionAmountNew > 80f) { reductionAmountNew = 80f; }

        detailedStatBases[4].text = reductionAmount.ToString() + "%";
        detailedStatChangables[4].text = reductionAmountNew.ToString() + "%";

        detailedStatBases[5].text = (currentMonster.stats[2].value * 0.1f).ToString() + "%";
        detailedStatChangables[5].text = ((currentMonster.stats[2].value + addedStats[2]) * 0.1f).ToString() + "%";


        float tagInTime = 7f - (7f * (currentMonster.stats[5].value * 0.008f));
        if (tagInTime < 0f) { tagInTime = 0f; }

        float tagInTimeNew = 7f - (7f * ((currentMonster.stats[5].value + addedStats[5]) * 0.008f));
        if (tagInTimeNew < 0f) { tagInTimeNew = 0f; }

        detailedStatBases[6].text = tagInTime.ToString() + "s";
        detailedStatChangables[6].text = tagInTimeNew.ToString() + "s";



        for (int i = 0; i < detailedStatChangables.Count; i++)
        {
            if (detailedStatChangables[i].text == detailedStatBases[i].text)
            {
                detailedStatChangables[i].color = normalColour;
            }
            else
            {
                detailedStatChangables[i].color = greenColour;
            }
        }

        for (int i = 0; i < statAmounts.Count; i++)
        {
            statAmounts[i].text = (addedStats[i] + currentMonster.stats[i].value).ToString();
            if (addedStats[i] > 0)
            {
                statAmounts[i].color = greenColour;
                statAddedAmounts[i].text = "+ " + addedStats[i].ToString() + " Points";
                removeStatButtons[i].SetActive(true);
            }
            else
            {
                statAmounts[i].color = normalColour;
                statAddedAmounts[i].text = "";
                removeStatButtons[i].SetActive(false);
            }

            if (currentStatPointsLeft <= 0)
            {
                addStatButtons[i].SetActive(false);
            }
            else
            {
                addStatButtons[i].SetActive(true);
            }
        }


        if (currentMonster.symbiotic)
        {
            for (int i = 0; i < addStatButtons.Count; i++)
            {
                addStatButtons[i].SetActive(false);
                removeStatButtons[i].SetActive(false);
            }
        }
    }


    private void LevelRandomStat()
    {
        int rand = Random.Range(0, 5);

        addedStats[rand]++;
        currentStatPointsLeft--;
        UpdateMenuStats();
    }

    public void Continue()
    {
        currentMonster.statPoints = currentStatPointsLeft;

        for (int i = 0; i < currentMonster.stats.Count; i++)
        {
            currentMonster.stats[i].value += addedStats[i];
        }

        if (newBeast)
        {
            GM.captureChoiceWindow.UpdateFromLevelUp(currentMonster);
        }
        else
        {
            if (slotIsForParty)
            {
                GM.collectionManager.partySlots[currentSlot].storedMonsterObject.GetComponent<PartySlot>().storedMonster = currentMonster;
            }
            else
            {
                for (int i = 0; i < GM.collectionManager.collectionMonsters.Count; i++)
                {
                    if (GM.collectionManager.collectionMonsters[i].storedID == currentSlot)
                    {
                        StoredMonster mon = new StoredMonster(currentMonster, currentSlot);
                        GM.collectionManager.collectionMonsters[i] = mon;
                        break;
                    }
                }
            }

            GM.collectionManager.UpdateCollectionBeasts(GM.collectionManager.currentBag);
        }

        
        mainObject.SetActive(false);
    }

    public void Leave()
    {
        if (newBeast)
        {
            GM.captureChoiceWindow.UpdateFromLevelUp(currentMonster);
            mainObject.SetActive(false);
        }
        else
        {
            mainObject.SetActive(false);
        }
    }

    public void AddStat(int index)
    {
        addedStats[index]++;
        currentStatPointsLeft--;
        UpdateMenuStats();
    }

    public void RemoveStat(int index)
    {
        addedStats[index]--;
        currentStatPointsLeft++;
        UpdateMenuStats();
    }

}

public enum LevelUpType
{
    Party,
    Bond
}
