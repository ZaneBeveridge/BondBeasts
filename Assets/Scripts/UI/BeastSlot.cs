using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BeastSlot : MonoBehaviour
{
    public Image staticImage;
    public Image dynamicImage;
    public Image variantImage;
    public TextMeshProUGUI levelText;
    //

    [HideInInspector] public GameManager manager;
    [HideInInspector] public Monster monster;

    public void Init(Monster mon, GameManager g)
    {
        manager = g;
        monster = mon;

        staticImage.sprite = monster.staticSprite;
        dynamicImage.sprite = monster.dynamicSprite;
        variantImage.sprite = monster.variant.variantStillSprite;

        dynamicImage.color = monster.colour.colour;

        levelText.text = "LVL " + monster.level.ToString();

    }

}
