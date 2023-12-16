using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattleHealthBar : MonoBehaviour
{
    [Header("Colours")]
    public Color normalColorFront;
    public Color normalColorBack;

    public Color stunColorFront;
    public Color stunColorBack;

    public Color guardColorFront;
    public Color guardColorBack;

    public Color dotColorFront;
    public Color dotColorBack;

    public Color regenColorFront;
    public Color regenColorBack;

    public Image frontImage;
    public Image backImage;

    public Slider slider;
    public TextMeshProUGUI healthText;


    public GameObject pulsingOverlay;
    //public Animator pulsingOverlayAnim;

    public GameObject capBar;
    public GameObject capText;
    public GameObject capExtra;
    public GameObject capArt;

    private string maxHealthText = "100";

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        maxHealthText = maxHealth.ToString();
    }

    public void ResetHealth()
    {
        slider.value = slider.maxValue;
        healthText.text = slider.value.ToString();
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        healthText.text = slider.value.ToString();

        if (health < 35f)
        {
            pulsingOverlay.SetActive(true);
        }
        else
        {
            pulsingOverlay.SetActive(false);
        }


    }


    public void ChangeColor(string colorString)
    {
        if (colorString == "Normal")
        {
            frontImage.color = normalColorFront;
            backImage.color = normalColorBack;
        }
        else if (colorString == "Yellow")
        {
            frontImage.color = stunColorFront;
            backImage.color = stunColorBack;
        }
        else if (colorString == "Blue")
        {
            frontImage.color = guardColorFront;
            backImage.color = guardColorBack;
        }
        else if (colorString == "Red")
        {
            frontImage.color = dotColorFront;
            backImage.color = dotColorBack;
        }
        else if (colorString == "BrightGreen")
        {
            frontImage.color = regenColorFront;
            backImage.color = regenColorBack;
        }

    }

    public void ActivateCapBar(bool state)
    {
        if (state == true)
        {
            capBar.SetActive(true);
            capText.SetActive(true);
            capExtra.SetActive(true);
            capArt.SetActive(true);
        }
        else if (state == false)
        {
            capBar.SetActive(false);
            capText.SetActive(false);
            capExtra.SetActive(false);
            capArt.SetActive(false);
        }
    }
}
