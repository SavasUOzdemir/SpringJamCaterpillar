using Cinemachine;
using System.Collections;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IConsumer, IMonoBehaviourSingleton
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Light _light;

    [SerializeField] private GameObject _caterpillarMesh;
    [SerializeField] private GameObject _butterflyMesh;

    private IEnumerator _walkingSoundRoutine;
    private IEnumerator _massChangeCoroutine;
    private MassChanger _massChanger;

    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _camera;
    [SerializeField] public CinemachineFreeLook freeLookVirtualCam;

    private bool _axisRecenteringEnableCoroutineRunning;

    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _waitBeforeCameraReset = 5f;
    private Vector3 _velocity;

    private bool _isGrounded;
    private bool _delayReposition;
    private bool _isMoving;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private float turnSmoothVelocity;
    [SerializeField] private float turnSmoothTime = 0.1f;

    public const float METAMORPHOSIS_THRESHOLD_WEIGHT = 150;

    public delegate void OnWinDelegate();
    public event OnWinDelegate OnWin;

    private AudioPlaybackService _audioPlaybackService;

    public PlayerState State { get; private set; }

    public void Awake()
    {
        State = PlayerState.CaterPillar;

        MonoBehaviourLocator.Instance.Register<PlayerCharacter>(this);
    }

    public void Start()
    {
        _audioPlaybackService = ServiceLocator.Instance.Get<AudioPlaybackService>();
        _walkingSoundRoutine = _audioPlaybackService.GetPlayerMovementAudioCoroutine();

        _massChanger = new MassChanger(this);
        _massChangeCoroutine = _massChanger.MassChangeCoroutine();
    }

    private void Update()
    {
        if (OnCanvasEnableDisable._GamePaused)
            return;

        HandleJumpInput();

        //gravity
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        HandleMovementInput();
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
            StartCoroutine(WinCoroutine());
        }
        else
        {
            _rigidbody.mass = newWeight;
        }
    }
    IEnumerator WinCoroutine()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(1f);
        OnWin?.Invoke();
        Time.timeScale = 0;

    }
    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }
    
    private void BecomeButterfly()
    {
        State = PlayerState.Butterfly;

        _caterpillarMesh.SetActive(false);
        _butterflyMesh.SetActive(true);

        StopCoroutine(_massChangeCoroutine);
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    public Light GetLight()
    {
        return _light;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    private void HandleJumpInput()
    {
        if (State == PlayerState.Butterfly) return;

        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && !_isGrounded) // we land
        {
            if (_isMoving)
            {
                StartCoroutine(_walkingSoundRoutine);
            }
        }
        if(!isGrounded && _isGrounded) // we take off
        {
            if (_isMoving)
            {
                _audioPlaybackService.StopAudio(AudioType.PlayerWalking);
                StopCoroutine(_walkingSoundRoutine);
            }
        }

        _isGrounded = isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -4f;
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }
    }

    private void HandleMovementInput()
    {
        if (State == PlayerState.Butterfly) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            if(_isMoving == false && _isGrounded)
            {
                _isMoving = true;
                StartCoroutine(_walkingSoundRoutine);
            }

            freeLookVirtualCam.m_YAxisRecentering.m_enabled = false;
            freeLookVirtualCam.m_RecenterToTargetHeading.m_enabled = false;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _controller.Move(moveDir.normalized * _speed * Time.deltaTime);

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
            if (_isMoving)
            {
                _isMoving = false;
                _audioPlaybackService.StopAudio(AudioType.PlayerWalking);
                StopCoroutine(_walkingSoundRoutine);
            }

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
            _axisRecenteringEnableCoroutineRunning = false;
            yield break;
        }

        freeLookVirtualCam.m_YAxisRecentering.m_enabled = true;
        freeLookVirtualCam.m_RecenterToTargetHeading.m_enabled = true;
        _axisRecenteringEnableCoroutineRunning = false;
        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PhysicsInteractable"))
        {
            Debug.Log(-Vector3.up * _rigidbody.mass);
            collision.rigidbody.AddForceAtPosition(-Vector3.up * _rigidbody.mass, groundCheck.transform.TransformPoint(groundCheck.transform.position)- Vector3.one * .2f);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("PhysicsInteractable")) 
        {
            Debug.Log(-Vector3.up * _rigidbody.mass);
            collision.rigidbody.AddForceAtPosition(-Vector3.up*_rigidbody.mass, groundCheck.transform.TransformPoint(groundCheck.transform.position) - Vector3.one * .2f);
        }
    }
}
