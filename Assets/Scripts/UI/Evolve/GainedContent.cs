using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GainedContent : MonoBehaviour
{
    public TextMeshProUGUI text;



    public void Init(string t)
    {
        text.text = t;
    }
}
