using UnityEngine;
using Cinemachine;
using System.Collections;

public class ControllerScript : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] Transform cam;
    [SerializeField] CinemachineFreeLook freeLookVirtualCam;
    Coroutine enableCamRepositioning;

    [SerializeField] float speed = 6;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 3;
    Vector3 velocity;
    bool isGrounded;
    bool delayReposition;
    bool coroutineRunning;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    float turnSmoothVelocity;
    [SerializeField] float turnSmoothTime = 0.1f;

    // Update is called once per frame
    void Update()
    {
        //jump
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
  
        if (direction.magnitude >= 0.1f)
        {
            freeLookVirtualCam.m_YAxisRecentering.m_enabled = false;
            freeLookVirtualCam.m_RecenterToTargetHeading.m_enabled = false;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            delayReposition = true;
            StopCoroutine(EnableAxisRecentering());
        }
        else
        {
            delayReposition = false;
            if (!coroutineRunning)
                StartCoroutine(EnableAxisRecentering());
        }
    }

    IEnumerator EnableAxisRecentering()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(10);
        if (delayReposition)
        {
            enableCamRepositioning = null;
            coroutineRunning = false;
            yield break; 
        }
        freeLookVirtualCam.m_YAxisRecentering.m_enabled = true;
        freeLookVirtualCam.m_RecenterToTargetHeading.m_enabled = true;
        coroutineRunning = false;
        yield return null;
    }
}