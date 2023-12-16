using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemIcon : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI text;

    public void Init(Sprite spr, string txt, int amount)
    {
        itemImage.sprite = spr;
        text.text = txt + " X" + amount;
    }
}
