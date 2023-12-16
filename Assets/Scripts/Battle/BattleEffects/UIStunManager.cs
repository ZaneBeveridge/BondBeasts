using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStunManager : MonoBehaviour
{
    public List<Image> starImages = new List<Image>();

    public List<Sprite> starSprites = new List<Sprite>();

    public float rotationSpeed = 100f;

    private float tickTimer = 0;
    private bool stunActive = false;

    private int amountOfStars = 0;
    void Update()
    {
        if (stunActive)
        {


            if (tickTimer > 0)
            {
                tickTimer -= Time.deltaTime;
            }
            else if (tickTimer <= 0)
            {
                if (amountOfStars > 0)
                {
                    amountOfStars -= 1;
                    UpdateStars();
                    tickTimer = 1f;
                }
                else
                {
                    StopStun();
                }
            }
        }
    }


    public void Stun(float time)
    {
        amountOfStars = (int)time;
        tickTimer = 1f;
        UpdateStars();
        stunActive = true;

        StartCoroutine(SpinStar());
    }

    public void StopStun()
    {
        amountOfStars = 0;
        tickTimer = 0f;
        UpdateStars();
        stunActive = false;

        StopCoroutine(SpinStar());
    }

    public void UpdateStars()
    {
        if (amountOfStars <= 0)
        {
            for (int i = 0; i < starImages.Count; i++)
            {
                starImages[i].gameObject.SetActive(false);
            }
            
        }
        else
        {
            for (int i = 0; i < starImages.Count; i++)
            {
                starImages[i].gameObject.SetActive(true);
                starImages[i].sprite = starSprites[amountOfStars - 1];
            }

            
        }
    }


    private IEnumerator SpinStar()
    {
        while (true)
        {
            for (int i = 0; i < starImages.Count; i++)
            {
                starImages[i].rectTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
            }
            
            yield return null;
        }
    }
}
