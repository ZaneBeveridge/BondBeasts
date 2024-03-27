using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MenuTabButtons : MonoBehaviour
{
    public Image backgroundImage;
    public Button ownButton;

    public Sprite selectedSprite;
    public Sprite unselectedSprite;

    public Animator anim;
    public void Select(bool instant)
    {
        backgroundImage.sprite = selectedSprite;
        ownButton.enabled = false;

        if (instant)
        {
            anim.SetTrigger("SelectTriggerInstant");
        }
        else
        {
            anim.SetTrigger("SelectTrigger");
        }
        
    }

    public void Hide(bool instant)
    {
        backgroundImage.sprite = unselectedSprite;
        ownButton.enabled = true;

        if (instant)
        {
            anim.SetTrigger("UnselectTriggerInstant");
        }
        else
        {
            anim.SetTrigger("UnselectTrigger");
        }
    }

}
