using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleCapBar : MonoBehaviour
{
    public RectTransform capBar;
    public RectTransform fillBar;
    public RectTransform artCap;

    public Slider slider;
    public TextMeshProUGUI healthText;
    private float maxCapture;

    //public GameManager GM;

    public void SetSize(float health)
    {
        float cPosX = 635 - (health * 6.35f);
        float fWidth = health * 6.35f;

        capBar.localPosition = new Vector3(cPosX, 0f, 0f);
        fillBar.sizeDelta = new Vector2(fWidth, 60f);
        artCap.localPosition = new Vector2(cPosX, 0f);
    }

    public void SetMaxCapturePoints(float maxCapturePoints, float health) // Sets the max value of the slider
    {
        //Debug.Log(maxCapturePoints.ToString());
        //Debug.Log(health.ToString()) ;
        float realCap = 0f;
        float realHealth = 0f;

        if (health > 25f)
        {
            realCap = maxCapturePoints * (health / 100f);
            realHealth = health;
        }
        else
        {
            realCap = maxCapturePoints * 0.25f;
            realHealth = 25f;
        }

        slider.maxValue = (int)realCap;
        maxCapture = (int)realCap;

        healthText.text = slider.value.ToString() + " / " + maxCapture.ToString();

        SetSize(realHealth);
    }

    public void ResetCapBar()
    {
        slider.value = slider.maxValue;
        healthText.text = slider.value.ToString() + " / " + maxCapture.ToString();

        SetSize(100f);
    }

    public void SetCaptureLevel(float amount, float health)
    {
        slider.value = (int)amount;
        healthText.text = slider.value.ToString() + " / " + maxCapture.ToString();

        SetSize(health);
    }
}
