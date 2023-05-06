using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IConsumer
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Light _light;

    [SerializeField] private GameObject _caterpillarMesh;
    [SerializeField] private GameObject _butterflyMesh;
    
    private IEnumerator _massChangeCoroutine;
    private MassChanger _massChanger;

    const float METAMORPHOSIS_THRESHOLD_WEIGHT = 60;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cam;
    [SerializeField] private CinemachineFreeLook freeLookVirtualCam;

    private IEnumerator enableCamRepositioning;
    private bool _axisRecenteringEnableCoroutineRunning;

    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _waitBeforeCameraReset = 5f;
    private Vector3 velocity;

    private bool _isGrounded;
    private bool _delayReposition;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private float turnSmoothVelocity;
    [SerializeField] private float turnSmoothTime = 0.1f;

    private void Update()
    {
        //jump
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (_isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }

        //gravity
        velocity.y += _gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        HandleMovementInput();
    }

    public void Start()
    {
        _massChanger = new MassChanger(this);
        _massChangeCoroutine = _massChanger.MassChangeCoroutine();
    }

    private void StartMassChangeCoroutine(IEnumerator _massChangeCoroutine)
    {
        StartCoroutine(_massChangeCoroutine);
    }

    private void StopMassChangeCoroutine()
    {
        StopCoroutine(_massChangeCoroutine);
    }

    public void SetWeight(float newWeight)
    {
        if(newWeight < 1)
        {
            newWeight = 1;
        }

        if(newWeight > METAMORPHOSIS_THRESHOLD_WEIGHT)
        {
            BecomeButterfly();
        }
        else
        {
            _rigidbody.mass = newWeight;
        }
    }

    private void BecomeButterfly()
    {
        Debug.LogWarning("I became a butterfly!");

        _caterpillarMesh.gameObject.SetActive(false);
        _butterflyMesh.gameObject.SetActive(true);
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    public Light GetLight()
    {
        return _light;
    }
    private void HandleMovementInput()
    {
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
            controller.Move(moveDir.normalized * _speed * Time.deltaTime);

            _delayReposition = true;
            StopCoroutine(AxisRecenteringEnableCoroutine());

            if (!_massChanger.IsCalculating)
            {
                StartCoroutine(_massChangeCoroutine);
                _massChanger.IsCalculating = true;
            }
        }
        else
        {
            _delayReposition = false;

            if (_massChanger.IsCalculating)
            {
                StopCoroutine(_massChangeCoroutine);
                _massChanger.IsCalculating = false;
            }

            if (!_axisRecenteringEnableCoroutineRunning)
            {
                StartCoroutine(AxisRecenteringEnableCoroutine());
            }
        }
    }

    private IEnumerator AxisRecenteringEnableCoroutine()
    {
        _axisRecenteringEnableCoroutineRunning = true;
        yield return new WaitForSeconds(_waitBeforeCameraReset);

        if (_delayReposition)
        {
            enableCamRepositioning = null;
            _axisRecenteringEnableCoroutineRunning = false;
            yield break;
        }

        freeLookVirtualCam.m_YAxisRecentering.m_enabled = true;
        freeLookVirtualCam.m_RecenterToTargetHeading.m_enabled = true;
        _axisRecenteringEnableCoroutineRunning = false;
        yield return null;
    }
}
