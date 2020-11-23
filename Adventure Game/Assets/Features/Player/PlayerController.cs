using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PhysicsCheck))]
public class PlayerController : MonoBehaviour
{
    public Rigidbody2D RigidBody { get; private set; }

    private BoxCollider2D boxCollider;

    private PhysicsCheck physicsCheck;

    private PlayerStateMachine stateMachine;
    
    [Header("Movement")]
    [Space]
    [Header("Idle")]
    [Header("Jumping")]
    [Tooltip("Vertical force applied to the Rigidbody on jump")]
    public float jumpForce = 100f;

    [Header("Falling")]
    [Tooltip("Gravity multiplier while falling.")]
    [Range(0, 10)]
    public float fallMultiplier = 2.5f;
    [Tooltip("Gravity multiplier while jumping after the jump button is released.")]
    [Range(0, 10)] 
    public float lowJumpMultiplier = 2f;
    [Tooltip("Time the player can queue a jump before a collision with the ground")]
    public float forgiveJumpSeconds; // can help make jumping more responsive





    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();

        boxCollider = GetComponent<BoxCollider2D>();

        physicsCheck = GetComponent<PhysicsCheck>();

        stateMachine = new PlayerStateMachine(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        stateMachine.ChangeState(new PlayerIdleState());
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        stateMachine.OnCollisionEnter(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        stateMachine.OnCollisionExit(collision);
    }
    public void ChangeState(IPlayerMovementState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public bool IsFalling()
    {
        return physicsCheck.IsFalling();
    }

    public bool IsGrounded()
    {
        return physicsCheck.IsGrounded();
    }

}
