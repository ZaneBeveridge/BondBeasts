using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class JumpHandler : MonoBehaviour
{
    public FriendlyMonsterController controller;
    public Image jumpButton;
    public float jumpStartTime;
    private float jumpTime;
    private bool isJumping;

    private bool active = false;
    void Update()
    {
        

        if (isJumping && active)
        {
            if (jumpTime > 0)
            {
                controller.Jump();
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                jumpButton.color = new Color(1f, 1f, 1f);

            }
        }
        /*
        if (controller.isGrounded || isJumping)
        {
            jumpButton.color = new Color(1f, 1f, 1f);
        }
        else
        {
            jumpButton.color = new Color(0.25f, 0.25f, 0.25f);
        }
        */
    }

    public void Off()
    {
        active = false;
        isJumping = false;
    }

    public void On()
    {
        active = true;
    }


    public void PointerDown()
    {
        if (controller.isGrounded && active)
        {
            isJumping = true;
            jumpTime = jumpStartTime;
            controller.Jump();
            jumpButton.color = new Color(0.25f, 0.25f, 0.25f);
        }

        
    }

    public void PointerUp()
    {
        isJumping = false;
        jumpButton.color = new Color(1f, 1f, 1f);
    }


}
