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

    public Color perfectGuardColorFront;
    public Color perfectGuardColorBack;

    public Color dotColorFront;
    public Color dotColorBack;

    public Color regenColorFront;
    public Color regenColorBack;

    public Image frontImage;
    public Image backImage;

    public Slider slider;
    public Slider easeSlider;
    public TextMeshProUGUI healthText;

    private float lerpSpeed = 0.05f;


    public GameObject pulsingOverlay;
    //public Animator pulsingOverlayAnim;

    public GameObject dotMaterialObject;
    public GameObject regenMaterialObject;

    public GameObject guardObject;
    public GameObject perfectGuardObject;

    public GameObject capBar;
    public GameObject capText;
    public GameObject capExtra;
    public GameObject capArt;

    private string maxHealthText = "100";


    //private bool startTimerForEaseSlider = false;
    private float timeUntilEaseStarts = .75f;
    private float easeTimer = 0f;

    private float currentAimingHealth = 0f;

    void Update()
    {
        if (easeSlider != null)
        {
            if (easeTimer > 0)
            {
                easeTimer -= Time.deltaTime;
            }
            else if (easeTimer <= 0)
            {
                easeSlider.value = Mathf.Lerp(easeSlider.value, currentAimingHealth, lerpSpeed);
            }
        }
        
    }

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        if (easeSlider != null)
        {
            easeSlider.maxValue = maxHealth;
        }
        
        maxHealthText = maxHealth.ToString();
    }

    public void ResetHealth()
    {
        slider.value = slider.maxValue;
        if (easeSlider != null)
        {
            easeSlider.maxValue = slider.maxValue;
        }
        
        healthText.text = slider.value.ToString();
    }

    public void SetHealth(float health, bool dmg)
    {
        slider.value = health;
        healthText.text = slider.value.ToString();

        currentAimingHealth = health;

        if (dmg)
        {
            easeTimer = timeUntilEaseStarts;
        }
        else
        {
            if (easeSlider != null)
            {
                easeSlider.value = health;
            }
            
        }

        if (pulsingOverlay != null)
        {
            if (health < 35f)
            {
                pulsingOverlay.SetActive(true);
            }
            else
            {
                pulsingOverlay.SetActive(false);
            }
        }
    }


    public void ChangeColor(string colorString)
    {
        if (dotMaterialObject != null) { dotMaterialObject.SetActive(false); };
        if (regenMaterialObject != null) { regenMaterialObject.SetActive(false); };
        if (guardObject != null) { guardObject.SetActive(false); };
        if (perfectGuardObject != null) { perfectGuardObject.SetActive(false); };

        if (colorString == "Normal")
        {
            frontImage.color = normalColorFront;
            backImage.color = normalColorBack;
        }
        else if (colorString == "Stun")
        {
            frontImage.color = stunColorFront;
            backImage.color = stunColorBack;
        }
        else if (colorString == "Guard")
        {
            frontImage.color = guardColorFront;
            backImage.color = guardColorBack;
            guardObject.SetActive(true);
        }
        else if (colorString == "PerfectGuard")
        {
            frontImage.color = perfectGuardColorFront;
            backImage.color = perfectGuardColorBack;
            perfectGuardObject.SetActive(true);
        }
        else if (colorString == "Dot")
        {
            frontImage.color = dotColorFront;
            backImage.color = dotColorBack;
            dotMaterialObject.SetActive(true);
        }
        else if (colorString == "Regen")
        {
            frontImage.color = regenColorFront;
            backImage.color = regenColorBack;
            regenMaterialObject.SetActive(true);
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
