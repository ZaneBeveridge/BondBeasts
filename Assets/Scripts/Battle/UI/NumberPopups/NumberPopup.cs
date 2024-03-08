using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberPopup : MonoBehaviour
{
    [Header("Changable Text, leave empty to use static text")]
    public TextMeshProUGUI text;
    public TextMeshProUGUI extraText;

    [Header("Animator")]
    public Animator anim;
    [Header("Set this just over the length of anim")]
    public float timeToDestroy;

    public void Init(string txt, int eTxt)
    {
        if (text != null)
        {
            text.text = txt;
        }

        if (extraText != null)
        {
            if (eTxt > 0)
            {
                extraText.text = "+" + eTxt.ToString();
            }
            else if (eTxt < 0)
            {
                extraText.text = eTxt.ToString();
            }
            else
            {
                extraText.text = "";
            }

            
        }

        anim.SetTrigger("Start");

        Destroy(this.gameObject, timeToDestroy);
    }
}
