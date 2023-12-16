using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskCutout : MonoBehaviour
{
    // The rect transform component to scale
    public RectTransform rectTransform;

    // The time it takes to complete the scale change
    private float duration = 0f;

    // The starting scale of the rect transform
    private Vector3 startScale = new Vector3(-6f, -6f, -6f);

    // The target scale of the rect transform
    private Vector3 targetScale = new Vector3(0f, 0f, 0f);

    // The elapsed time since the scale change started
    private float elapsedTime = 0f;

    private bool isPlaying = false;

    void Start()
    {
        // Set the starting scale of the rect transform
        rectTransform.localScale = startScale;
    }

    void Update()
    {
        if (isPlaying)
        {
            // Increase the elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the new scale based on the elapsed time and duration
            float t = Mathf.Clamp01(elapsedTime / duration);
            Vector3 newScale = Vector3.Lerp(startScale, targetScale, t);

            // Update the scale of the rect transform
            

            // If the scale change is complete, stop playing and reset elapsed time
            if (t < 0.8f)
            {
                rectTransform.localScale = newScale;
            }

            if (t >= .8f)
            {

                rectTransform.localScale = new Vector3(.5f, .5f, .5f);
            }

            if (t >= 1f)
            {
                isPlaying = false;
                elapsedTime = 0f;
                rectTransform.localScale = startScale;
            }
        }
    }


    public void Play(float time)
    {
        rectTransform.localScale = startScale;
        // Set the target scale and start playing
        targetScale = Vector3.zero;
        duration = time;
        isPlaying = true;
    }
}
