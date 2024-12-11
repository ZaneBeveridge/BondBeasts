using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScaleToAspectRatio : MonoBehaviour
{
    public Camera cam;
    public CanvasScaler scaler;

    public void Start()
    {
        Scale();
    }

    public void Scale()
    {
        //Debug.Log(Camera.main.aspect);


        if (Camera.main.aspect >= 1.8) // 1920x1080 +++
        {
            //Debug.Log("16:9++++");
            cam.orthographicSize = 5.6f;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
        }
        else if (Camera.main.aspect >= 1.7) // 1920x1080
        { 
            //Debug.Log("16:9");
            cam.orthographicSize = 6.4f;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
        }
        else if (Camera.main.aspect >= 1.6) // 1920x1200
        {
            //Debug.Log("16:10");
            cam.orthographicSize = 6.5f;
            scaler.referenceResolution = new Vector2(1200f, 1920f);
        }   
        else if (Camera.main.aspect >= 1.5) // 1920x1280
        {
            //Debug.Log("3:2");
            cam.orthographicSize = 6.7f;
            scaler.referenceResolution = new Vector2(1280f, 1920f);
        }    
        else// 1920x1440
        {
            //Debug.Log("4:3");
            cam.orthographicSize = 6.7f;
            scaler.referenceResolution = new Vector2(1440f, 1920f);
        }
    }
}
