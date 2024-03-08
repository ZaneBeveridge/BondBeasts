using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CaptureButton : MonoBehaviour
{
    public BattleManager manager;

    public float chargeAmount = 10.0f; // The time it takes to fully charge
    public float decayAmount = 2f;
    public Image chargeButton; // The button used to charge
    public Transform fillObject;
    public Animator anim;
    public Animator captureBarAnim;
    public Animator cameraAnim;

    [SerializeField]private float currentCharge = 0.0f; // The current charge level
    private bool isCharging = false; // Whether or not the player is currently charging

    private float loopCharge = 0.1f;

    void Start()
    {
        /*
        // Get the EventTrigger component from the button
        EventTrigger eventTrigger = chargeButton.GetComponent<EventTrigger>();

        // Create a new entry for the onPointerDown event
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((eventData) => { Charge(); });
        eventTrigger.triggers.Add(pointerDownEntry);

        // Create a new entry for the onPointerUp event
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((eventData) => { StopCharging(); });
        eventTrigger.triggers.Add(pointerUpEntry);
        */
    }

    void Update()
    {
        //Debug.Log(realChargeTime);
        if (isCharging)
        {

            if (loopCharge > 0)
            {
                loopCharge -= Time.deltaTime;
            }
            else if (loopCharge <= 0)
            {
                currentCharge += chargeAmount;
                loopCharge = 0.1f;
            }
        }
        else
        {
            if (loopCharge > 0)
            {
                loopCharge -= Time.deltaTime;
            }
            else if (loopCharge <= 0)
            {
                if (currentCharge > 0f)
                {
                    currentCharge -= decayAmount;
                    loopCharge = 0.5f;
                }
            }

            
        }

        manager.enemyMonsterController.capBar.SetCaptureLevel(currentCharge, manager.enemyMonsterController.enemyHealth);
        manager.enemyMonsterController.capBar.SetMaxCapturePoints(manager.enemyCapturePoints, manager.enemyMonsterController.enemyHealth);

        //fillObject.localScale = new Vector3(1f, currentCharge / 1f, 1f);

        float realCap = 0f;

        if (manager.enemyMonsterController.enemyHealth > 25)
        {
            realCap = manager.enemyCapturePoints * (manager.enemyMonsterController.enemyHealth / 100f);
        }
        else
        {
            realCap = manager.enemyCapturePoints * 0.25f;
        }

        if (currentCharge >= realCap)
        {
            manager.Capture();
            StopCharging();
            currentCharge = 0f;
        }
    }

    // Called when the button is pressed
    public void PointerDown()
    {
        if (manager.controlsActive)
        {
            Charge();
            manager.PauseControlsExceptCapture();
            
        }
        
    }

    // Called when the button is released
    public void PointerUp()
    {
        if (isCharging)
        {
            
            StopCharging();
            manager.ResumeControls();
        }
        

    }

    void Charge()
    {
        loopCharge = 0.1f;
        isCharging = true;
        chargeButton.color = new Color(0.25f, 0.25f, 0.25f);
        anim.SetBool("On", true);
        captureBarAnim.SetBool("Capturing", true);
        cameraAnim.SetBool("Capturing", true);
    }

    public void StopCharging()
    {
        loopCharge = 0f;
        isCharging = false;
        chargeButton.color = new Color(1f, 1f, 1f);
        anim.SetBool("On", false);
        captureBarAnim.SetBool("Capturing", false);
        cameraAnim.SetBool("Capturing", false);
        //currentCharge = 0.0f;
    }

    public void ResetBar()
    {
        currentCharge = 0f;
        loopCharge = 0f;
        isCharging = false;
    }
}
