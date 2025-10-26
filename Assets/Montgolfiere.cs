using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Montgolfiere : MonoBehaviour
{
    private Rigidbody rb;
    public float upForce = 0f;
    public float riseForce = 2f;
    public float fallForce = 1f;
    public float maxUpForce = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            upForce += riseForce;
            rb.linearDamping = 3; // add resistance when changing direction
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            upForce -= fallForce;
            rb.linearDamping = 5; // Very floaty when going down
        }
        else
        {
            upForce = 0;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        }

        upForce = Math.Clamp(upForce, 0, maxUpForce);

        rb.AddForce(Vector3.up * upForce, ForceMode.Force);

        Debug.Log(rb.linearVelocity);
    }
}
