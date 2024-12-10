using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyItemSlotMini : MonoBehaviour
{
    public Image backing;
    public Image icon;

    public Sprite backingEquipped;
    public Sprite backingLocked;
    public Sprite backingEmpty;

    public void Init(Sprite i)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = i;
    }

    public void Init(int mode) // 1=locked, 2=empty
    {
        icon.gameObject.SetActive(false);

        if (mode == 1)
        {
            backing.sprite = backingLocked;
        }
        else if (mode == 2)
        {
            backing.sprite = backingEmpty;
        }
    }
}
