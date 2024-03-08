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
            datas.Add(new ColourData(ColourWheel.Red, Color.red));
            datas.Add(new ColourData(ColourWheel.Orange, Color.red));
            datas.Add(new ColourData(ColourWheel.Yellow, Color.red));
            datas.Add(new ColourData(ColourWheel.Lime, Color.red));
            datas.Add(new ColourData(ColourWheel.Green, Color.red));
            datas.Add(new ColourData(ColourWheel.Teal, Color.red));
            datas.Add(new ColourData(ColourWheel.Sky, Color.red));
            datas.Add(new ColourData(ColourWheel.Blue, Color.red));
            datas.Add(new ColourData(ColourWheel.Purple, Color.red));
            datas.Add(new ColourData(ColourWheel.Pink, Color.red));
        }

    }


}

[System.Serializable]
public class ColourData
{
    public string name;
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
    Red,
    Orange,
    Yellow,
    Lime,
    Green,
    Teal,
    Sky,
    Blue,
    Purple,
    Pink
}
