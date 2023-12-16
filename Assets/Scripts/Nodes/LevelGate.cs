using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGate : MonoBehaviour
{
    public int levelReq;
    public GameObject textObject;

    public Sprite open;
    public Sprite closed;
    // Update is called once per frame
    public SpriteRenderer r;
    public GameManager GM;

    void Update()
    {
        int value = 0;

        for (int i = 0; i < GM.collectionManager.collectionMonsters.Count; i++)
        {
            value++;
        }

        for (int i = 0; i < GM.collectionManager.partySlots.Count; i++)
        {
            if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                value++;
            }
        }

        if (value >= levelReq)
        {
            r.sprite = open;
            textObject.SetActive(false);
        }
        else
        {
            r.sprite = closed;
            textObject.SetActive(true);
        }
    }
}
