using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUIIcon : MonoBehaviour
{
    public Image dynamicImg;
    public Image staticImg;
    public Image variantImage;
    public void SetVisuals(Monster mon)
    {
        dynamicImg.sprite = mon.dynamicSprite;
        staticImg.sprite = mon.staticSprite;
        variantImage.sprite = mon.variant.variantStillSprite;

        dynamicImg.color = mon.colour.colour;
    }
}
