using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Components")]
    public PhysicsMaterial2D material2D;
    public Rigidbody2D rigidBody;
    public Transform player;
    
    public LayerMask groundMask;

    public AnimationCurve accelerationCurve;
    
    [Space]
    [Header("Speed settings")]
    public float maxAcceleration = 2;
    public float maxSpeed = 5;
    public float reactiveCoefficient = 1.5f;
    public float airDragCoefficient = 0.3f;
    
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
    
    
    private Vector2 _moveDirection = Vector2.zero;
    private float _currentPositionOnCurve = 0;
    private float _lastJumpTime = 0;
    private RaycastHit2D _objectUnder;
    private Queue<(Vector3 , float)> _positionsSnapshot = new Queue<(Vector3, float)>();
    
    private PlayerInputScheme input;
    
    public bool IsLanded(Vector3 position) => Physics2D.OverlapCircle(position + ((Vector3)Physics2D.gravity.normalized * groundCheckDistance) , groundCheckRadius , groundMask);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(player.position + Vector3.up * jumpHeight , maxHeightGizmoRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(player.position , Vector3.down * underObjectFindDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(player.position + Vector3.down * groundCheckDistance , groundCheckRadius);
        Gizmos.color = Color.blue;
        if (Application.isPlaying)
            Gizmos.DrawRay(player.position , rigidBody.velocity * 10);
    }

    private void Awake()
    {
        input = new PlayerInputScheme();
        
        EnableAllActions();
        SubscribeAllActions();
    }

    private void Update()
    {
        _positionsSnapshot.Enqueue((player.position , Time.time));
        if (Time.time - _positionsSnapshot.Peek().Item2 >= safeJumpDelay)
            _positionsSnapshot.Dequeue();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        _objectUnder = Physics2D.Raycast(player.position, Physics2D.gravity.normalized, underObjectFindDistance, groundMask);
    }

    private void SubscribeAllActions()
    {
        input.Player.Move.started += StartMovePlayer;
        input.Player.Move.canceled += StartMovePlayer;
        input.Player.Jump.performed += Jump;
    }

    private void EnableAllActions()
    {
        input.Player.Move.Enable();   
        input.Player.Jump.Enable();   
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if (Time.time - _lastJumpTime < jumpCooldown) return;
        if (!obj.ReadValueAsButton()) return;
        if(!IsLanded(player.position) && !IsLanded(_positionsSnapshot.Peek().Item1)) return;
        
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

        if (_moveDirection.sqrMagnitude == 0)
        {
            _currentPositionOnCurve = 0;
            return;
        }
        
        if (Mathf.Abs(rigidBody.velocity.x) < maxSpeed)
        {
            bool isOppositeMove = Mathf.Approximately(Mathf.Sign(rigidBody.velocity.x) * Mathf.Sign(_moveDirection.x) , -1);
            Debug.Log(isOppositeMove);
            float currentSpeed = ((isOppositeMove ? reactiveCoefficient : 1) * maxAcceleration * accelerationCurve.Evaluate(_currentPositionOnCurve) * Time.deltaTime);
            var deltaVelocity = _moveDirection * (currentSpeed * (IsLanded(player.position)? 1 : airDragCoefficient));
            rigidBody.velocity += deltaVelocity;
        }
    }
}
