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

    public float cameraOffsetScale = 2;
    public Vector3 cameraOffsetPosition = new Vector3(0f, 10.4f, -2.5f);

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
        camOffset = cameraOffsetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (space.WasPressedThisFrame())
        {

        }

        //launches right
        if (left.WasPressedThisFrame())
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
            Vector3 force = Quaternion.AngleAxis(angle, grav) * up;
            rb.AddForce(force * 10 * rb.mass * speed , ForceMode.Impulse);

        }

        //launches left
        if (right.WasPressedThisFrame())
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
            Vector3 force = Quaternion.AngleAxis(-angle, grav) * up;
            rb.AddForce(force * 10 * rb.mass * speed , ForceMode.Impulse);
        }

        //update the camera
        cam.transform.position = this.transform.position + (camOffset * cameraOffsetScale);
        cam.transform.LookAt(this.transform, up);

        // make the ball fall
        rb.AddForce(-up * rb.mass * 40);

        //push the ball against the table
        rb.AddForce(-grav * rb.mass * 20);

        //an attempt to give the player a threshold of minimum speed
        Vector3 totalForce = rb.linearVelocity;
        if (totalForce.magnitude < .1f)
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
            print("stop");
        }

    }

    //changes the player's reletive gravety, and changes the camera to match
    public void ChangeGrav(Vector3 ballUp, Vector3 camUp, bool changeCam = true)
    {
        rb.linearVelocity = new Vector3(0, 0, 0);
        //find the fixed angle for camera
        float camAngle = Vector3.Angle(new Vector3(0,1,0), cameraOffsetPosition);

        up = ballUp;
        grav = camUp;

        if (changeCam)
        {
            Vector3 right = Vector3.Cross(grav, up);

            // get new up length of offset
            Vector3 tmp = Vector3.Normalize(grav) * cameraOffsetPosition.magnitude;

            //rotate it fixed degrees
            camOffset = Quaternion.AngleAxis(camAngle, Vector3.Cross(up, grav)) * tmp;
        }
    }
}
