using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MiniIconItem : MonoBehaviour
{
    public TextMeshProUGUI amountText;

    public Image backingImage;
    public Image iconImage;

    public Sprite materialBacking;
    public Sprite equipmentBacking;
    
    public void Init(StoredItem itm)
    {
        amountText.text = itm.amount.ToString();
        iconImage.sprite = itm.item.icon;

        if (itm.item.type == ItemType.Material)
        {
            backingImage.sprite = materialBacking;
        }
        else if (itm.item.type == ItemType.Catalyst)
        {
            backingImage.sprite = equipmentBacking;
        }
    }
}
