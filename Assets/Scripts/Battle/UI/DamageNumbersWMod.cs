using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DamageNumbersWMod : MonoBehaviour
{
    public Color redGradient;
    public Color whiteGradient;

    public TextMeshPro text1;
    public TextMeshPro text2;
    public void Init(int amount, int mod, Color amountColor, Color modColor)
    {
        text1.text = amount.ToString();

        if (mod > 0)
        {
            text2.text = "+" + mod.ToString(); 
        }
        else if (mod < 0)
        {
            text2.text =  mod.ToString(); ;
        }
        else
        {
            text2.text = "";
        }

        text1.color = amountColor;
        text2.color = modColor;
    }

    public void Init(string txt, Color color)
    {
        text1.text = txt;

        text2.text = "";

        text1.color = color;
    }
}
