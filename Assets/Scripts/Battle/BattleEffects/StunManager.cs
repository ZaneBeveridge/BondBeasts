using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunManager : MonoBehaviour
{
    public SpriteRenderer starImage;

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
            starImage.gameObject.SetActive(false);
        }
        else
        {
            starImage.gameObject.SetActive(true);
            starImage.sprite = starSprites[amountOfStars - 1];
        }
    }


    private IEnumerator SpinStar()
    {
        while (true)
        {
            starImage.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
