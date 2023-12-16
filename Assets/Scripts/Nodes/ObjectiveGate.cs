using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveGate : MonoBehaviour
{

    public Canvas canvas;
    public Sprite open;
    public Sprite closed;

    public SpriteRenderer r;

 

    public void Open()
    {
        r.sprite = open;
        if (canvas != null)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    public void Close()
    {
        r.sprite = closed;
        if (canvas != null)
        {
            canvas.gameObject.SetActive(true);
        }
    }




}
