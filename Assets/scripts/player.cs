using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class player : MonoBehaviour
{
    InputAction left;
    InputAction right;
    InputAction space;

    Vector3 camOffset;

    public CinemachineCamera cam;

    public Rigidbody rb;

    public Vector3 up = new Vector3(0,0,1);
    public Vector3 grav = new Vector3(0,1,0);
    public float angle = 45f;

    public float speed = 1;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        left = InputSystem.actions.FindAction("Left");
        right  = InputSystem.actions.FindAction("Right");
        space  = InputSystem.actions.FindAction("Test");
        camOffset = new Vector3(0f, 10.4f, -2.5f) * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (space.WasPressedThisFrame())
        {
            //rotate everything
            /*
             * up
             * grav
             * camOffset
             */

            rb.linearVelocity = new Vector3(0, 0, 0);
            up = new Vector3(0,1,0);
            grav = new Vector3(0, 0, -1);
            camOffset = Quaternion.AngleAxis(90f, Vector3.Cross(up, grav)) * camOffset;
            cam.transform.Rotate(Vector3.Cross(up, grav), 90f);
        }

        if (left.WasPressedThisFrame())
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
            Vector3 force = Quaternion.AngleAxis(angle, grav) * up;
            rb.AddForce(force * 10 * rb.mass * speed , ForceMode.Impulse);

        }

        if (right.WasPressedThisFrame())
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
            Vector3 force = Quaternion.AngleAxis(-angle, grav) * up;
            rb.AddForce(force * 10 * rb.mass * speed , ForceMode.Impulse);
        }

        cam.transform.position = this.transform.position + camOffset;

        // make the ball fall
        rb.AddForce(-up * rb.mass * 20);

        //push the ball against the table
        rb.AddForce(-grav * rb.mass * 20);

        Vector3 totalForce = rb.linearVelocity;

        if (totalForce.magnitude < .1f)
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
            print("stop");
        }

    }
}
