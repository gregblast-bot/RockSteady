using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;

    public Transform cam;

    // How to get frame rate?
    public float speed = 6f;

    public float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");  // A -> D OR L -> R [-1, 1]
        float vertical = Input.GetAxisRaw("Vertical");      // S -> W OR D -> U [-1, 1]
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}

//var horizontalInput = Input.GetAxis("horizontal");
//var verticalInput = Input.GetAxis("vertical");

//if (horizontalInput < 0)
//{
//    sharedKey = KeyState.LeftKey;
//}
//else if (horizontalInput > 0)
//{
//    sharedKey = KeyState.RightKey;
//}
//if (verticalInput < 0)
//{
//    sharedKey = KeyState.DownKey;
//}
//else if (verticalInput > 0)
//{
//    sharedKey = KeyState.UpKey;
//}
