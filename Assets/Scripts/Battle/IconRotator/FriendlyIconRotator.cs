using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyIconRotator : MonoBehaviour
{
    public IconRotatorItem midItem;
    public IconRotatorItem topItem;
    public IconRotatorItem botItem;
    





    public void SetMonsterRotator(Monster midMonster, List<Monster> bMonsters)
    {
        /*
        Color midColour = GetColourV(midMonster.colour.colour, 1f);
        midItem.SetIconImage(midMonster.dynamicSprite, midMonster.staticSprite, midMonster.variant.variantStillSprite, midColour);

        //Debug.Log(topMonster.name + " " + botMonster.name);

        if (bMonsters.Count <= 0) // 0
        {
            topItem.SetOff();
            botItem.SetOff();
        }
        else if (bMonsters.Count == 1) // 1
        {
            botItem.SetOff();

            Color topColour = GetColourV(bMonsters[0].colour.colour, .5f);
            topItem.SetIconImage(bMonsters[0].dynamicSprite, bMonsters[0].staticSprite, bMonsters[0].variant.variantStillSprite, topColour);
        }
        else // 2
        {
            Color topColour = GetColourV(bMonsters[0].colour.colour, .5f);
            topItem.SetIconImage(bMonsters[0].dynamicSprite, bMonsters[0].staticSprite, bMonsters[0].variant.variantStillSprite, topColour);

            Color botColour = GetColourV(bMonsters[1].colour.colour, .5f);
            botItem.SetIconImage(bMonsters[1].dynamicSprite, bMonsters[1].staticSprite, bMonsters[1].variant.variantStillSprite, botColour);
        }
        */
    }

    private Color GetColourV(Color colour, float value) // value 0-1 0.5 is 50
    {
        float h, s, v;
        Color returnColour;

        Color.RGBToHSV(colour, out h, out s, out v);

        returnColour = Color.HSVToRGB(h, s, value);

        return returnColour;

    }
}
