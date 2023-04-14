using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIsGrounded : MonoBehaviour
{
    public bool isGrounded;

    void OnTriggerEnter(Collider other)
    {
        isGrounded = true;
        print("Player is grounded " + isGrounded);
    }

    void OnTriggerExit(Collider other)
    {
        isGrounded = false;
        print("Player is grounded " + isGrounded);
    }
}
