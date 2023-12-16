using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSprites : MonoBehaviour
{
    public Image dynamicImage;
    public Image staticImage;
    public Image variantImage;



    public void UpdateArt(Monster mon)
    {
        dynamicImage.sprite = mon.dynamicSprite;
        staticImage.sprite = mon.staticSprite;
        variantImage.sprite = mon.variant.variantStillSprite;

        dynamicImage.color = mon.colour.colour;
    }

    public void SetAlpha(float value)
    {
        dynamicImage.color = new Color(dynamicImage.color.r, dynamicImage.color.g, dynamicImage.color.b, value);
        staticImage.color = new Color(staticImage.color.r, staticImage.color.g, staticImage.color.b, value);
        variantImage.color = new Color(variantImage.color.r, variantImage.color.g, variantImage.color.b, value);
    }
}
