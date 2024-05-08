using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public float newSpeed;
    public PlayableDirector playableDirector;

 

    public void Play()
    {
        playableDirector.Play();
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(newSpeed);
    }

    public void Pause()
    {
        playableDirector.Pause();
    }

    public void Resume()
    {
        playableDirector.Resume();
    }
}


