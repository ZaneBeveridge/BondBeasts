using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropSlot : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI amountText;


    [HideInInspector] public GameManager manager;
    [HideInInspector] public MonsterItemSO item;

    public void Init(StoredItem i, GameManager g)
    {
        manager = g;
        item = i.item;
        image.sprite = item.icon;
        amountText.text = i.amount.ToString();

        //Debug.Log("Item: " + item.itemName + ". Amount: " + i.amount.ToString());
    }

    public void OnClick()
    {
        ///manager.OpenItemInspectTooltip(item, transform);
    }
}
