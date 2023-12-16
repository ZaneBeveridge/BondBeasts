using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetTrigger("Start");
    }

    public void End()
    {
        animator.SetTrigger("End");
    }
}
