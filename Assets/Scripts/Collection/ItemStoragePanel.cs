using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStoragePanel : MonoBehaviour
{
    public CollectionManager manager;
    public GameObject obj;

    private List<ItemSlotUI> items = new List<ItemSlotUI>();

    public void RefreshItems()
    {

        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Destroy(items[i].gameObject);
            }

            items = new List<ItemSlotUI>();
        }


        for (int i = 0; i < manager.GM.itemsOwned.Count; i++)
        {
            if (manager.GM.itemsOwned[i].amount > 0)
            {
                GameObject item = Instantiate(manager.itemSlotPrefab, manager.itemContent);

                ItemSlotUI itUI = item.GetComponent<ItemSlotUI>();
                itUI.Init(manager.GM.itemsOwned[i].item.icon, manager.GM.itemsOwned[i].item.itemName, manager.GM.itemsOwned[i].amount, manager.GM.itemsOwned[i].item, manager.GM);
                items.Add(itUI);
            }
        }
    }

   
}
