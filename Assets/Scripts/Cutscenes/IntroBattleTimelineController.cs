using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
public class IntroBattleTimelineController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public float newSpeed = 1;
    public Image dynamicImg;
    public Image staticImg;
    public Image variantImg;

    public void Play(Monster monster)
    {
        dynamicImg.color = Color.black;
        staticImg.color = Color.black;
        variantImg.color = Color.black;

        dynamicImg.sprite = monster.dynamicSprite;
        staticImg.sprite = monster.staticSprite;
        variantImg.sprite = monster.variant.variantStillSprite;

        playableDirector.Play();
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(newSpeed);
    }
}
