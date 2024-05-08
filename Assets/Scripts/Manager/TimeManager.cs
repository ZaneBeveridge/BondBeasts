using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    private bool frozen = false;

    void Update()
    {
        if (!frozen & Time.timeScale < 1f)
        {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
        
    }

    public void DoSlowMotion(float length)
    {
        slowdownLength = length;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void Pause(float time)
    {
        if (time == 0)
        {
            frozen = true;
        }
        else if (time == 1)
        {
            frozen = false;
        }

        Time.timeScale = time;
    }
}
