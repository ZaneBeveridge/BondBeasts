using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AftermathUI : MonoBehaviour
{
    public GameManager GM;
    public TimelineController timelineController;

    public Sprite wonSprite;
    public Sprite lostSprite;

    public Image vicImage;

    public GameObject mainObject;

    public Transform itemSpawnArea;
    public GameObject itemPrefab;
    
    public LevelUpUI levelUpUI;


    public List<MonsterUIIcon> monsters = new List<MonsterUIIcon>();
    public List<TextMeshProUGUI> names = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> names2 = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> xps = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> xps2 = new List<TextMeshProUGUI>();
    public List<GameObject> buttons = new List<GameObject>();

    public List<Slider> sliders = new List<Slider>();

    private List<GameObject> items = new List<GameObject>();
    public void Init(string text, List<float> times, int xp, int num)
    {
        GM.overworldUI.healthBar.SetHealth(GM.playerHP);

        if (text == "WON")
        {
            vicImage.sprite = wonSprite;

            for (int i = 0; i < GM.battleManager.rewardedItems.Count; i++)
            {
                GameObject obj = Instantiate(itemPrefab, itemSpawnArea);
                obj.GetComponent<ItemIcon>().Init(GM.battleManager.rewardedItems[i].item.icon, GM.battleManager.rewardedItems[i].item.itemName, GM.battleManager.rewardedItems[i].amount);
                items.Add(obj);

                bool merge = false;
                int mergeId = 0;

                if (GM.itemsOwned.Count > 0)
                {
                    for (int j = 0; j < GM.itemsOwned.Count; j++)
                    {
                        if (GM.itemsOwned[j].item == GM.battleManager.rewardedItems[i].item)
                        {
                            mergeId = j;
                            merge = true;
                            break;
                        }
                    }
                }

                if (merge)
                {
                    GM.itemsOwned[mergeId].amount += GM.battleManager.rewardedItems[i].amount;
                }
                else
                {
                    GM.itemsOwned.Add(new StoredItem(GM.battleManager.rewardedItems[i].item, GM.battleManager.rewardedItems[i].amount));
                }
            }
        }
        else if (text == "LOST")
        {
            vicImage.sprite = lostSprite;
        }
        else if (text == "CAPTURE")
        {
            vicImage.sprite = wonSprite;
        }

        mainObject.SetActive(true);

        List<float> splitXps = new List<float>();

        for (int i = 0; i < monsters.Count; i++)
        {
            if (i < num)
            {
                //Debug.Log("Num: " + num);
                //Debug.Log("Index: " + i);
                monsters[i].gameObject.SetActive(true);
                monsters[i].SetVisuals(GM.collectionManager.partySlots[i].storedMonsterObject.GetComponent<PartySlot>().storedMonster);

                Monster mon = GM.collectionManager.partySlots[i].storedMonsterObject.GetComponent<PartySlot>().storedMonster;
                names[i].text = mon.name;
                names2[i].text = mon.name;
                float combinedTimes = times[0] + times[1] + times[2];

                float div = xp / combinedTimes;
                

                splitXps.Add(times[i] * div);

                if (mon.level >= GM.levelCap && mon.level < ((mon.capLevel + 1) * 10))
                {
                    xps[i].text = "MAX LEVEL";
                    xps2[i].text = "MAX LEVEL";

                    sliders[i].value = 1f;
                }
                else if (mon.level >= ((mon.capLevel + 1) * 10))
                {
                    xps[i].text = "TRANSFORM READY";
                    xps2[i].text = "TRANSFORM READY";

                    sliders[i].value = 1f;
                    buttons[i].gameObject.SetActive(false);
                }
                else
                {
                    float xpReq = 100;
                    for (int j = 0; j < mon.level - 1; j++)
                    {
                        xpReq = xpReq * 1.2f;
                    }

                    float xpMax = Mathf.RoundToInt(xpReq);


                    if (mon.level == GM.levelCap - 1 && mon.xp + splitXps[i] >= xpMax)
                    {
                        xps[i].text = "MAX LEVEL";
                        xps2[i].text = "MAX LEVEL";

                        sliders[i].value = 1f;

                        mon.xp = Mathf.RoundToInt(xpMax);

                        if (mon.xp >= Mathf.RoundToInt(xpReq) && mon.level < GM.levelCap)
                        {
                            //levelUP!
                            buttons[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            buttons[i].gameObject.SetActive(false);
                        }

                        
                    }
                    else
                    {
                        if (Mathf.RoundToInt(splitXps[i]) > 0)
                        {
                            xps[i].text = "+ " + Mathf.RoundToInt(splitXps[i]) + " XP";
                            xps2[i].text = "+ " + Mathf.RoundToInt(splitXps[i]) + " XP";
                        }
                        else
                        {
                            xps[i].text = "0 XP";
                            xps2[i].text = "0 XP";
                        }

                        if (splitXps[i] > 0)
                        {
                            // take cap amount and if total xp needed to reach this level is equal to mon.xp + splitXps[i], then set xp to max xp needed to reach current cap

                            int xpToReachCap = 0;

                            float xpReq2 = 100;
                            float addAmount = 100f;
                            for (int j = 0; j < GM.levelCap - 2; j++)
                            {
                                xpReq2 = xpReq2 * 1.2f;
                                addAmount += xpReq2;
                            }

                            xpToReachCap = Mathf.RoundToInt(addAmount);
                            //Debug.Log(xpToReachCap);

                            int lowerLvlXP = 0;

                            float xpReq3 = 100;
                            float addAmount2 = 0f;

                            if (mon.level > 1) // if lvl 1 0, lvl 2 100, lvl 3 120 lvl 4 144 ITS FUCKED LAND AROUND HERE BEWARE
                            {
                                addAmount2 = 100f;

                                for (int j = 0; j < mon.level - 2; j++)
                                {
                                    xpReq3 = xpReq3 * 1.2f;
                                    addAmount2 += xpReq3;
                                }

                            }

                            

                            lowerLvlXP = Mathf.RoundToInt(addAmount2);
                            //Debug.Log(splitXps[i]);

                            //Debug.Log(lowerLvlXP);

                            //Debug.Log(mon.xp + splitXps[i] + lowerLvlXP);

                            if (mon.xp + splitXps[i] + lowerLvlXP >= xpToReachCap)
                            {
                                mon.xp = xpToReachCap;
                            }
                            else
                            {
                                mon.xp += Mathf.RoundToInt(splitXps[i]);
                            }

                            
                        }


                        sliders[i].value = mon.xp / xpMax;

                        if (mon.xp >= Mathf.RoundToInt(xpReq) && mon.level < 100)
                        {
                            //levelUP!
                            buttons[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            buttons[i].gameObject.SetActive(false);
                        }
                    }
                }


            }
            else if (i >= num)
            {
                Debug.Log("Index: " + i);
                monsters[i].gameObject.SetActive(false);
            }
        }

        timelineController.Play();
    }

    public void Done()
    {
        mainObject.SetActive(false);
        if (GM.playerHP <= 0)
        {
            GM.playerHP = 50f;
            
            GM.popupManager.FullyHealed();
            GM.MovePlayerHome();
        }

        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i]);
        }

        items = new List<GameObject>();

        

        GM.overworldUI.healthBar.SetHealth(GM.playerHP);

        //Debug.Log("Hello1");
        GM.playerManager.currentRoamerController.RoamerControllerMove();
    }

    public void LevelUp(int slot)
    {
        levelUpUI.Init(GM.collectionManager.partySlots[slot].storedMonsterObject.GetComponent<PartySlot>().storedMonster, slot, LevelUpType.Afterbattle);
    }

    public void DoneLeveling(int slot)
    {
        Monster mon = GM.collectionManager.partySlots[slot].storedMonsterObject.GetComponent<PartySlot>().storedMonster;

        float xpReq = 100;
        for (int j = 0; j < mon.level - 1; j++)
        {
            xpReq = xpReq * 1.2f;
        }

        float xpMax = Mathf.RoundToInt(xpReq);

        if (mon.xp >= Mathf.RoundToInt(xpReq) && mon.level < GM.levelCap)
        {
            //levelUP!
            buttons[slot].gameObject.SetActive(true);
        }
        else
        {
            buttons[slot].gameObject.SetActive(false);
        }

        if (mon.level < GM.levelCap)
        {
            sliders[slot].value = mon.xp / xpMax;
        }
        else
        {
            xps[slot].text = "MAX LEVEL";
            xps2[slot].text = "MAX LEVEL";


            sliders[slot].value = 1f;
        }

        
    }

}
