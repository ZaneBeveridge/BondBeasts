using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlackManager : MonoBehaviour
{
    public Animator anim;
    public GameManager GM;
    private List<GameObject> objectsToHide = new List<GameObject>();
    private List<GameObject> objectsToShow = new List<GameObject>();

    private string type;
    private float timer = 0f;
    private bool doTimer = false;

    void Update()
    {
        if (doTimer)
        {
            if (timer > 0)
            {
                //Debug.Log("Hello");
                timer -= Time.deltaTime;
            }
            else if (timer <= 0)
            {
                timer = 0f;
                doTimer = false;
                anim.SetTrigger("Fade");
            }
        }
        
    }

    public void Fade(List<GameObject> objsToHide, List<GameObject> objsToShow, string ty, float delay) // copy this and add manager for different places sent here to be called in FadeDone()
    {
        objectsToHide = objsToHide;
        objectsToShow = objsToShow;
        type = ty;

        if (delay == 0)
        {
            anim.SetTrigger("Fade");
        }
        else
        {
            timer = delay;
            doTimer = true;
        }
    }


    public void FadeSwitch()
    {
        for (int i = 0; i < objectsToShow.Count; i++)
        {
            objectsToShow[i].SetActive(true);
        }

        for (int i = 0; i < objectsToHide.Count; i++)
        {
            objectsToHide[i].SetActive(false);
        }
    }

    public void FadeDone()
    {
        if (type == "Lose")
        {
            GM.battleManager.MiddleLoss();
        }
        else if (type == "LoseFinal")
        {
            GM.battleManager.Lose();
        }
        else if (type == "Win")
        {
            GM.battleManager.Win();
        }
        else if (type == "WinSurvival")
        {
            GM.battleManager.WinRoundSurvival(false);
        }
        else if (type == "Capture")
        {
            GM.battleManager.Capture();
        }
        //Tell whatever send it here to continue here
    }
}
