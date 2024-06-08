using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneImagePrefab : MonoBehaviour
{
    public int index;
    public Animator anim;
    public Image image;

    private bool doDestroy = false;
    public void Show()
    {
        anim.SetTrigger("ShowInstant");
    }

    public void Hide()
    {
        anim.SetTrigger("HideInstant");
        doDestroy = true;
    }

    public void FadeIn()
    {
        anim.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        anim.SetTrigger("FadeOut");
        doDestroy = true;
    }

    public void Shake(bool destroy)
    {
        anim.SetTrigger("Shake");
        doDestroy = destroy;
    }

    public void DoDestroy()
    {
        if (doDestroy)
        {
            Destroy(this.gameObject);
        }
    }


}
