using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpAmount = 10;

    public PlayerIsGrounded grounded;
    public bool doubleJumpAvailable = true;
    
    public float gravityScale = 10;
    public float fallingGravityScale = 40;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grounded = GetComponent<PlayerIsGrounded>();
    }

    // Update is called once per frame
    void Update()
    {
        ResetDoubleJump();
        if (Input.GetKeyDown(KeyCode.Space) && grounded.isGrounded)
        {
            rb.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
            print("jumped");
        }

        if(Input.GetKeyDown(KeyCode.Space) && !grounded.isGrounded && rb.velocity.y >= 0 && doubleJumpAvailable)
        {
            rb.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
            doubleJumpAvailable = false;
            print("jumped");
        }

    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);

        if(rb.velocity.y < 0)
        {
            rb.AddForce(Physics.gravity * (fallingGravityScale - 1) * rb.mass);
        }
    }

    void ResetDoubleJump()
    {
        if (grounded.isGrounded)
        {
            doubleJumpAvailable = true;
        }
    }
}
