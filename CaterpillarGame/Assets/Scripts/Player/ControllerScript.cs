using UnityEngine;
using Cinemachine;
using System.Collections;

public class ControllerScript : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cam;
    [SerializeField] private CinemachineFreeLook freeLookVirtualCam;
    private Coroutine enableCamRepositioning;

    [SerializeField] private float speed = 6f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float waitBeforeCameraReset = 5f;
    private Vector3 velocity;

    private bool isGrounded;
    private bool delayReposition;
    private bool coroutineRunning;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private float turnSmoothVelocity;
    [SerializeField] private float turnSmoothTime = 0.1f;

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
        yield return new WaitForSeconds(waitBeforeCameraReset);
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