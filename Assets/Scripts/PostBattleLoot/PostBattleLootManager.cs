using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostBattleLootManager : MonoBehaviour
{
    public GameObject miniBeastPopupPrefab;
    public GameObject miniItemPopupPrefab;

    public Transform spawnLoc;

    private bool stopLoop = false;

    private List<StoredItem> itms = new List<StoredItem>();
    private List<Monster> mons = new List<Monster>();

    private List<GameObject> objs = new List<GameObject>();
    public void StartPostBattleLoot(List<StoredItem> items, List<Monster> capturedBeasts)
    {
        stopLoop = false;
        objs = new List<GameObject>();
        itms = items;
        mons = capturedBeasts;
        StartCoroutine(LoopLoot());
    }

    public void StopLoop()
    {
        StopCoroutine(LoopLoot());
        for (int i = 0; i < objs.Count; i++)
        {
            Destroy(objs[i]);
        }
    }


    IEnumerator LoopLoot()
    {
        while (stopLoop == false)
        {
            yield return new WaitForSeconds(0.2f);

            if (itms.Count > 0) // do items
            {
                GameObject obj = Instantiate(miniItemPopupPrefab, spawnLoc);
                obj.GetComponent<MiniIconItem>().Init(itms[0]);
                objs.Add(obj);
                itms.RemoveAt(0);
            }
            else
            {
                if (mons.Count > 0) // do mons
                {
                    GameObject obj = Instantiate(miniBeastPopupPrefab, spawnLoc);
                    obj.GetComponent<MiniIconBeast>().Init(mons[0]);
                    objs.Add(obj);
                    mons.RemoveAt(0);
                }
            }

            if (itms.Count <= 0 && mons.Count <= 0)
            {
                stopLoop = true;
            }
        }

        

    }
}

