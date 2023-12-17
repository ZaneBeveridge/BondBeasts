using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class EnhancedTouchManager : MonoBehaviour
{
    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
    }

    private void Start()
    {
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        Debug.Log("What?");
    }
}
