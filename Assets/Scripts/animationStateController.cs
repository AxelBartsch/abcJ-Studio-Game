using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    PlayerIsGrounded grounded;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        grounded = GetComponent<PlayerIsGrounded>();
    }

    // Update is called once per frame
    void Update()
    {
        Running();
        Jump();

        
        
    }

    void Running()
    {
        if (Input.GetKey("w") && grounded.isGrounded)
        {
            animator.SetBool("isRunning", true);
        }

        if (!Input.GetKey("w"))
        {
            animator.SetBool("isRunning", false);
        }
    }

    void Jump()
    {
        if (Input.GetKey("space") && grounded.isGrounded)
        {
            animator.SetBool("jumpPressed", true);

        }

        if (!Input.GetKey("space") && !grounded.isGrounded)
        {
            animator.SetBool("jumpPressed", false);
            animator.SetBool("midAir", true);
        }

        if (grounded.isGrounded)
        {
            animator.SetBool("midAir", false);
            animator.SetBool("isLanding", true);
        }

        if (!Input.GetKey("w") && grounded.isGrounded)
        {
            animator.SetBool("isLanding", false);
            animator.SetBool("isRunning", true);
        }
    }
}
