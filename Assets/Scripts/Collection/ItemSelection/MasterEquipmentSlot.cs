using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterEquipmentSlot : MonoBehaviour
{
    public Transform transformSpawn;

    public GameObject itemSlot01Prefab;
    public GameObject itemSlot02Prefab;
    public GameObject itemSlot03Prefab;

    private MasterStoredItems masterItems;

    public void Init(MasterStoredItems mItem, GameManager GM)
    {
        masterItems = mItem;
        if (mItem.item01.item.id != 0) // 1
        {
            GameObject itm = Instantiate(itemSlot01Prefab, transformSpawn);
            itm.GetComponent<EquipmentItemSlot>().Init(mItem.item01, GM);
        }

        if (mItem.item02.item.id != 0) // 2
        {
            GameObject itm = Instantiate(itemSlot02Prefab, transformSpawn);
            itm.GetComponent<EquipmentItemSlot>().Init(mItem.item02, GM);
        }

        if (mItem.item03.item.id != 0) // 3
        {
            GameObject itm = Instantiate(itemSlot03Prefab, transformSpawn);
            itm.GetComponent<EquipmentItemSlot>().Init(mItem.item03, GM);
        }
    }
}
