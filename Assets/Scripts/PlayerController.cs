using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 20f;
    private float xInput;
    private float zInput;
    // Start is called before the first frame update

    private void Start()
    { 
        rb = GetComponent<Rigidbody>();   
    }

    void Update()
    {
        ProcessInputs();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void ProcessInputs()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

    }

    private void Move()
    {
        rb.AddForce(new Vector3(xInput, 0f, zInput) * moveSpeed);
    }
}
