using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CutsceneTextController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    
    public Image backingImage;

    public Animator anim;

    public void Init(string text, string speaker, Color textColour, Color speakerColour, Color backgroundColour)
    {
        dialogueText.text = text;
        speakerNameText.text = speaker;
        dialogueText.color = textColour;
        speakerNameText.color = speakerColour;
        backingImage.color = backgroundColour;
    }


    public void Show()
    {
        anim.SetTrigger("ShowInstant");
    }

    public void Hide()
    {
        anim.SetTrigger("HideInstant");
    }

    public void FadeIn()
    {
        anim.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        anim.SetTrigger("FadeOut");
    }

    public void Shake()
    {
        anim.SetTrigger("Shake");
    }

}
