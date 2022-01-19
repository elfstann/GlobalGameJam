using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputScheme input;

    public PhysicsMaterial2D material2D;
    public Rigidbody2D rigidBody;
    public Transform player;

    public LayerMask groundMask;

    public AnimationCurve accelerationCurve;
    
    public float maxAcceleration = 2;
    public float maxSpeed = 5;
    
    public float jumpHeight = 2;
    public float gizmoRadius = 1;
    
    public float groundCheckDistance = 0.2f;
    public float underObjectFindDistance = 0.2f;

    private Vector2 moveDirection = Vector2.zero;
    private bool isLanded = true;
    private float currentPositionOnCurve = 0;
    private RaycastHit2D objectUnder;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(player.position + Vector3.up * jumpHeight , gizmoRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(player.position , Vector3.down * underObjectFindDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(player.position , Vector3.down * groundCheckDistance);
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

    private void FixedUpdate()
    {
        MovePlayer();
        objectUnder = Physics2D.Raycast(player.position, Physics2D.gravity.normalized, underObjectFindDistance, groundMask);
        
        isLanded = Physics2D.Raycast(player.position, Physics2D.gravity.normalized, groundCheckDistance, groundMask);
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
        if (obj.ReadValueAsButton() && isLanded)
        {
            float startYSpeed = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * jumpHeight);
            Vector2 startVelocity = -startYSpeed * Physics2D.gravity.normalized;
            rigidBody.velocity += startVelocity;
        }
    }

    private void StartMovePlayer(InputAction.CallbackContext obj)
    {
        if (Physics2D.Raycast(player.position, Physics2D.gravity.normalized, underObjectFindDistance, groundMask))
        {
            moveDirection = -Vector2.Perpendicular(objectUnder.normal) * obj.ReadValue<float>();
        }
        else
        {
            moveDirection.y = 0;
            moveDirection.x = obj.ReadValue<float>();
        }
        Debug.Log(moveDirection);
    }
    
    

    private void MovePlayer()
    {
        currentPositionOnCurve += ((moveDirection.sqrMagnitude == 0 ? -1 : 1) * Time.deltaTime);
        currentPositionOnCurve = Mathf.Clamp01(currentPositionOnCurve);

        if (moveDirection.sqrMagnitude == 0)
        {
            currentPositionOnCurve = 0;
            return;
        }
        
        if (Mathf.Abs(rigidBody.velocity.x) < maxSpeed)
        {
            var deltaVelocity = moveDirection * (accelerationCurve.Evaluate(currentPositionOnCurve) * maxAcceleration * Time.deltaTime);
            rigidBody.velocity += deltaVelocity;
        }
    }
}
