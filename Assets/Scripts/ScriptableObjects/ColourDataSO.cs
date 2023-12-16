using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColourData", menuName = "SO/Colour/ColourData", order = 1)]
public class ColourDataSO : ScriptableObject
{
    public List<ColourData> datas = new List<ColourData>();

    private void Awake()
    {
        if (datas.Count <= 0)
        {
            datas.Add(new ColourData(ColourWheel.Red1, Color.red));
            datas.Add(new ColourData(ColourWheel.Red2, Color.red));
            datas.Add(new ColourData(ColourWheel.Red3, Color.red));
            datas.Add(new ColourData(ColourWheel.Orange1, Color.red));
            datas.Add(new ColourData(ColourWheel.Orange2, Color.red));
            datas.Add(new ColourData(ColourWheel.Orange3, Color.red));
            datas.Add(new ColourData(ColourWheel.Yellow1, Color.red));
            datas.Add(new ColourData(ColourWheel.Yellow2, Color.red));
            datas.Add(new ColourData(ColourWheel.Yellow3, Color.red));
            datas.Add(new ColourData(ColourWheel.Green1, Color.red));
            datas.Add(new ColourData(ColourWheel.Green2, Color.red));
            datas.Add(new ColourData(ColourWheel.Green3, Color.red));
            datas.Add(new ColourData(ColourWheel.Blue1, Color.red));
            datas.Add(new ColourData(ColourWheel.Blue2, Color.red));
            datas.Add(new ColourData(ColourWheel.Blue3, Color.red));
            datas.Add(new ColourData(ColourWheel.Purple1, Color.red));
            datas.Add(new ColourData(ColourWheel.Purple2, Color.red));
            datas.Add(new ColourData(ColourWheel.Purple3, Color.red));
        }

    }


}

[System.Serializable]
public class ColourData
{
    [HideInInspector]public string name;
    public ColourWheel colourWheel;
    public Color startColour;
    public Color endColour;

    public ColourData(ColourWheel w, Color c)
    {
        colourWheel = w;
        startColour = c;
        endColour = c;
        name = colourWheel.ToString();
    }

}

public enum ColourWheel
{
    Red1,
    Red2,
    Red3,
    Orange1,
    Orange2,
    Orange3,
    Yellow1,
    Yellow2,
    Yellow3,
    Green1,
    Green2,
    Green3,
    Blue1,
    Blue2,
    Blue3,
    Purple1,
    Purple2,
    Purple3,

}
