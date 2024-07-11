using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterUIFirstPickOption : MonoBehaviour
{
    [Header("References")]
    public Sprite selectedArt;
    public Sprite unselectedArt;
    public Image background;

    public Image dynamicImage;
    public Image staticImage;
    public Image variantImage;

    public Animator anim;
    public Button button;

    public Monster monster;

    public void Reroll()
    {
        dynamicImage.sprite = monster.dynamicSprite;
        staticImage.sprite = monster.staticSprite;
        variantImage.sprite = monster.variant.variantStillSprite;

        dynamicImage.color = monster.colour.colour;

        anim.SetBool("Hidden", false);
        anim.SetBool("Inspect", false);
    }

    

    public void Hide()
    {
        background.sprite = unselectedArt;
        button.interactable = false;
        anim.SetBool("Hidden", true);
    }
    public void Show()
    {
        background.sprite = unselectedArt;
        button.interactable = true;
        anim.SetBool("Hidden", false);
    }

    public void Deselect()
    {
        background.sprite = unselectedArt;
        button.interactable = true;
        anim.SetBool("Inspect", false);
    }

    public void Select()
    {
        background.sprite = selectedArt;
        button.interactable = false;
        anim.SetBool("Inspect", true);
    }
}
