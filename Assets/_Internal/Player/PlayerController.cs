using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : Singleton<PlayerController>
{
    [Space]
    [Header("Player Settings")]
    public PlayerState currentPlayerState = PlayerState.Rabbit;
    public HealthController rabbitHealthController;
    public HealthController bearHealthController;
    public ItemPicker itemPicker;

    [Header("Components")]
    public PhysicsMaterial2D material2D;
    public Rigidbody2D rigidBody;
    public Transform player;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public Animator bearAnimator;
    public Animator rabbitAnimator;
    public LayerMask groundMask;

    public AnimationCurve accelerationCurve;

    [Space]
    [Header("Speed settings")]
    public float maxAcceleration = 2;
    public float maxSpeed = 5;
    public float reactiveCoefficient = 1.5f;
    public float airDragCoefficient = 0.3f;
    public float sprintMultiplier = 1.7f;


    [Tooltip("For moving in perpendicular to normal touch vector")]
    public float underObjectFindDistance = 0.2f;

    [Space]
    [Header("Jump settings")]
    public float jumpHeight = 2;
    public float jumpCooldown = 0.25f;
    public float safeJumpDelay = 0.2f;
    public float positionsSavingTimeInterval = 0.1f;
    public float groundCheckDistance = 0.2f;
    public float groundCheckRadius = 0.2f;

    [Space]
    [Header("Gizmo settings")]
    public float maxHeightGizmoRadius = 1;

    private bool isPaused = false;

    private Vector2 _moveDirection = Vector2.zero;
    private float _currentPositionOnCurve = 0;
    private float _lastJumpTime = 0;
    private RaycastHit2D _objectUnder;
    private Queue<(Vector3, float)> _positionsSnapshot = new Queue<(Vector3, float)>();
    private float _currentMaxSpeed;
    private float _currentMaxAcceleration;


    private PlayerInputScheme input;
    private bool sprintingPressd;

    private HealthController currentHealthController;
    public HealthController CurrentHealthController { get { return currentHealthController; } }

    public bool Sprinting => sprintingPressd && IsLanded(player.position);

    public bool IsLanded(Vector3 position) => Physics2D.OverlapCircle(position + ((Vector3)Physics2D.gravity.normalized * groundCheckDistance), groundCheckRadius, groundMask);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(player.position + Vector3.up * jumpHeight, maxHeightGizmoRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(player.position, Vector3.down * underObjectFindDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(player.position + Vector3.down * groundCheckDistance, groundCheckRadius);
        Gizmos.color = Color.blue;
        if (Application.isPlaying)
            Gizmos.DrawRay(player.position, rigidBody.velocity * 10);
    }

    private void Awake()
    {
        input = new PlayerInputScheme();

        EnableAllActions();
        SubscribeAllActions();
        _currentMaxAcceleration = maxAcceleration;
        _currentMaxSpeed = maxSpeed;

        PauseManager.Instance.onGamePaused += PausePlayer;
    }

    private void Start()
    {
        SetHealthController();
    }


    private void Update()
    {
        if (isPaused) return;

        if (rigidBody.velocity.x < -0.01)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (rigidBody.velocity.x > 0.01)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        
        _positionsSnapshot.Enqueue((player.position , Time.time));

        if (Time.time - _positionsSnapshot.Peek().Item2 >= safeJumpDelay)
            _positionsSnapshot.Dequeue();

        DecreaseShakeTime();
    }

    private void LateUpdate()
    {
        if (isPaused) return;

        Animations();
    }

    private void PausePlayer(bool state)
    {
        isPaused = state;
    }

    private void SetHealthController()
    {
        if (currentPlayerState == PlayerState.Rabbit)
            currentHealthController = rabbitHealthController;
        else
            currentHealthController = bearHealthController;

        currentHealthController.OnHealthChanged?.Invoke(currentHealthController.CurrentHealth, currentHealthController.HealthImage);
    }

    private void Animations()
    {
        Animator currentAnimator = currentPlayerState == PlayerState.Bear ? bearAnimator : rabbitAnimator;
        currentAnimator.SetBool("Landed" , IsLanded(player.position));
        currentAnimator.SetBool("Move", rigidBody.velocity.sqrMagnitude > 0.001f && IsLanded(player.position));
        currentAnimator.SetFloat("Speed", rigidBody.velocity.magnitude / maxSpeed);
    }

    private void FixedUpdate()
    {
        if (isPaused) return;

        MovePlayer();
        _objectUnder = Physics2D.Raycast(player.position, Physics2D.gravity.normalized, underObjectFindDistance, groundMask);
    }

    private void EnableAllActions()
    {
        input.Player.Move.Enable();   
        input.Player.Jump.Enable();
        input.Player.Sprint.Enable();
        input.Player.Fire.Enable();
        input.Player.SwapHero.Enable();
    }

    private void SubscribeAllActions()
    {
        input.Player.Move.started += StartMovePlayer;
        input.Player.Move.canceled += StartMovePlayer;
        input.Player.Jump.performed += Jump;
        input.Player.Sprint.performed += Sprint;
        input.Player.Fire.performed += Fire;
        input.Player.SwapHero.performed += SwapHero;
    }

    private void SwapHero(InputAction.CallbackContext obj)
    {
        currentPlayerState = (PlayerState) (1 - ((int)currentPlayerState));
        SetHealthController();
        Debug.Log(currentPlayerState);
        rabbitAnimator.gameObject.SetActive(currentPlayerState == PlayerState.Rabbit);
        bearAnimator.gameObject.SetActive(currentPlayerState == PlayerState.Bear);
        currentHealthController.OnHealthChanged?.Invoke(currentHealthController.CurrentHealth, currentHealthController.HealthImage);
    }

    private void Fire(InputAction.CallbackContext obj)
    {
        if (obj.ReadValueAsButton() && currentPlayerState == PlayerState.Bear) bearAnimator.SetTrigger("Attack");
    }

    private void Sprint(InputAction.CallbackContext obj)
    {
        if (obj.ReadValueAsButton())
        {
            sprintingPressd = true;
        }
        else
        {
            sprintingPressd = false;
        }
        SetSpeed();
        Debug.Log(sprintingPressd);
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if (Time.time - _lastJumpTime < jumpCooldown) return;
        if (!obj.ReadValueAsButton()) return;

        if(!IsLanded(player.position) && !IsLanded(_positionsSnapshot.Peek().Item1)) return;
        
        if (currentPlayerState == PlayerState.Rabbit)   
            StartCoroutine(StartJump());
    }

    private IEnumerator StartJump()
    {
        rabbitAnimator.SetTrigger("Jump");

        yield return new WaitForSeconds(1f / 6f);
        
        float startYSpeed = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * jumpHeight);
        Vector2 startVelocity = -startYSpeed * Physics2D.gravity.normalized;
        
        
        var rigidBodyVelocity = rigidBody.velocity;
        if (IsLanded(_positionsSnapshot.Peek().Item1) && !IsLanded(player.position))
            rigidBodyVelocity.y = startVelocity.y;
        else
            rigidBodyVelocity.y += startVelocity.y;

        _lastJumpTime = Time.time;
        rigidBody.velocity = rigidBodyVelocity;
    }

    private void StartMovePlayer(InputAction.CallbackContext obj)
    {
        if (Physics2D.Raycast(player.position, Physics2D.gravity.normalized, underObjectFindDistance, groundMask))
        {
            _moveDirection = -Vector2.Perpendicular(_objectUnder.normal) * obj.ReadValue<float>();
        }
        else
        {
            _moveDirection.y = 0;
            _moveDirection.x = obj.ReadValue<float>();
        }
    }



    private void MovePlayer()
    {
        _currentPositionOnCurve += ((_moveDirection.sqrMagnitude == 0 ? -1 : 1) * Time.deltaTime);
        _currentPositionOnCurve = Mathf.Clamp01(_currentPositionOnCurve);

        SetSpeed();

        if (_moveDirection.sqrMagnitude == 0)
        {
            _currentPositionOnCurve = 0;
            return;
        }

        if (Mathf.Abs(rigidBody.velocity.x) < _currentMaxSpeed)
        {
            bool isOppositeMove = Mathf.Approximately(Mathf.Sign(rigidBody.velocity.x) * Mathf.Sign(_moveDirection.x), -1);
            float currentSpeed = ((isOppositeMove ? reactiveCoefficient : 1) * _currentMaxAcceleration * accelerationCurve.Evaluate(_currentPositionOnCurve) * Time.deltaTime);
            var deltaVelocity = _moveDirection * (currentSpeed * (IsLanded(player.position) ? 1 : airDragCoefficient));
            rigidBody.velocity += deltaVelocity;
        }
    }

    private void SetSpeed()
    {
        if (Sprinting)
        {
            _currentMaxSpeed = maxSpeed * sprintMultiplier;
            _currentMaxAcceleration = maxAcceleration * sprintMultiplier;
        }
        else
        {
            _currentMaxSpeed = maxSpeed / sprintMultiplier;
            _currentMaxAcceleration = maxAcceleration / sprintMultiplier;
        }
    }

    CinemachineBasicMultiChannelPerlin perlin;
    float shakeTime = 0;
    public void ShakeCamera(float intensity, float time)
    {
        perlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        shakeTime = time;
    }

    private void DecreaseShakeTime()
    {
        if (shakeTime <= 0f)
            return;

        shakeTime -= Time.deltaTime;
        if(shakeTime<=0f) perlin.m_AmplitudeGain = 0;
    }

    private void OnDestroy()
    {
        PauseManager.Instance.onGamePaused -= PausePlayer;
    }

}

public enum PlayerState
{
    Rabbit, Bear
}
