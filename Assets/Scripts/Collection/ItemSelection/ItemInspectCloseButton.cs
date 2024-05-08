using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInspectCloseButton : MonoBehaviour
{
    private GameManager GM;

    public void Init(GameManager g)
    {
        GM = g;
    }

    public void OnClick()
    {
        GM.CloseItemInspectTooltip();
    }
}
