using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIconRotator : MonoBehaviour
{
    public List<IconRotatorItem> items;

    private Color GetColourV(Color colour, float value) // value 0-1 0.5 is 50
    {
        float h, s, v;
        Color returnColour;

        Color.RGBToHSV(colour, out h, out s, out v);

        returnColour = Color.HSVToRGB(h, s, value);

        return returnColour;

    }
}
