using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniIconBeast : MonoBehaviour
{
    public Image backingImage;

    public Sprite symbioticSprite;
    public Sprite parasiticSprite;

    public Image dynamicImage;
    public Image staticImage;
    public Image variantImage;

    public void Init(Monster mon)
    {
        dynamicImage.sprite = mon.dynamicSprite;
        dynamicImage.color = mon.colour.colour;

        staticImage.sprite = mon.staticSprite;
        variantImage.sprite = mon.variant.variantStillSprite;

        if (mon.symbiotic)
        {
            backingImage.sprite = symbioticSprite;
        }
        else
        {
            backingImage.sprite = parasiticSprite;
        }
    }
}
