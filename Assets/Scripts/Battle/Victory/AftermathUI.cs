using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AftermathUI : MonoBehaviour
{
    public GameManager GM;
    public TimelineController timelineController;

    public GameObject mainObject;

    public Transform itemSpawnArea;
    public GameObject itemPrefab;

    public Transform leftConfettiLoc;
    public Transform rightConfettiLoc;
    public GameObject leftConfettiPrefab;
    public GameObject rightConfettiPrefab;

    public List<VictoryBeastManager> beastManagers = new List<VictoryBeastManager>();

    private List<GameObject> items = new List<GameObject>();
    private List<StoredMonster> monsList = new List<StoredMonster>();

    private int sliderCount = 0;
    private List<Monster> capMons = new List<Monster>();
    public void Init(List<float> times, int xp, List<StoredMonster> mons, List<Monster> capturedBeasts)
    {
        capMons = capturedBeasts;
        GM.overworldUI.healthBar.SetHealth(GM.playerHP, false);
        mainObject.SetActive(true);

        GM.overworldUI.gameObject.SetActive(false);

        SpawnItems();

        //Debug.Log(xp);
        //Debug.Log(times[0]);
        //Debug.Log(times[1]);
        //Debug.Log(times[2]);

        float xpF = xp;


        List<float> splitXps = new List<float>();
        float combinedTimes = times[0] + times[1] + times[2];
        float div = xpF / combinedTimes;

        //Debug.Log(div);
        splitXps.Add(times[0] * div);
        splitXps.Add(times[1] * div);
        splitXps.Add(times[2] * div);

        //Debug.Log(splitXps[0]);
        //Debug.Log(splitXps[1]);
        //Debug.Log(splitXps[2]);

        for (int i = 0; i < beastManagers.Count; i++)
        {
            beastManagers[i].ResetInit();
        }

        for (int i = 0; i < mons.Count; i++)
        {
            beastManagers[mons[i].storedID].Init(mons[i], Mathf.RoundToInt(splitXps[mons[i].storedID]));
        }

        monsList = mons;

        
        timelineController.Play();
    }

    public void StartLevelSliderAnims()
    {
        sliderCount = 0;
        ContinueSliders();
    }

    public void ContinueSliders()
    {
        sliderCount++;

        if (sliderCount <= monsList.Count) // do xp slider then continue after wards
        {
            beastManagers[sliderCount - 1].DoXpSlider();
        }
        else
        {
            //Finished xp sliders
            timelineController.Resume();
        }
    }

    private void SpawnItems()
    {
        for (int i = 0; i < GM.battleManager.rewardedItems.Count; i++) // spawn items
        {
            GameObject obj = Instantiate(itemPrefab, itemSpawnArea);
            obj.GetComponent<ItemIcon>().Init(GM.battleManager.rewardedItems[i].item.icon, GM.battleManager.rewardedItems[i].item.itemName, GM.battleManager.rewardedItems[i].amount);
            items.Add(obj);

            bool merge = false;
            int mergeId = 0;

            if (GM.collectionManager.itemsOwned.Count > 0)
            {
                for (int j = 0; j < GM.collectionManager.itemsOwned.Count; j++)
                {
                    if (GM.collectionManager.itemsOwned[j].item == GM.battleManager.rewardedItems[i].item)
                    {
                        mergeId = j;
                        merge = true;
                        break;
                    }
                }
            }

            if (merge)
            {
                GM.collectionManager.itemsOwned[mergeId].amount += GM.battleManager.rewardedItems[i].amount;
            }
            else
            {
                GM.collectionManager.AddItemToStorage(GM.battleManager.rewardedItems[i].item, GM.battleManager.rewardedItems[i].amount);
            }
        }
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

        

        GM.overworldUI.healthBar.SetHealth(GM.playerHP, false);

        GM.playerManager.currentRoamerController.RoamerControllerMove();

        GM.overworldUI.gameObject.SetActive(true);

        GM.overworldUI.postBattleLootManager.StartPostBattleLoot(GM.battleManager.rewardedItems, capMons);
    }

    public void SpawnVictoryParticles()
    {
        if (Application.isPlaying)
        {
            GameObject objL = Instantiate(leftConfettiPrefab, leftConfettiLoc);
            GameObject objR = Instantiate(rightConfettiPrefab, rightConfettiLoc);
        }
    }

}
