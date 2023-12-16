using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEquipMenu : MonoBehaviour
{
    public RectTransform trans;
    public RectTransform trans2;
    public Transform content;
    public GameObject prefab;
    public GameObject obj;
    public GameObject objBack;

    private List<ItemSlotUIEquip> slots = new List<ItemSlotUIEquip>();
    public List<StoredItem> equipItems = new List<StoredItem>();

    private CollectionInspectPanel inspectPanel;
    private GameManager manager;
    public void Refresh(List<StoredItem> equippableItems, string type, int numInStorage, GameManager g)
    {
        manager = g;
        if (slots.Count > 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                Destroy(slots[i].gameObject);
            }

            slots = new List<ItemSlotUIEquip>();
        }

        equipItems = equippableItems;
        for (int i = 0; i < equippableItems.Count; i++)
        {
            GameObject obj = Instantiate(prefab, content);
            ItemSlotUIEquip slt = obj.GetComponent<ItemSlotUIEquip>();
            slt.Init(equippableItems[i].item, equippableItems[i].amount, this, type, numInStorage, g);
            slots.Add(slt);
        }
    }


    public void Show(CollectionInspectPanel panel, bool rightSide)
    {
        obj.SetActive(true);
        objBack.SetActive(true);
        inspectPanel = panel;

        if (rightSide)
        {
            trans.localPosition = new Vector3(710f, 0f, 0f);
            trans2.localPosition = new Vector3(710f, 0f, 0f);
        }
        else
        {
            trans.localPosition = new Vector3(-710f, 0f, 0f);
            trans2.localPosition = new Vector3(-710f, 0f, 0f);
        }
    }

    public void Hide()
    {
        if (manager != null)
        {
            manager.itemInspectManagerPopup.CloseCurrentPanel();
        }
        
        obj.SetActive(false);
        objBack.SetActive(false);
    }
}
