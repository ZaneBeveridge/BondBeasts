using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class JumpHandler : MonoBehaviour
{
    public FriendlyMonsterController controller;
    public Image jumpButton;

    public Animator anim;
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
                jumpButton.color = new Color(0.4f, 0.4f, 0.4f);
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                jumpButton.color = new Color(0.4f, 0.4f, 0.4f);
                //anim.SetBool("Big", false);
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

        if (controller.isGrounded)
        {
            jumpButton.color = new Color(1f, 1f, 1f);
        }
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
            jumpButton.color = new Color(0.4f, 0.4f, 0.4f);
            anim.SetBool("Big", true);
        }

        
    }

    public void PointerUp()
    {
        isJumping = false;
        jumpButton.color = new Color(0.4f, 0.4f, 0.4f);
        anim.SetBool("Big", false);
    }


}
