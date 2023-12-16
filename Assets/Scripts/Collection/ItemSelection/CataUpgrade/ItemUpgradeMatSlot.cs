using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUpgradeMatSlot : MonoBehaviour
{
    public Image background;
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI totalText;
    public TextMeshProUGUI neededText;


    public Color hasMatsText;
    public Color hasMatsBackground;

    public Color noMatsText;
    public Color noMatsBackground;

    public void Init(MonsterItemSOAmount mat, int owned, bool hasItems)
    {
        icon.sprite = mat.item.icon;
        nameText.text = mat.item.itemName;
        totalText.text = owned.ToString();
        neededText.text = mat.amount.ToString();

        if (hasItems)
        {
            background.color = hasMatsBackground;
            totalText.color = hasMatsText;
            neededText.color = hasMatsText;
        }
        else
        {
            background.color = noMatsBackground;
            totalText.color = noMatsText;
            neededText.color = noMatsText;
        }
    }


}
