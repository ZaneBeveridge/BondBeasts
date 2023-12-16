using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSlotsManagerItems : MonoBehaviour
{

    public List<ItemSlotManager> mans = new List<ItemSlotManager>();

    public bool CheckIfAlreadyEquipped(MonsterItemSO item)
    {
        bool state = false;


        for (int i = 0; i < mans.Count; i++)
        {
            if (mans[i].storedItemObject != null)
            {
                if (item.groupedId == mans[i].storedItemObject.GetComponent<ItemEquipSlotMoveable>().item.groupedId)
                {
                    state = true;
                    break;
                }
                
            }
        }

        return state;
    }
}
