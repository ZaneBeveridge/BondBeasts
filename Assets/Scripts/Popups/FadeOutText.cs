using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeOutText : MonoBehaviour
{
    public TextMeshProUGUI text1;

    private bool fading = false;
    private float fadeTimer = 0f;
    private float maxTime = 0f;

    private bool moving = false;
    private float movingFadeTimer = 0f;
    private float movingMaxTime = 0f;
    void Update()
    {
        if (moving)
        {
            if (movingFadeTimer < 1)
            {
                movingFadeTimer += Time.deltaTime / movingMaxTime;
                text1.rectTransform.localScale = new Vector3(movingFadeTimer / 1, movingFadeTimer / 1, movingFadeTimer / 1);
            }
            else if (movingFadeTimer >= 1)
            {
                text1.rectTransform.localScale = new Vector3(1f, 1f, 1f);
                fading = true;
                moving = false;
            }
        }


        if (fading)
        {
            if (fadeTimer > 0)
            {
                fadeTimer -= Time.deltaTime / maxTime;
                text1.color = new Color(text1.color.r, text1.color.g, text1.color.b, fadeTimer);
            }
            else if (fadeTimer <= 0)
            {
                fading = false;
                Destroy(this.gameObject);
            }
        }
    }

    public void Fade(float timeToFade)
    {
        maxTime = timeToFade / 2;
        fadeTimer = timeToFade / 2;

        movingMaxTime = timeToFade / 2;
        movingFadeTimer = 0f;
        moving = true;
        fading = false;
    }
}
